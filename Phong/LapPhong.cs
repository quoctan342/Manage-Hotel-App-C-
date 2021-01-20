using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSan.Phog
{
    public partial class LapPhong : Form
    {
        QLKHACHSANEntities db = new QLKHACHSANEntities();
        public LapPhong()
        {
            InitializeComponent();
            ShowCBB();
            string maphong = cbbLP.SelectedValue.ToString();
            LoaiPhong lp = db.LoaiPhongs.Find(maphong);
            txtDonGia.Text = lp.DonGia.ToString();
        }

        void ShowCBB()
        {
            var result = from c in db.LoaiPhongs select c;
            cbbLP.DataSource = result.ToList();
            cbbLP.DisplayMember = "MaLoaiPhong";
            cbbLP.ValueMember = "MaLoaiPhong";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = from c in db.Phongs select c;
            Phong ph = new Phong()
            {
                MaPhong = "PH" + (result.Count() + 1),
                TinhTrang = "Trống",
                MaLoaiPhong = cbbLP.SelectedValue.ToString(),
                TenPhong = txtTenPhong.Text
            };
            db.Phongs.Add(ph);
            db.SaveChanges();
            MessageBox.Show("Tạo phòng mới thành công");
        }

        private void cbbLP_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maphong = cbbLP.SelectedValue.ToString();
            LoaiPhong lp = db.LoaiPhongs.Find(maphong);
            txtDonGia.Text = lp.DonGia.ToString();
        }
    }
}
