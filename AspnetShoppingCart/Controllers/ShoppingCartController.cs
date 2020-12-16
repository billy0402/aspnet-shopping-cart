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
    }
}