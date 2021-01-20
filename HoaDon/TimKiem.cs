using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSan.HoaDon
{
    public partial class TimKiem : Form
    {
        QLKHACHSANEntities db = new QLKHACHSANEntities();
        public FHD fh;
        public TimKiem(FHD sender)
        {
            InitializeComponent();
            this.fh = sender;
        }

        void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnTimKiem_Click(object sender, EventArgs e)
        {
            this.fh(this.cbbHoaDon.Text);
            this.Close();        
        }

        private void TimKiem_Load(object sender, EventArgs e)
        {

        }

        private void txtTenKH_Leave(object sender, EventArgs e)
        {
            var result = from c in db.HoaDonTTs
                         from d in db.KhachHangs
                         where (d.TenKhachHang == txtTenKH.Text && d.MaKhachHang == c.MaKhachhang)
                         select new
                         {
                             c.MaHoaDon
                         };
            cbbHoaDon.DataSource = result.ToList();
            cbbHoaDon.DisplayMember = "MaHoaDon";
            cbbHoaDon.ValueMember = "MaHoaDon";
        }
    }
}
