using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSan.TraCuuPhong
{
    public partial class TraCuuPhong : Form
    {
        QLKHACHSANEntities db = new QLKHACHSANEntities();
        BindingSource listphong = new BindingSource();
        public TraCuuPhong()
        {
            InitializeComponent();
           

        }
       
        private void btnTk_Click(object sender, EventArgs e)
        {
           

            string tenp = txtPhong.Text;
            var result2 = from c in db.Phongs
                          from ct in db.LoaiPhongs
                          where c.MaLoaiPhong == ct.MaLoaiPhong && c.TenPhong == tenp
                         select new { c.MaPhong, c.TenPhong, c.MaLoaiPhong, ct.DonGia, c.TinhTrang };
            if (result2.Count() == 0)
            {
                
                MessageBox.Show("Không tìm phòng!");
                
            }
            else
            {
                MessageBox.Show("Tra cứu thành công!");
                dgvTracuu.DataSource = result2.ToList();
            }
        }
    }
}
