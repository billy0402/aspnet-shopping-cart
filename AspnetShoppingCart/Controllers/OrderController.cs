using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AspnetShoppingCart.Models;

namespace AspnetShoppingCart.Controllers
{
    public class OrderController : Controller
    {
        // 建立可存取 ShoppingCart.mdf 資料庫的 ShoppingCartEntities 類別物件 db
        ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: Order/Index
        public ActionResult Index()
        {
            // 找出會員帳號並指並給 userId
            string userId = (Session["Member"] as Member).UserId;

            // 找出目前會員的所有訂單主檔紀錄並依照 Date 進行遞增排序
            // 將查詢結果指定給 orders
            var orders = db.Order.Where(m => m.UserId == userId).OrderByDescending(m => m.Date).ToList();

            // 指定 Index.cshtml 套用 _LayoutMember.cshtml，View 使用 orders
            return View("Index", "_LayoutMember", orders);
        }

        // GET: Order/Detail
        public ActionResult Detail(string orderGuid)
        {
            // 根據 orderGuid 找出和訂單主檔關聯的訂單明細，並指定給 orderDetails
            var orderDetails = db.OrderDetail.Where(m => m.OrderGuid == orderGuid).ToList();


            // 指定 Detail.cshtml 套用 _LayoutMember.cshtml，View 使用 orderDetails
            return View("Detail", "_LayoutMember", orderDetails);
        }

        public ActionResult Add(string receiver, string email, string address)
        {
            // 找出會員帳號並指並給 userId
            string userId = (Session["Member"] as Member).UserId;
            // 建立唯一的識別值給 guid 變數，用來當作訂單編號
            // Order 的 OrderGuid 欄位會關連到 OrderDetail 的 OrderGuid 欄位
            // 形成一對多的關係，即一筆訂單資料會對應到多筆訂單明細
            string guid = Guid.NewGuid().ToString();

            // 建立訂單主檔資料
            Order order = new Order();
            order.OrderGuid = guid;
            order.UserId = userId;
            order.Receiver = receiver;
            order.Email = email;
            order.Address = address;
            order.Date = DateTime.Now;
            db.Order.Add(order);

            // 找出目前會員在訂單明細中是購物車狀態的產品
            var cartList = db.OrderDetail
                .Where(m => m.IsApproved == "否" && m.UserId == userId)
                .ToList();
            // 將購物車狀態產品的 IsApproved 設為 "是"，表示確認訂購產品
            foreach (var item in cartList)
            {
                item.OrderGuid = guid;
                item.IsApproved = "是";
            }

            // 更新資料庫，異動 Order 和 OrderDetail
            // 完成訂單主檔和訂單明細的更新
            db.SaveChanges();

            // 執行 SoppingCart 控制器的 Index 動作方法
            return RedirectToAction("Index");
        }
    }
}