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
    public delegate void FBC(String value);

    public partial class DoanhThu : Form
    {
        QLKHACHSANEntities db = new QLKHACHSANEntities();
        public DoanhThu()
        {
            InitializeComponent();
            dgvCTBC.ColumnCount = 3;

            dgvCTBC.Columns[0].Name = "Loại phòng";
            dgvCTBC.Columns[1].Name = "Doanh thu";
            dgvCTBC.Columns[2].Name = "Tỷ lệ";
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            TimKiemBCDT tk = new TimKiemBCDT(SetValue);
            tk.ShowDialog();
        }

        private void SetValue(String value)
        {
            this.txtMaBC.Text = value;
        }
        void ResetForm()
        {
            txtMaBC.Text = "";
            txtThang.Text = "";
            dgvCTBC.DataSource = null;
            dgvCTBC.Rows.Clear();
            dgvCTBC.Refresh();
        }
        private void btnThongKe_Click(object sender, EventArgs e)
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
            var list_lp = (
                          from c in db.CT_HoaDon
                          from b in db.PhieuThuePhongs
                          from d in db.Phongs
                          from a in db.LoaiPhongs
                          where (
                          c.MaPhieuThuePhong == b.MaPhieuThuePhong
                          && b.MaPhong == d.MaPhong
                          && d.MaLoaiPhong == a.MaLoaiPhong)
                          select new { d.MaLoaiPhong }).Distinct();
            
            foreach (var i in list_lp)
            {
                double tong = 0;
                double tongdt = 0;
                var res = from c in db.HoaDonTTs select c;
                foreach (var g in res)
                {
                    string th = String.Format("{0:MM}", g.NgayThanhToan);
                    string ye = String.Format("{0:yyyy}", g.NgayThanhToan);
                    if (th == thang && ye == nam)
                    {
                        tongdt = tongdt + g.TriGia;
                    }
                }
                var result = from c in db.CT_HoaDon
                             from d in db.HoaDonTTs
                             from a in db.PhieuThuePhongs
                             from b in db.Phongs
                             from g in db.LoaiPhongs
                             where (c.MaHoaDon == d.MaHoaDon && c.MaPhieuThuePhong == a.MaPhieuThuePhong
                             && a.MaPhong == b.MaPhong && b.MaLoaiPhong == g.MaLoaiPhong)
                             select new
                             {
                                 d.NgayThanhToan, g.MaLoaiPhong, c.TriGia
                             };
                foreach (var a in result)
                {
                    string t = String.Format("{0:MM}", a.NgayThanhToan);
                    string y = String.Format("{0:yyyy}", a.NgayThanhToan);

                    
                    if (t == thang && y == nam && i.MaLoaiPhong == a.MaLoaiPhong)
                    {
                        tong = tong + a.TriGia;
                    }
                }
                int tyle = (int)(0.5f + ((100f * tong) / tongdt));


                string[] row1 = new string[]
                {
                    i.MaLoaiPhong,
                    tong.ToString(),
                    tyle.ToString() + "%"
                };
                dgvCTBC.Rows.Add(row1);
            }
          
            
        }

        private void btnLap_Click(object sender, EventArgs e)
        {
            if (txtThang.Text != "" || txtMaBC.Text != "")
            {
                MessageBox.Show("Lỗi xảy ra");
                return;
            }
            string mabc = txtNhapThang.Text;
            var check = from c in db.BaoCaoDoanhThus where c.Thang == mabc select c;
            if (check.Count() != 0)
            {
                MessageBox.Show("Báo cáo tháng " + mabc + "đã tồn tại.");
                return;
            };



            var result = from c in db.BaoCaoDoanhThus select c;
            int id = 0;
            foreach (var i in result)
            {
                id = id + 1;
            }

            BaoCaoDoanhThu bc = new BaoCaoDoanhThu()
            {
                MaBaoCaoDT = "BCDT" + (id + 1),
                Thang = txtNhapThang.Text,
                Nam = txtNam.Text,
                Xoa = 0
            };
            db.BaoCaoDoanhThus.Add(bc);
            db.SaveChanges();

            if (db.SaveChanges() == 0)
            {
                for (int i = 0; i < (dgvCTBC.Rows.Count - 1); i++)
                {
                    var re = from c in db.CT_BaoCaoDT select c;
                    int id_ct = 0;
                    foreach (var d in re)
                    {
                        id_ct = id_ct + 1;
                    }
                    string loaiphong = dgvCTBC[0, i].Value.ToString();
                    double doanhthu = double.Parse(dgvCTBC[1, i].Value.ToString());
                    string tyle = dgvCTBC[2, i].Value.ToString();
                    CT_BaoCaoDT ct = new CT_BaoCaoDT()
                    {
                        MaCTBaoCaoDT = "CTDT" + (id_ct + 1),
                        MaBaoCaoDT = "BCDT" + (id + 1),
                        MaLoaiPhong = loaiphong,
                        DoanhThu = doanhthu,
                        TyLe = tyle
                    };

                    db.CT_BaoCaoDT.Add(ct);
                    db.SaveChanges();
                }
                MessageBox.Show("Lập báo cáo thành công");
            }
        }

        private void txtThang_TextChanged(object sender, EventArgs e)
        {
            dgvCTBC.ColumnCount = 3;

            dgvCTBC.Columns[0].Name = "Loại phòng";
            dgvCTBC.Columns[1].Name = "Doanh thu";
            dgvCTBC.Columns[2].Name = "Tỷ lệ";

            string[] row1;
            string thang = txtThang.Text;
            var result = from c in db.BaoCaoDoanhThus where c.Thang == thang select c;
            foreach (var i in result)
            {
                txtMaBC.Text = i.MaBaoCaoDT;
                var re = from a in db.CT_BaoCaoDT select a;
                foreach (var b in re)
                {
                row1 = new string[]
                    {
                        b.MaLoaiPhong,
                        b.DoanhThu.ToString(),
                        b.TyLe
                    };
                    dgvCTBC.Rows.Add(row1);
                }
            }
           
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            dgvCTBC.Rows.Clear();
            dgvCTBC.Refresh();
            dgvCTBC.ColumnCount = 3;

            dgvCTBC.Columns[0].Name = "Loại phòng";
            dgvCTBC.Columns[1].Name = "Doanh thu";
            dgvCTBC.Columns[2].Name = "Tỷ lệ";
            string thang = txtThang.Text;
            string[] row1;
            var result = from c in db.HoaDonTTs select c;
            double tong = 0;
            foreach (var i in result)
            {
                tong = tong + i.TriGia;
            }

            var re = from a in db.LoaiPhongs
                     from b in db.Phongs
                     from c in db.PhieuThuePhongs
                     from d in db.CT_HoaDon
                     from f in db.HoaDonTTs
                     where (a.MaLoaiPhong == b.MaLoaiPhong && b.MaPhong == c.MaPhong && c.MaPhieuThuePhong == d.MaPhieuThuePhong
                     && d.MaHoaDon == f.MaHoaDon && f.Xoa == 0)
                     select new { a.MaLoaiPhong };
            foreach (var t in re)
            {
                var rr = from a in db.CT_HoaDon
                         from b in db.HoaDonTTs
                         from c in db.PhieuThuePhongs
                         from d in db.Phongs
                         from f in db.LoaiPhongs
                         where (a.MaHoaDon == b.MaHoaDon && b.Xoa == 0
                                && a.MaPhieuThuePhong == c.MaPhieuThuePhong && c.MaPhong == d.MaPhong
                                && d.MaLoaiPhong == f.MaLoaiPhong && f.MaLoaiPhong == t.MaLoaiPhong)
                         select new { a.TriGia };
                double tongtien = 0;

                foreach (var h in rr)
                {
                    tongtien = tongtien + h.TriGia;
                }
                int tyle = (int)(0.5f + ((100f * tongtien) / tong));
                row1 = new string[]
                {
                    t.MaLoaiPhong,
                    tongtien.ToString(),
                    tyle.ToString()+"%"
                };
                dgvCTBC.Rows.Add(row1);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaBC.Text == "")
            {
                MessageBox.Show("Lỗi xảy ra");
                return;
            }
            string mabc = txtMaBC.Text;

            BaoCaoDoanhThu bc = db.BaoCaoDoanhThus.Find(mabc);
            bc.Xoa = 1;
            db.SaveChanges();
            if (db.SaveChanges() == 0)
            {
                MessageBox.Show("Xóa thành công báo cáo tháng" + txtThang.Text);
                ResetForm();
            }
        }

        private void txtMaBC_TextChanged(object sender, EventArgs e)
        {
            if (txtMaBC.Text == "")
            {
                return;
            }
            BaoCaoDoanhThu bc = db.BaoCaoDoanhThus.Find(txtMaBC.Text);
            txtThang.Text = bc.Thang + "/" + bc.Nam;
            var result = from c in db.CT_BaoCaoDT where c.MaBaoCaoDT == bc.MaBaoCaoDT select c;
            foreach (var i in result)
            {
                string[] row1 = new string[]
                          {
                              i.MaLoaiPhong,
                              i.DoanhThu.ToString(),
                              i.TyLe.ToString()
                          };
                dgvCTBC.Rows.Add(row1);

            }
          
        }
    }
}
