using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AspnetShoppingCart.Models;

namespace AspnetShoppingCart.Controllers
{
    public class HomeController : Controller
    {
        // 建立可存取 ShoppingCart.mdf 資料庫的 ShoppingCartEntities 類別物件 db
        ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: Home/Index
        public ActionResult Index()
        {
            // 取得所有產品放入 products
            var products = db.Product.ToList();

            // 若 Session["Member"] 為空，表示會員未登入
            if (Session["Member"] == null)
            {
                // 指定 Index.cshtml 套用 _Layout.cshtml，View 使用 products
                return View("Index", "_Layout", products);
            }

            // 會員登入狀態
            // 指定 Index.cshtml 套用 _LayoutMember.cshtml，View 使用 products
            return View("Index", "_LayoutMember", products);
        }
    }
}