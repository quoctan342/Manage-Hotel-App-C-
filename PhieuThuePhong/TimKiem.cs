using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSan.PTP
{
    public partial class TimKiem : Form
    {
        QLKHACHSANEntities db = new QLKHACHSANEntities();
        public FDMP fdm;

        public TimKiem(FDMP sender)
        {
            InitializeComponent();
            ShowCBB();
            string madmp = cbbDMP.SelectedValue.ToString();
            var result = from c in db.Phongs
                         where (c.MaDanhMucPhong == madmp && c.TinhTrang == "Đang thuê")
                         select c;
            cbbP.DataSource = result.ToList();
            cbbP.DisplayMember = "TenPhong";
            cbbP.ValueMember = "MaPhong";
            this.fdm = sender;
        }
        void ShowCBB()
        {
            var result = from c in db.DanhMucPhongs select c;
            cbbDMP.DataSource = result.ToList();
            cbbDMP.DisplayMember = "TenDMP";
            cbbDMP.ValueMember = "MaDMPhong";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string maphong = cbbP.SelectedValue.ToString();
            var result = from c in db.PhieuThuePhongs where c.MaPhong == maphong select c;
            foreach (var i in result)
            {
                this.fdm(i.MaPhieuThuePhong);
                this.Close();
            }
        }

        private void cbbDMP_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbbP.DataSource = null;
            string madmp = cbbDMP.SelectedValue.ToString();
            var result = from c in db.Phongs
                         where (c.MaDanhMucPhong == madmp && c.TinhTrang == "Đang thuê")
                         select c;
            cbbP.DataSource = result.ToList();
            cbbP.DisplayMember = "TenPhong";
            cbbP.ValueMember = "MaPhong";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
