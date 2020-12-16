using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AspnetShoppingCart.Models;

namespace AspnetShoppingCart.Controllers
{
    public class AuthController : Controller
    {
        // 建立可存取 ShoppingCart.mdf 資料庫的 ShoppingCartEntities 類別物件 db
        ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: Auth/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        public ActionResult Login(string userId, string password)
        {
            // 依帳密取得會員並指定給 member
            var member = db.Member.Where(m => m.UserId == userId && m.Password == password).FirstOrDefault();
            // 若 member 為 null，表示會員未註冊
            if (member == null)
            {
                ViewBag.Message = "帳密錯誤，登入失敗";
                return View();
            }

            // 使用 Session 變數紀錄使用者名稱
            Session["Username"] = member.Name;
            // 使用 Session 變數紀錄登入的會員物件
            Session["Member"] = member;
            // 執行 Home 控制器的 Index 動作方法
            return RedirectToAction("Index", "Home");
        }
    }
}