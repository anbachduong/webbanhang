﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoWebBanHang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            this.ThongTinHoaDons = new HashSet<ThongTinHoaDon>();
        }
    
        [Required(ErrorMessage = "Mã sản phẩm không được bỏ trống")]
        public int ID_SanPham { get; set; }
        public Nullable<int> ID_DanhMuc { get; set; }
        public Nullable<int> MaXuatXu { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm không được bỏ trống")]
        public string TenSanPham { get; set; }
        public string MoTa { get; set; }
        public Nullable<int> SoLuong { get; set; }
        public string HinhAnh { get; set; }
        public Nullable<int> DonGia { get; set; }
        public Nullable<System.DateTime> NgayCapNhat { get; set; }
    
        public virtual DanhMuc DanhMuc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThongTinHoaDon> ThongTinHoaDons { get; set; }
    }
}
