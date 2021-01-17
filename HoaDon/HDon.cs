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

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThemPTP_Click(object sender, EventArgs e)
        {
            string maptp = txtNhapPTP.Text;
            string songaythue = txtSoNgayThue.Text;
            if (maptp == "")
            {
                MessageBox.Show("Bạn chưa nhập phiếu thuê phòng");
                return;
            }
            if (songaythue == "")
            {
                MessageBox.Show("Bạn chưa nhập số ngày thuê");
                return;
            }
            PhieuThuePhong ptp = db.PhieuThuePhongs.Find(maptp);
            if (ptp == null)
            {
                MessageBox.Show("Mã phiếu thuê phòng không tồn tại.");
                return;
            }
            var checkptp = from c in db.CT_HoaDon where c.MaPhieuThuePhong == maptp select c;
            if (checkptp.Count() != 0)
            {
                MessageBox.Show("Mã phiếu thuê phòng đã được thanh toán rồi");
                return;
            }

        

            Phong ph = db.Phongs.Find(ptp.MaPhong);
            LoaiPhong lp = db.LoaiPhongs.Find(ph.MaLoaiPhong);


            dgvHoaDon.ColumnCount = 5;

            dgvHoaDon.Columns[0].Name = "Mã phiếu thuê phòng";
            dgvHoaDon.Columns[1].Name = "Mã phòng";
            dgvHoaDon.Columns[2].Name = "Số ngày thuê";
            dgvHoaDon.Columns[3].Name = "Đơn giá";
            dgvHoaDon.Columns[4].Name = "Thành tiền";

            double thanhtien = lp.DonGia * int.Parse(txtSoNgayThue.Text);

            string[] row1;
            
            row1 = new string[] {
                                    ptp.MaPhieuThuePhong,
                                    ptp.MaPhong,
                                    txtSoNgayThue.Text,
                                    lp.DonGia.ToString(),
                                    thanhtien.ToString()
            };
            for (int i = 0; i < (dgvHoaDon.Rows.Count - 1); i++)
            {
                string checkmaptp = dgvHoaDon[0, i].Value.ToString();

                if (checkmaptp == ptp.MaPhieuThuePhong)
                {
                    MessageBox.Show("Mã phiếu thuê phòng này đã được thêm bên dưới");
                    return;
                }
            }
            dgvHoaDon.Rows.Add(row1);

            txtNhapPTP.Text = string.Empty;
            txtSoNgayThue.Text = string.Empty;

        }

        private void txtSoNgayThue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        void TinhTongTienHoaDon()
        {
            double tong = 0;
            for (int i = 0; i < (dgvHoaDon.Rows.Count - 1); i++)
            {
                tong = tong + double.Parse(dgvHoaDon[4, i].Value.ToString());
            }

            txtTriGia.Text = tong.ToString();
        }

        void ResetForm()
        {
            txtMaHD.Text = string.Empty;
            txtKhach.Text = string.Empty;
            txtDiaChi.Text = string.Empty;
            DateTime d = DateTime.Now;
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
            if (txtKhach.Text == "")
            {
                MessageBox.Show("Tên khách hàng/ cơ quan thanh toán không được bỏ trống");
                return;
            }
            if (txtDiaChi.Text == "")
            {
                MessageBox.Show("Địa chỉ của khách hàng/ cơ quan thanh toán không được bỏ trống");
                return;
            }
            if (dgvHoaDon.Rows.Count == 0)
            {
                MessageBox.Show("Hóa đơn không được bỏ trống phần thanh toán ");
                return;
            }
            var result = from c in db.HoaDonTTs select c;
            int id = 0;
            foreach (var i in result)
            {
                id = id + 1;
            }

            HoaDonTT hdtt = new HoaDonTT()
            {
                MaHoaDon = "HD" + (id + 1),
                NguoiThanhToan = txtKhach.Text,
                DiaChiNguoiThanhToan = txtDiaChi.Text,
                NgayThanhToan = DateTime.Parse(txtNgayTT.Text),
                TriGia = double.Parse(txtTriGia.Text),
                Xoa = 0,
                Thang = String.Format("{0:M/yyyy}", DateTime.Parse(txtNgayTT.Text))
            };
            db.HoaDonTTs.Add(hdtt);
            db.SaveChanges();
            if (db.SaveChanges() == 0) { 
                for (int i = 0; i < (dgvHoaDon.Rows.Count - 1); i++)
                {
                    var result2 = from c in db.CT_HoaDon select c;
                    int id_cthd = 0;
                    foreach (var j in result2)
                    {
                        id_cthd = id_cthd + 1;
                    }

                    string maphieuthuephong = dgvHoaDon[0, i].Value.ToString();
                    double thanhtien = double.Parse(dgvHoaDon[3, i].Value.ToString());
                    double trigia = double.Parse(dgvHoaDon[4, i].Value.ToString());
                    int songaythue = int.Parse(dgvHoaDon[2,i].Value.ToString());
                    CT_HoaDon cthd = new CT_HoaDon()
                    {
                        MaCTHoaDon = "CTHD" + (id_cthd + 1),
                        MaHoaDon = "HD" + (id + 1),
                        MaPhieuThuePhong = maphieuthuephong,
                        SoNgayThue = songaythue,
                        ThanhTien = thanhtien,
                        TriGia = trigia
                    };


                    db.CT_HoaDon.Add(cthd);
                    db.SaveChanges();
                }
                MessageBox.Show("Thêm hóa đơn thành công. Hóa đơn có mã: HD" + (id + 1));
                ResetForm();
            }

        }

        private void txtMaHD_TextChanged(object sender, EventArgs e)
        {
            if (txtMaHD.Text == "")
            {
                return;
            }
            string mahd = txtMaHD.Text;
            HoaDonTT hd = db.HoaDonTTs.Find(mahd);

            txtKhach.Text = hd.NguoiThanhToan;
            txtDiaChi.Text = hd.DiaChiNguoiThanhToan;
            txtNgayTT.Text = String.Format("{0:d/M/yyyy}", hd.NgayThanhToan);
            var result = from c in db.CT_HoaDon
                         from a in db.PhieuThuePhongs
                         where (c.MaHoaDon == mahd && c.MaPhieuThuePhong == a.MaPhieuThuePhong)
                         select new { c.MaPhieuThuePhong, a.MaPhong, c.SoNgayThue, c.ThanhTien, c.TriGia };
            dgvHoaDon.ColumnCount = 5;

            dgvHoaDon.Columns[0].Name = "Mã phiếu thuê phòng";
            dgvHoaDon.Columns[1].Name = "Mã phòng";
            dgvHoaDon.Columns[2].Name = "Số ngày thuê";
            dgvHoaDon.Columns[3].Name = "Đơn giá";
            dgvHoaDon.Columns[4].Name = "Thành tiền";
            string[] row1;

            foreach (var i in result) {
                row1 = new string[] {
                                        i.MaPhieuThuePhong,
                                        i.MaPhong,
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
            hd.NguoiThanhToan = txtKhach.Text;
            hd.DiaChiNguoiThanhToan = txtDiaChi.Text;

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
    }
}
