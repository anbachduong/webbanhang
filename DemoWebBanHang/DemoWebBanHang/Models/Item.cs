using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DemoWebBanHang.Models;

namespace DemoWebBanHang.Models
{
    public class Item
    {
        private SanPham pr = new SanPham();
        public SanPham Pr
        {
            get { return pr; }
            set { pr = value; }
        }
        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public Item()
        { }
        public Item(SanPham product, int quantity)
        {
            this.pr = product;
            this.quantity = quantity;
        }
    }
}