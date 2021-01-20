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
    public delegate void FMD(String value);

    public partial class MatDo : Form
    {
        QLKHACHSANEntities db = new QLKHACHSANEntities();
        public MatDo()
        {
            InitializeComponent();
            dgvCTBC.ColumnCount = 3;

            dgvCTBC.Columns[0].Name = "Phòng";
            dgvCTBC.Columns[1].Name = "Số ngày thuê";
            dgvCTBC.Columns[2].Name = "Tỷ lệ";
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            if (txtThang.Text != "" || txtMaBC.Text != "")
            {
                MessageBox.Show("Lỗi xảy ra");
                return;
            }

            dgvCTBC.ColumnCount = 3;

            dgvCTBC.Columns[0].Name = "Phòng";
            dgvCTBC.Columns[1].Name = "Số ngày thuê";
            dgvCTBC.Columns[2].Name = "Tỷ lệ";

            string thang = txtNhapThang.Text;
            string[] row1;
            var tongsongaythue = from c in db.HoaDonTTs
                                 from a in db.CT_HoaDon
                         where (c.MaHoaDon == a.MaHoaDon)
                         select new { a.SoNgayThue };
            int tong = 0;
            foreach (var v in tongsongaythue)
            {
                tong = tong + v.SoNgayThue;
            }
            var result = from c in db.PhieuThuePhongs
                        from d in db.CT_HoaDon
                        from f in db.HoaDonTTs
                        where (c.MaPhieuThuePhong == d.MaPhieuThuePhong
                        && d.MaHoaDon == f.MaHoaDon && f.Xoa == 0)
                        select new { c.MaPhong };
            foreach (var i in result)
            {
                var re = from a in db.CT_HoaDon
                         from b in db.HoaDonTTs
                         from c in db.PhieuThuePhongs
                         where (a.MaHoaDon == b.MaHoaDon && b.Xoa == 0
                         && a.MaPhieuThuePhong == c.MaPhieuThuePhong && c.MaPhong == i.MaPhong)
                         select new { a.SoNgayThue };
                int songaythue = 0;
                foreach (var t in re)
                {
                    songaythue = songaythue + t.SoNgayThue;
                }
                int tyle = (int)(0.5f + ((100f * songaythue) / tong));
                row1 = new string[]
               {
                    i.MaPhong,
                    songaythue.ToString(),
                    tyle+"%"
               };
                dgvCTBC.Rows.Add(row1);
            }
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLap_Click(object sender, EventArgs e)
        {
            var result = from c in db.BaoCaoMDSDs select c;

            BaoCaoMDSD bc = new BaoCaoMDSD()
            {
                MaBaoCaoMDSD = "BCMD" + (result.Count() + 1),
                Thang = txtNhapThang.Text,
                Xoa = 0,
                Nam = txtNam.Text
            };
            db.BaoCaoMDSDs.Add(bc);
            db.SaveChanges();
            string mabc = "BCMD" + (result.Count() + 1);

            if (db.SaveChanges() == 0)
            {
                for (int i = 0; i < dgvCTBC.Rows.Count; i++)
                {
                    var res = from c in db.CT_BaoCaoMDSD select c;
                    CT_BaoCaoMDSD ct = new CT_BaoCaoMDSD()
                    {
                        MaCTBaoCaoMDSD = "CTMD" + (res.Count() + 1),
                        MaBaoCaoMDSD = mabc,
                        MaPhong = dgvCTBC[0, i].Value.ToString(),
                        SoNgayThue = int.Parse(dgvCTBC[1, i].Value.ToString()),
                        TyLe = dgvCTBC[2, i].Value.ToString()
                    };
                    db.CT_BaoCaoMDSD.Add(ct);
                    db.SaveChanges();
                }
            }
            MessageBox.Show("Lập báo cáo thành công");

        }

        private void btnThongKe_Click_1(object sender, EventArgs e)
        {
            dgvCTBC.Rows.Clear();
            if (txtThang.Text != "" || txtMaBC.Text != "")
            {
                MessageBox.Show("Lỗi xảy ra");
                return;
            }
            string thang = txtNhapThang.Text;
            if (thang == "1") { thang = "01"; }
            if (thang == "2") { thang = "02"; }
            if (thang == "3") { thang = "03"; }
            if (thang == "4") { thang = "04"; }
            if (thang == "5") { thang = "05"; }
            if (thang == "6") { thang = "06"; }
            if (thang == "7") { thang = "07"; }
            if (thang == "8") { thang = "08"; }
            if (thang == "9") { thang = "09"; }
            string nam = txtNam.Text;
         
            var result = (from c in db.CT_HoaDon
                         from d in db.HoaDonTTs
                         from a in db.PhieuThuePhongs
                         from b in db.Phongs
                         where (c.MaHoaDon == d.MaHoaDon && c.MaPhieuThuePhong == a.MaPhieuThuePhong && a.MaPhong == b.MaPhong)
                         select new
                         {
                           a.MaPhong, b.TenPhong
                         }).Distinct();
            foreach (var i in result)
            {
                double tong = 0;
                double tongmd = 0;
                var res = from c in db.CT_HoaDon
                          from d in db.HoaDonTTs
                          where (c.MaHoaDon == d.MaHoaDon)
                          select new
                          {
                              d.NgayThanhToan, c.SoNgayThue
                          };
                foreach (var g in res)
                {
                    string th = String.Format("{0:MM}", g.NgayThanhToan);
                    string ye = String.Format("{0:yyyy}", g.NgayThanhToan);
                    if (th == thang && ye == nam)
                    {
                        tongmd = tongmd + g.SoNgayThue;
                    }
                }

                var kq = from c in db.CT_HoaDon
                             from d in db.HoaDonTTs
                             from a in db.PhieuThuePhongs
                             from b in db.Phongs
                             where (c.MaHoaDon == d.MaHoaDon && c.MaPhieuThuePhong == a.MaPhieuThuePhong
                             && a.MaPhong == b.MaPhong)
                             select new
                             {
                                 d.NgayThanhToan,
                                 a.MaPhong,
                                 c.SoNgayThue
                             };
                foreach (var a in kq)
                {
                    string t = String.Format("{0:MM}", a.NgayThanhToan);
                    string y = String.Format("{0:yyyy}", a.NgayThanhToan);


                    if (t == thang && y == nam && i.MaPhong == a.MaPhong)
                    {
                        tong = tong + a.SoNgayThue;
                    }
                }
                int tyle = (int)(0.5f + ((100f * tong) / tongmd));


                string[] row1 = new string[]
                {
                    i.MaPhong,
                    tong.ToString(),
                    tyle.ToString() + "%"
                };
                dgvCTBC.Rows.Add(row1);
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            TimKiemBCMD tk = new TimKiemBCMD(SetValue);
            tk.ShowDialog();
        }
        private void SetValue(String value)
        {
            this.txtMaBC.Text = value;
        }

        private void txtMaBC_TextChanged(object sender, EventArgs e)
        {
            if (txtMaBC.Text =="")
            {
                return;
            }
            BaoCaoMDSD bc = db.BaoCaoMDSDs.Find(txtMaBC.Text);
            txtThang.Text = bc.Thang + "/" + bc.Nam;
            var result = from c in db.CT_BaoCaoMDSD where c.MaBaoCaoMDSD == bc.MaBaoCaoMDSD select c;
            foreach (var i in result)
            {
                string[] row1 = new string[]
                {
                    i.MaPhong,
                    i.SoNgayThue.ToString(),
                    i.TyLe
                };
                dgvCTBC.Rows.Add(row1);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtMaBC.Text = "";
            txtThang.Text = "";
            dgvCTBC.Rows.Clear();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
             if (txtMaBC.Text =="")
            {
                return;
            }
            BaoCaoMDSD bc = db.BaoCaoMDSDs.Find(txtMaBC.Text);
            bc.Xoa = 1;
            db.SaveChanges();
            MessageBox.Show("Xóa thành công");
        }
    }
}
