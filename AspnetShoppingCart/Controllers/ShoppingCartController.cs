using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AspnetShoppingCart.Models;

namespace AspnetShoppingCart.Controllers
{
    public class ShoppingCartController : Controller
    {
        // 建立可存取 ShoppingCart.mdf 資料庫的 ShoppingCartEntities 類別物件 db
        ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: ShoppingCart/Index
        public ActionResult Index()
        {
            // 取得登入會員帳號並指定給 userId
            string userId = (Session["Member"] as Member).UserId;

            // 取得未成為訂單明細的資料，即購物車內容
            var orderDetails = db.OrderDetail
                .Where(m => m.UserId == userId && m.IsApproved == "否")
                .ToList();

            // 指定 Index.cshtml 套用 _LayoutMember.cshtml，View 使用 orderDetails
            return View("Index", "_LayoutMember", orderDetails);
        }

        // GET: ShoppingCart/Add
        public ActionResult Add(string pId)
        {
            // 取得登入會員帳號並指定給 userId
            string userId = (Session["Member"] as Member).UserId;

            // 找出會員放入訂單明細的產品
            // 該產品的 IsApproved 為 "否"，表示該產品是購物車狀態
            var currentCart = db.OrderDetail
                .Where(m => m.PId == pId && m.IsApproved == "否" && m.UserId == userId)
                .FirstOrDefault();

            // 若 currentCart 等於 null，表示會員選購的產品不是購物車狀態
            if (currentCart == null)
            {
                // 找出目前選購的產品並指定給 product
                var product = db.Product.Where(m => m.PId == pId).FirstOrDefault();
                // 將產品放入訂單明細，因為產品的 IsApproved 為 "否"，表示為購物車狀態
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.PId = product.PId;
                orderDetail.Name = product.Name;
                orderDetail.Price = product.Price;
                orderDetail.Quantity = 1;
                orderDetail.IsApproved = "否";
                orderDetail.UserId = userId;
                db.OrderDetail.Add(orderDetail);
            }
            else
            {
                // 若產品為購物車狀態，即將該產品數量加 1
                currentCart.Quantity += 1;
            }
            db.SaveChanges();

            // 執行 SoppingCart 控制器的 Index 動作方法
            return RedirectToAction("Index");
        }
    }
}