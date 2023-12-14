using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Braintree;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_Models;
using Rocky_Models.ViewModels;
using Rocky_Utility;
using Rocky_Utility.BrainTree;
namespace Rocky.Controllers
{
    [Authorize(Roles =WC.AdminRole)]
    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository _orderHRepo;
        private readonly IOrderDetailRepository _orderDRepo;
        private readonly IBrainTreeGate _brain;
       
        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderController(
        IOrderHeaderRepository orderHRepo, IOrderDetailRepository orderDRepo, IBrainTreeGate brain)
        {
            _brain = brain;
            _orderDRepo = orderDRepo;
            _orderHRepo = orderHRepo;
        }


        public IActionResult Index(string searchName = null, string searchEmail = null, string searchPhone = null, string Status=null)
        {
            OrderListVM orderListVM = new OrderListVM()
            {
                OrderHList = _orderHRepo.GetAll(),
                StatusList = WC.listStatus.ToList().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };

            if (!string.IsNullOrEmpty(searchName))
            {
                orderListVM.OrderHList = orderListVM.OrderHList.Where(u => u.FullName.ToLower().Contains(searchName.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchEmail))
            {
                orderListVM.OrderHList = orderListVM.OrderHList.Where(u => u.Email.ToLower().Contains(searchEmail.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchPhone))
            {
                orderListVM.OrderHList = orderListVM.OrderHList.Where(u => u.PhoneNumber.ToLower().Contains(searchPhone.ToLower()));
            }
            if (!string.IsNullOrEmpty(Status) && Status!= "--Order Status--")
            {
                orderListVM.OrderHList = orderListVM.OrderHList.Where(u => u.OrderStatus.ToLower().Contains(Status.ToLower()));
            }

            return View(orderListVM);
        }


        public IActionResult Details(int id)
        {
            OrderVM = new OrderVM()
            {
                OrderHeader = _orderHRepo.FirstOrDefault(u => u.Id == id),
                OrderDetail = _orderDRepo.GetAll(o => o.OrderHeaderId == id, includeProperties: "Product")
            };

            return View(OrderVM);
        }

        [HttpPost]
        public IActionResult StartProcessing()
        {
            OrderHeader orderHeader = _orderHRepo.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = WC.StatusInProcess;
            _orderHRepo.Save();
            TempData[WC.Success] = "Order is In Process";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ShipOrder()
        {
            OrderHeader orderHeader = _orderHRepo.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = WC.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;
            _orderHRepo.Save();
            TempData[WC.Success] = "Order Shipped Successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult CancelOrder()
        {
            OrderHeader orderHeader = _orderHRepo.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);

            var gateway = _brain.GetGateway();
            /*Transaction transaction = gateway.Transaction.Find(orderHeader.TransactionId);

           if(transaction.Status == TransactionStatus.AUTHORIZED || transaction.Status == TransactionStatus.SUBMITTED_FOR_SETTLEMENT)
            {
                //no refund
                Result<Transaction> resultvoid = gateway.Transaction.Void(orderHeader.TransactionId);
            }
            else
            {
                //refund
                Result<Transaction> resultRefund = gateway.Transaction.Refund(orderHeader.TransactionId);
            }*/
            orderHeader.OrderStatus = WC.StatusRefunded;
            _orderHRepo.Save();
            TempData[WC.Success] = "Order Cancelled Successfully";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        
        [HttpPost]
        public IActionResult UpdateOrderDetails()
        {
            OrderHeader orderHeaderFromDb = _orderHRepo.FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderFromDb.FullName = OrderVM.OrderHeader.FullName;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.State = OrderVM.OrderHeader.State;
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
            orderHeaderFromDb.Email = OrderVM.OrderHeader.Email;

            _orderHRepo.Save();
            TempData[WC.Success] = "Order Details Updated Successfully";

            return RedirectToAction("Details","Order",new { id=orderHeaderFromDb.Id});
        }
        public IActionResult CreateNote(OrderVM orderVM)
        {
            //Излишние запросы, но OrderVM orderVM лист product null при передаче в IActionResult


            orderVM = new OrderVM()
            {
                OrderHeader = _orderHRepo.FirstOrDefault(u => u.Id == orderVM.OrderHeader.Id),
                OrderDetail = _orderDRepo.GetAll(
                o => o.OrderHeaderId == orderVM.OrderHeader.Id,
                includeProperties: "Product,Product.Category,Product.ApplicationType,Product.LoadingWarehouse,Product.UnloadingWarehouse")
            };


            var orderHeader = orderVM.OrderHeader; // получение данных заголовка заказа
            IEnumerable<OrderDetail> orderDetails = orderVM.OrderDetail; // получение списка деталей заказа

            var excelStream = CreateExcelDocument(orderHeader, orderDetails);

            return File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrderDetails.xlsx");
        }
        public MemoryStream CreateExcelDocument(OrderHeader orderHeader, IEnumerable<OrderDetail> orderDetails)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Заказ");

            // Форматирование заголовков
            var headers = new[] { "ID", "Описание", "Сроки доставки", "Оплата", "Тип товара", "Тип доставки", "Склад загрузки", "Склад разгрузки", "Тип доставки" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                worksheet.Cell(1, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            int currentRow = 2;
            foreach (var detail in orderDetails)
            {
                // Заполнение данных с форматированием
                worksheet.Cell(currentRow, 1).Value = detail.Id;

                string description = Regex.Replace(detail.Product.Description, "<.*?>", String.Empty);

                worksheet.Cell(currentRow, 2).Value = WebUtility.HtmlDecode(description);
                worksheet.Cell(currentRow, 3).Value = detail.Product.DeliveryTime;
                worksheet.Cell(currentRow, 4).Value = detail.Product.Price;
                worksheet.Cell(currentRow, 5).Value = detail.Product.Category.Name;
                worksheet.Cell(currentRow, 6).Value = detail.Product.ApplicationType.Name;
                worksheet.Cell(currentRow, 7).Value = detail.Product.LoadingWarehouse.Name;
                worksheet.Cell(currentRow, 8).Value = detail.Product.UnloadingWarehouse.Name;
                worksheet.Cell(currentRow, 9).Value = detail.Product.ApplicationType.Name;
                worksheet.Row(currentRow).Height = worksheet.Row(currentRow).Height * 4; // Увеличение в 2 раза

                for (int i = 1; i <= headers.Length; i++)
                {
                    worksheet.Cell(currentRow, i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(currentRow, i).Style.Border.BottomBorderColor = XLColor.DarkGray;
                    worksheet.Cell(currentRow, i).Style
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center) // Выравнивание по центру по вертикали
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left); // Выравнивание по левому краю
                }

                currentRow++;
            }

            // Формула для подсчета общей суммы
            worksheet.Cell(currentRow + 1, 4).FormulaA1 = $"=SUM(D2:D{currentRow})";

            // Дополнительная информация
            worksheet.Cell(currentRow, 1).Value = "Исполнитель:";
            worksheet.Cell(currentRow, 2).Value = orderHeader.FullName;
            worksheet.Cell(currentRow, 3).Value = orderHeader.PhoneNumber;

            // Авторазмер для колонок
            worksheet.Columns().AdjustToContents();

            // Форматирование для подсветки итоговой строки
            for (int i = 1; i <= headers.Length; i++)
            {
                worksheet.Cell(currentRow + 1, i).Style.Font.Bold = true;
                worksheet.Cell(currentRow + 1, i).Style.Fill.BackgroundColor = XLColor.Aqua;
            }

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return stream;
        }

    }
}
