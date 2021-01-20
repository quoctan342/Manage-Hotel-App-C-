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
    public partial class TimKiemBCMD : Form
    {
        public FMD bc;
        QLKHACHSANEntities db = new QLKHACHSANEntities();
        public TimKiemBCMD(FMD sender)
        {
            InitializeComponent();
            this.bc = sender;
        }

        private void TimKiemBCMD_Load(object sender, EventArgs e)
        {

        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string thang = txtNhapThang.Text;
            string nam = txtnam.Text;
            var result = from c in db.BaoCaoMDSDs where (c.Thang == thang && c.Nam == nam) select c;
            foreach (var i in result)
            {
                this.bc(i.MaBaoCaoMDSD);
                this.Close();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
