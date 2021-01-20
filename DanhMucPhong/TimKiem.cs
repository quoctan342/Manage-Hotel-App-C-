using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSan.DMP
{
    public partial class TimKiem : Form
    {
        QLKHACHSANEntities db = new QLKHACHSANEntities();
        public FDMP fdm;
        public TimKiem(FDMP sender)
        {
            InitializeComponent();
            this.fdm = sender;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tendmp = this.txtTen.Text;
            if (txtTen.Text == "")
            {
                MessageBox.Show("Bạn chưa điền tên danh mục phòng cần tìm.");
                return;
            }
            var result = from c in db.DanhMucPhongs where (c.TenDMP == tendmp && c.Xoa == 0) select c;
            if (result.Count() == 0)
            {
                MessageBox.Show("Danh mục phòng n   ày không tồn tại.");
                return;
            }
            foreach (var i in result)
            {
                this.fdm(i.MaDMPhong);
                this.Close();
            }

        }
    }
}
