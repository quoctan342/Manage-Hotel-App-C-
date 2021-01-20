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
    public delegate void FDMP(String value);

    public partial class PTP : Form
    {
        QLKHACHSANEntities db = new QLKHACHSANEntities();
        public PTP()
        {
            InitializeComponent();
            ShowCBB();
            dgvKH.ColumnCount = 4;

            dgvKH.Columns[0].Name = "Khách hàng";
            dgvKH.Columns[1].Name = "Loại khách";
            dgvKH.Columns[2].Name = "CMND";
            dgvKH.Columns[3].Name = "Địa chỉ";

            string madmp = cbbDMP.SelectedValue.ToString();
            var phong = from c in db.Phongs where (c.MaDanhMucPhong == madmp && c.TinhTrang == "Trống") select c;
            cbbP.DataSource = phong.ToList();
            cbbP.DisplayMember = "TenPhong";
            cbbP.ValueMember = "MaPhong";
            DateTime date = DateTime.Now;
            txtNgayThue.Text = String.Format("{0:d/M/yyyy}", date);
        }

        void ShowCBB()
        {
            var dmp = from c in db.DanhMucPhongs select c;
            cbbDMP.DataSource = dmp.ToList();
            cbbDMP.DisplayMember = "TenDMP";
            cbbDMP.ValueMember = "MaDMPhong";
            var loaikhach = from c in db.LoaiKhachHangs select c;
            cbLoaiKhach.DataSource = loaikhach.ToList();
            cbLoaiKhach.DisplayMember = "TenLoaiKhachHang";
            cbLoaiKhach.ValueMember = "MaLoaiKhachHang";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PTP_Load(object sender, EventArgs e)
        {

        }

        private void cbbDMP_SelectedIndexChanged(object sender, EventArgs e)
        {
            string madmp = cbbDMP.SelectedValue.ToString();
            var phong = from c in db.Phongs where (c.MaDanhMucPhong == madmp && c.TinhTrang == "Trống") select c;
            cbbP.DataSource = phong.ToList();
            cbbP.DisplayMember = "TenPhong";
            cbbP.ValueMember = "MaPhong";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvKH.Rows.Count; i++)
            {
                if (dgvKH[2,i].Value.ToString() == txtCMND.Text)
                {
                    MessageBox.Show("Khách hàng với CMND: " + txtCMND.Text + " đã được thêm bên dưới. Vui lòng thêm khách hàng khác");
                    return;
                }
            }
            if(dgvKH.Rows.Count == 3)
            {
                MessageBox.Show("Mỗi phòng chỉ được tối đa 3 khách hàng.");
                return;
            }
            string[] row1 = new string[]
            {
                txtKH.Text,
                cbLoaiKhach.Text,
                txtCMND.Text,
                txtDiaChi.Text
            };
            dgvKH.Rows.Add(row1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime ngbd = Convert.ToDateTime(txtNgayThue.Text);
            var count_ptp = from c in db.PhieuThuePhongs select c;
            PhieuThuePhong ptp = new PhieuThuePhong()
            {
                MaPhieuThuePhong = "PTP" + (count_ptp.Count() + 1),
                MaPhong = cbbP.SelectedValue.ToString(),
                NgayBatDauThue = ngbd.Date,
                Xoa = 0
            };
            db.PhieuThuePhongs.Add(ptp);
            Phong pg = db.Phongs.Find(cbbP.SelectedValue.ToString());
            pg.TinhTrang = "Đang thuê";
            string maptp = "PTP" + (count_ptp.Count() + 1);
            for (int i = 0; i < dgvKH.Rows.Count; i++)
            {
                string cmnd = dgvKH[2, i].Value.ToString();
                string loaikh = dgvKH[1, i].Value.ToString();
                var count_kh = from c in db.KhachHangs select c;

                var check_cmnd = from c in db.KhachHangs where c.CMND == cmnd select c;
                if (check_cmnd.Count() == 0)
                {
                    var malkh = from c in db.LoaiKhachHangs where c.TenLoaiKhachHang == loaikh select c;
                    KhachHang kh = new KhachHang();
                    kh.MaKhachHang = "KH" + (count_kh.Count() + 1);
                    kh.TenKhachHang = dgvKH[0, i].Value.ToString();
                    kh.CMND = cmnd;
                    kh.DiaChi = dgvKH[3, i].Value.ToString();
                    foreach (var b in malkh)
                    {
                        kh.MaLoaiKhachHang = b.MaLoaiKhachHang;
                    }
                    db.KhachHangs.Add(kh);
                    var count_ct = from c in db.CT_PhieuThuePhong select c;
                    CT_PhieuThuePhong ct = new CT_PhieuThuePhong()
                    {
                        
                        MaCTPhieuThuePhong = "CTPTP" + (count_ct.Count() + 1),
                        MaPhieuThuePhong = maptp,
                        MaKhachHang = "KH" + (count_kh.Count() + 1)
                    };
                    db.CT_PhieuThuePhong.Add(ct);
                }
                else
                {
                    var kh = from c in db.KhachHangs where c.CMND == cmnd select c;
                    foreach (var a in kh)
                    {
                        var count_ct = from c in db.CT_PhieuThuePhong select c;
                        CT_PhieuThuePhong ct = new CT_PhieuThuePhong()
                        {
                            MaCTPhieuThuePhong = "CTPTP" + (count_ct.Count() + 1),
                            MaPhieuThuePhong = maptp,
                            MaKhachHang = a.MaKhachHang
                        };

                        db.CT_PhieuThuePhong.Add(ct);
                    }

                }
                db.SaveChanges();

            }
            MessageBox.Show("Tạo phiếu thuê phòng thành công");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            TimKiem tk = new TimKiem(SetValue);
            tk.ShowDialog();
        }
        private void SetValue(String value)
        {
            this.txtPTP.Text = value;
        }

        private void txtNgayThue_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtPTP_TextChanged(object sender, EventArgs e)
        {
            if (txtPTP.Text == "")
            {
                return;
            }
            dgvKH.Rows.Clear();
            PhieuThuePhong ptp = db.PhieuThuePhongs.Find(txtPTP.Text);
            Phong ph = db.Phongs.Find(ptp.MaPhong);
            DanhMucPhong dmp = db.DanhMucPhongs.Find(ph.MaDanhMucPhong);

            txtDMP.Text = dmp.TenDMP;
            txtPhong.Text = ph.TenPhong;
            txtNgayThue.Text = string.Format("{0:d/M/yyyy}", ptp.NgayBatDauThue);
            var result = from c in db.CT_PhieuThuePhong
                         from d in db.KhachHangs
                         from a in db.LoaiKhachHangs
                         where (c.MaPhieuThuePhong == ptp.MaPhieuThuePhong
                         && c.MaKhachHang == d.MaKhachHang
                         && d.MaLoaiKhachHang == a.MaLoaiKhachHang) 
                         select new
                         {
                             d.TenKhachHang,
                             a.TenLoaiKhachHang,
                             d.CMND,
                             d.DiaChi
                         };
            foreach (var i in result)
            {
                string[] row1 = new string[]
                    {
                        i.TenKhachHang,
                        i.TenLoaiKhachHang,
                        i.CMND,
                        i.DiaChi
                    };
                dgvKH.Rows.Add(row1);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtPTP.Text = "";
            DateTime date = DateTime.Now;
            txtNgayThue.Text = String.Format("{0:d/M/yyyy}", date);
            txtDMP.Text = "";
            txtPhong.Text = "";
            dgvKH.Rows.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            PhieuThuePhong ptp = db.PhieuThuePhongs.Find(txtPTP.Text);
            ptp.Xoa = 1;
            db.SaveChanges();
            MessageBox.Show("Đã xóa thành công phiếu thuê phòng này.");
            txtPTP.Text = "";
            DateTime date = DateTime.Now;
            txtNgayThue.Text = String.Format("{0:d/M/yyyy}", date);
            txtDMP.Text = "";
            txtPhong.Text = "";
            dgvKH.Rows.Clear();
        }
    }
}
