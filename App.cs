using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;


namespace QuanLyKhachSan
{
    public partial class App : Form
    {

        public App()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnHDTT_Click(object sender, EventArgs e)
        {
            HoaDon.HDon hd = new HoaDon.HDon();
            hd.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DMP.DMP dmp = new DMP.DMP();
            dmp.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PTP.PTP ptp = new PTP.PTP();
            ptp.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TraCuuPhong.TraCuuPhong tcp = new TraCuuPhong.TraCuuPhong();
            tcp.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            BaoCao.DoanhThu dt = new BaoCao.DoanhThu();
            dt.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BaoCao.MatDo md = new BaoCao.MatDo();
            md.Show();
        }
    }
}
