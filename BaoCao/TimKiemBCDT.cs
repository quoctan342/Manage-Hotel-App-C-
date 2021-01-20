using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachSan.BaoCao
{
    public partial class TimKiemBCDT : Form
    {
        public FBC bc;
        QLKHACHSANEntities db = new QLKHACHSANEntities();
        public TimKiemBCDT(FBC sender)
        {
            InitializeComponent();
            this.bc = sender;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string thang = txtNhapThang.Text;
            string nam = txtnam.Text;
            var result = from c in db.BaoCaoDoanhThus where (c.Thang == thang && c.Nam == nam) select c;
            foreach (var i in result)
            {
                this.bc(i.MaBaoCaoDT);
                this.Close();
            }
          
        }

        private void txtNhapThang_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void TimKiemBCDT_Load(object sender, EventArgs e)
        {

        }
    }
}
