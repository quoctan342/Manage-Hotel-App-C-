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
            string mahd = this.txtMaHoaDon.Text;
            if (txtMaHoaDon.Text == "")
            {
                MessageBox.Show("Bạn chưa điền mã hóa đơn cần tìm.");
                return;
            }
            var result = from c in db.HoaDonTTs where (c.MaHoaDon == mahd && c.Xoa == 0) select c;
            if (result.Count() == 0)
            {
                MessageBox.Show("Hóa đơn thanh toán này không tồn tại.");
                return;
            }

            this.fh(this.txtMaHoaDon.Text);
            this.Close();        
        }

    }
}
