using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWebBanHang.Models;
using System.Data.Entity;
using PagedList;
using System.Net;

namespace DemoWebBanHang.Controllers
{
    public class ShoppingCartController : Controller
    {
        private WebBanHangEntities de = new WebBanHangEntities();

        public ActionResult Index()
        {
            return View();
        }

        private int isExisting(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Pr.ID_SanPham == id)
                    return i;
            return -1;
        }

        public ActionResult Delete(int id)
        {
            int index = isExisting(id);
            List<Item> cart = (List<Item>)Session["cart"];
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return View("Cart");
        }
        public ActionResult Update(FormCollection fc)
        {
            string[] quantites = fc.GetValues("quantity");
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                cart[i].Quantity = Convert.ToInt32(quantites[i]);
            Session["cart"] = cart;
            return View("Cart");
        }

        public ActionResult OrderNow(int id)
        {
            if(Session["cart"]==null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item(de.SanPhams.Find(id),1));
                Session["cart"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int index = isExisting(id);
                if (index == -1)
                    cart.Add(new Item(de.SanPhams.Find(id), 1));
                else
                    cart[index].Quantity++;
                Session["cart"] = cart;
            }
            return View("Cart");
        }
        public ActionResult Checkout()
        {
            return View("Checkout");
        }
        public ActionResult saveOrder(FormCollection fc)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            //Lưu đơn đặt hàng
            HoaDon hd = new HoaDon();
            hd.NgayThanhToan = DateTime.Now;
            hd.HoTen = fc["UserName"];
            hd.DiaChi = fc["DiaChi"];
            hd.SoDienThoai = fc["SoDienThoai"];
            hd.TongTien = fc["TongTien"];
            hd.TenHoaDon = "Đơn hàng mới";
            de.HoaDons.Add(hd);
            de.SaveChanges();
            foreach(Item Item in cart)
            {
                ThongTinHoaDon tthd = new ThongTinHoaDon();
                tthd.ID_HoaDon = hd.ID_HoaDon;
                tthd.ID_SanPham = Item.Pr.ID_SanPham;
                tthd.DonGia = Item.Pr.DonGia;
                tthd.Quantity = Item.Quantity;
                //Update số lượng
                foreach (SanPham product in de.SanPhams.Where(x => x.ID_SanPham == Item.Pr.ID_SanPham))
                {
                    product.SoLuong = Item.Pr.SoLuong - tthd.Quantity;
                    if (product.SoLuong < 0)
                    {
                        ViewBag.DuplicateMessage = "Vượt quá số lượng trong kho";
                        return View("Cart");
                    }
                }
                de.ThongTinHoaDons.Add(tthd);
                de.SaveChanges();
            }
            //Xóa tất cả giỏ hàng khi thanh toán
            Session.Remove("cart");
            return View("Thanks");
        }
        public ActionResult OrderDetail(int? page)
        {
            return View(de.HoaDons.OrderByDescending(x => x.ID_HoaDon).ToPagedList(page ?? 1, 3));
        }
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = de.HoaDons.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
    }
}