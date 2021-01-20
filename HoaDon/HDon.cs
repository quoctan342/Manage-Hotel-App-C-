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
    public delegate void FHD(String value);

    public partial class HDon : Form
    {
        QLKHACHSANEntities db = new QLKHACHSANEntities();
        public HDon()
        {
            InitializeComponent();
            DateTime date = DateTime.Now;
            txtNgayTT.Text = String.Format("{0:d/M/yyyy}", date);
            dgvHoaDon.ColumnCount = 6;
            dgvHoaDon.Columns[0].Name = "Mã phiếu thuê phòng";
            dgvHoaDon.Columns[1].Name = "Mã phòng";
            dgvHoaDon.Columns[2].Name = "Phòng";
            dgvHoaDon.Columns[3].Name = "Số ngày thuê";
            dgvHoaDon.Columns[4].Name = "Đơn giá";
            dgvHoaDon.Columns[5].Name = "Thành tiền";
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThemPTP_Click(object sender, EventArgs e)
        {
            KhachHang kh = db.KhachHangs.Find(txtMKH.Text);
            txtKhach.Text = kh.TenKhachHang;
            txtDiaChi.Text = kh.DiaChi;
            txtMaKH.Text = kh.MaKhachHang;
            var result = from a in db.PhieuThuePhongs
                         from c in db.CT_PhieuThuePhong
                         from b in db.Phongs
                         from d in db.LoaiPhongs
                         where (c.MaKhachHang == kh.MaKhachHang && c.MaPhieuThuePhong == a.MaPhieuThuePhong
                         && a.MaPhong == b.MaPhong && b.MaLoaiPhong == d.MaLoaiPhong)
                         select new { a.MaPhieuThuePhong, b.MaPhong, b.TenPhong, a.NgayBatDauThue, d.DonGia};
            foreach (var i in result) {
                var tee = from c in db.CT_HoaDon where c.MaPhieuThuePhong == i.MaPhieuThuePhong select c;
                if (tee.Count() != 0)
                {
                    continue;
                }
                double songaythue = (DateTime.Now - i.NgayBatDauThue).TotalDays;
                int songay = Convert.ToInt32(songaythue);
                double dongia = i.DonGia;
                double thanhtien = dongia * songay;
                string[] row1 = new string[] {
                    i.MaPhieuThuePhong,
                    i.MaPhong,
                    i.TenPhong,
                    songay.ToString(),
                    i.DonGia.ToString(),
                    thanhtien.ToString()
                };

                dgvHoaDon.Rows.Add(row1);
            }
        }

        void TinhTongTienHoaDon()
        {
            double tong = 0;
            for (int i = 0; i < (dgvHoaDon.Rows.Count); i++)
            {
                tong = tong + double.Parse(dgvHoaDon[5, i].Value.ToString());
            }

            txtTriGia.Text = tong.ToString();
        }

        void ResetForm()
        {
            txtMaHD.Text = string.Empty;
            txtKhach.Text = string.Empty;
            txtDiaChi.Text = string.Empty;
            DateTime d = DateTime.Now;
            txtMaKH.Text = "";
            txtNgayTT.Text = String.Format("{0:d/M/yyyy}", d);
            dgvHoaDon.DataSource = null;
            dgvHoaDon.Rows.Clear();
            dgvHoaDon.Refresh();
        }

        private void dgvHoaDon_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            TinhTongTienHoaDon();
        }

        private void dgvHoaDon_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            TinhTongTienHoaDon();
        }

        private void dgvHoaDon_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TinhTongTienHoaDon();
        }

        private void btnLapHD_Click(object sender, EventArgs e)
        {
            var result = from c in db.HoaDonTTs select c;
            HoaDonTT hd = new HoaDonTT()
            {
                MaHoaDon = "HD" + (result.Count() + 1),
                MaKhachhang = txtMaKH.Text,
                NgayThanhToan = Convert.ToDateTime(txtNgayTT.Text),
                TriGia = float.Parse(txtTriGia.Text),
                Xoa = 0
            };
            db.HoaDonTTs.Add(hd);
            string mahoadon = "HD" + (result.Count() + 1);
            for (int i = 0; i < dgvHoaDon.Rows.Count; i++)
            {
                var re = from c in db.CT_HoaDon select c;
                CT_HoaDon ct = new CT_HoaDon()
                {
                    MaCTHoaDon = "CTHD" + (re.Count() + 1),
                    MaHoaDon = mahoadon,
                    MaPhieuThuePhong = dgvHoaDon[0, i].Value.ToString(),
                    SoNgayThue = int.Parse(dgvHoaDon[3, i].Value.ToString()),
                    ThanhTien = float.Parse(dgvHoaDon[4, i].Value.ToString()),
                    TriGia = float.Parse(dgvHoaDon[5, i].Value.ToString())
                };
                db.CT_HoaDon.Add(ct);
                db.SaveChanges();

            }

            db.SaveChanges();
            MessageBox.Show("Lập hóa đơn thành công");
        }

        private void txtMaHD_TextChanged(object sender, EventArgs e)
        {
            if (txtMaHD.Text == "")
            {
                return;
            }
            string mahd = txtMaHD.Text;
            HoaDonTT hd = db.HoaDonTTs.Find(mahd);
            txtMaKH.Text = hd.MaKhachhang;
            KhachHang kh = db.KhachHangs.Find(hd.MaKhachhang);
            txtKhach.Text = kh.TenKhachHang;
            txtDiaChi.Text = kh.DiaChi;
            txtNgayTT.Text = String.Format("{0:d/M/yyyy}", hd.NgayThanhToan);
            var result = from c in db.CT_HoaDon
                         from a in db.PhieuThuePhongs
                         from b in db.Phongs
                         where (c.MaHoaDon == mahd && c.MaPhieuThuePhong == a.MaPhieuThuePhong && a.MaPhong == b.MaPhong)
                         select new { c.MaPhieuThuePhong, a.MaPhong, c.SoNgayThue, c.ThanhTien, c.TriGia, b.TenPhong };
            string[] row1;

            foreach (var i in result) {
                row1 = new string[] {
                                        i.MaPhieuThuePhong,
                                        i.MaPhong,
                                        i.TenPhong,
                                        i.SoNgayThue.ToString(),
                                        i.ThanhTien.ToString(),
                                        i.TriGia.ToString()
                };
                dgvHoaDon.Rows.Add(row1);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            TimKiem fTK = new TimKiem(SetValue);
            fTK.ShowDialog();
        }

        private void SetValue(String value)
        {
            this.txtMaHD.Text = value;
        }

        private void btnHDMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnXoaHD_Click(object sender, EventArgs e)
        {
            string mahd = txtMaHD.Text;
            HoaDonTT hd = db.HoaDonTTs.Find(mahd);
            if (hd == null)
            {
                MessageBox.Show("Có lỗi xảy ra.");
                return;
            }

            hd.Xoa = 1;
            db.SaveChanges();
            if (db.SaveChanges() == 0)
            {
                MessageBox.Show("Xóa thành công hóa đơn này");
                ResetForm();

            }

        }

        private void btnUpdateHD_Click(object sender, EventArgs e)
        {
            string mahd = txtMaHD.Text;
            HoaDonTT hd = db.HoaDonTTs.Find(mahd);

            for (int i = 0; i < (dgvHoaDon.Rows.Count - 1); i++)
            {
                string maptp = dgvHoaDon[0, i].Value.ToString();
                CT_HoaDon ctd = db.CT_HoaDon.Find(maptp);
                int songaythue = int.Parse(dgvHoaDon[2, i].Value.ToString());
                double thanhtien = double.Parse(dgvHoaDon[4, i].Value.ToString());
                double dongia = double.Parse(dgvHoaDon[3, i].Value.ToString());
                if (ctd != null)
                {                   
                    ctd.SoNgayThue = songaythue;
                    ctd.TriGia = thanhtien;
                } else
                {
                    
                    var result = from c in db.CT_HoaDon select c;
                    int id = 0;
                    foreach (var a in result)
                    {
                        id = id + 1;
                    }
                    CT_HoaDon cthd = new CT_HoaDon()
                    {
                        MaCTHoaDon = "CTHD" + (id + 1),
                        MaHoaDon = mahd,
                        MaPhieuThuePhong = maptp,
                        SoNgayThue = songaythue,
                        ThanhTien = dongia,
                        TriGia = thanhtien
                    };
                    db.CT_HoaDon.Add(cthd);
                }
            }

            var re = from c in db.CT_HoaDon where c.MaHoaDon == mahd select c;
            foreach (var b in re)
            {
                int check = 0;
                for (int i = 0; i < dgvHoaDon.Rows.Count - 1; i++)
                {
                    if (dgvHoaDon[0,i].Value.ToString() == b.MaPhieuThuePhong)
                    {
                        check = check + 1;
                    }
                }
                if (check == 0)
                {
                    db.CT_HoaDon.Remove(b);
                }
            }

            double trigia = double.Parse(txtTriGia.Text);
            hd.TriGia = trigia;

            db.SaveChanges();
            
            if (db.SaveChanges() == 0)
            {
                MessageBox.Show("Cập nhật hóa đơn thành công");
            }
        }

        private void dgvHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void HDon_Load(object sender, EventArgs e)
        {

        }
    }
}
