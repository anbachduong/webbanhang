using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWebBanHang.Models;
using System.IO;
using System.Net;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;

namespace DemoWebBanHang.Controllers
{
    public class HomeController : Controller
    {
        private WebBanHangEntities db = new WebBanHangEntities();
        public ActionResult Index()
        {
            return View(db.SanPhams.ToList());
        }
        public ActionResult AllProduct(string searching)
        {
            return View(db.SanPhams.Where(x => x.TenSanPham.Contains(searching) || searching == null).ToList());
        }
        public ActionResult Menu()
        {
            List<DanhMuc> cList = db.DanhMucs.ToList();
            return PartialView(cList);
        }
        public ActionResult Category(long CategoryID)
        {
            List<SanPham> cList = db.SanPhams.Where(x => x.ID_DanhMuc==CategoryID).ToList();
            return View(cList);
        }
        public ActionResult ProductDetail(long ProductID)
        {
            SanPham product = db.SanPhams.Where(x => x.ID_SanPham == ProductID).SingleOrDefault();
            return View(product);
        }
        public ActionResult ListProduct(int? page,string searchinglp,string searchBy)
        {
            return View(db.SanPhams.Where(x => x.TenSanPham.Contains(searchinglp) || searchinglp == null).ToList().ToPagedList(page ?? 1, 10));
        }

        //Tạo sản phẩm
        [HttpGet]

        public ActionResult Create()

        {

            return View();

        }
        public ActionResult ListDanhMuc()
        {


            var items = db.DanhMucs.ToList();
            if (items != null)
            {
                ViewBag.data = items;
            }

            return View();
        }

        [HttpPost]

        public ActionResult Create(SanPham model, HttpPostedFileBase uploadedfile)

        {

            if (ModelState.IsValid)

            {
                
                using (WebBanHangEntities dbModel = new WebBanHangEntities())
                {
                    //string mota = Request.Form["MoTa"];
                    //mota = model.MoTa;
                    string path = "";
                    if (uploadedfile != null && uploadedfile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(uploadedfile.FileName);
                        //path = Path.Combine(Server.MapPath("~/Content/images/products"), fileName);
                        string pathsave = Server.MapPath("~/Content/images/products") + "/" + fileName;
                        path = "/Content/images/products" + "/" + fileName;
                        uploadedfile.SaveAs(pathsave);
                    }
                    model.HinhAnh = path;
                    if (dbModel.SanPhams.Any(x => x.ID_SanPham == model.ID_SanPham))
                    {
                        ViewBag.DuplicateMessage = "Mã sản phẩm đã tồn tại!";
                        return View("Create", model);
                    }
                    db.SanPhams.Add(model);

                    db.SaveChanges();
                }
                return RedirectToAction("ListProduct");

            }

            return View(model);

        }

        //Sửa sản phẩm
        public ActionResult Edit(int id)
        {
            using (WebBanHangEntities dbModel = new WebBanHangEntities())
            {
                return View(dbModel.SanPhams.Where(x => x.ID_SanPham == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, SanPham sp, HttpPostedFileBase uploadedfile)
        {
            
            try
            {
                using (WebBanHangEntities dbModel = new WebBanHangEntities())
                {
                    string path = "";
                    if (uploadedfile != null && uploadedfile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(uploadedfile.FileName);
                        //path = Path.Combine(Server.MapPath("~/Content/images/products"), fileName);
                        string pathsave = Server.MapPath("~/Content/images/products") + "/" + fileName;
                        path = "/Content/images/products" + "/" + fileName;
                        uploadedfile.SaveAs(pathsave);
                    }
                    sp.HinhAnh = path;
                    dbModel.Entry(sp).State = EntityState.Modified;
                    dbModel.SaveChanges();
                }
                return RedirectToAction("ListProduct");
            }
            catch
            {
                return View();
            }
        }
        //Xóa sản phẩm
        public ActionResult Delete(int id)
        {
            using (WebBanHangEntities dbModel = new WebBanHangEntities())
            {
                return View(dbModel.SanPhams.Where(x => x.ID_SanPham == id).FirstOrDefault());
            }
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (WebBanHangEntities dbModel = new WebBanHangEntities())
                {
                    SanPham sp = dbModel.SanPhams.Where(x => x.ID_SanPham == id).FirstOrDefault();
                    dbModel.SanPhams.Remove(sp);
                    dbModel.SaveChanges();
                }
                return RedirectToAction("ListProduct");
            }
            catch
            {
                return View();
            }
            
        }
        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(KhachHang objUser)
        {
            if (ModelState.IsValid)
            {
                using (WebBanHangEntities db = new WebBanHangEntities())
                {
                    var obj = db.KhachHangs.Where(a => a.TaiKhoan.Equals(objUser.TaiKhoan) && a.MatKhau.Equals(objUser.MatKhau)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.MaKH.ToString();
                        Session["UserName"] = obj.HoTen.ToString();
                        Session["DiaChi"] = obj.DiaChi.ToString();
                        Session["SoDienThoai"] = obj.DienThoai.ToString();
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        objUser.LoginErrorMesseger = "Tài khoản hoặc mật khẩu không đúng";
                        return View("Login",objUser);
                    }
                }
            }
            return View(objUser);
        }
        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index","Home");
        }

        //Đăng ký
        [HttpGet]

        public ActionResult DangKy()

        {

            return View();

        }

        [HttpPost]
        public ActionResult DangKy (KhachHang custom)
        {
            if (ModelState.IsValid)
            {
                using (WebBanHangEntities dbModel = new WebBanHangEntities())
                {
                    if (dbModel.KhachHangs.Any(x => x.TaiKhoan == custom.TaiKhoan))
                    {
                        ViewBag.DuplicateMessage = "Người dùng đã tồn tại";
                        return View("DangKy", custom);
                    }
                    db.KhachHangs.Add(custom);
                    db.SaveChanges();
                }
                ViewBag.SuccessMessage = "Đăng ký thành công!";
            }
            return View("DangKy", new KhachHang());
        }

    }
}