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
    public delegate void FDMP(String value);

    public partial class DMP : Form
    {
        QLKHACHSANEntities db = new QLKHACHSANEntities();

        public DMP()
        {
            InitializeComponent();
            ShowCBP();
            dgvCT.ColumnCount = 4;

            dgvCT.Columns[0].Name = "Mã phòng";
            dgvCT.Columns[1].Name = "Tên phòng";
            dgvCT.Columns[2].Name = "Loại Phòng";
            dgvCT.Columns[3].Name = "Đơn giá";
        }

        void ShowCBP()
        {
            var result = from c in db.Phongs
                         from d in db.LoaiPhongs
                         where (c.MaLoaiPhong == d.MaLoaiPhong && c.MaDanhMucPhong == null)
                         select c;
           
            cbPhong.DataSource = result.ToList();
            cbPhong.DisplayMember = "TenPhong";
            cbPhong.ValueMember = "MaPhong";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvCT.Rows.Count - 1; i++)
            {
                if (dgvCT[0,i].Value.ToString() == cbPhong.SelectedValue.ToString())
                {
                    MessageBox.Show("Phòng này đã được thêm ở dưới. Vui lòng chọn phòng khác");
                    return;
                }
            }


            Phong ph = db.Phongs.Find(cbPhong.SelectedValue.ToString());
            LoaiPhong lp = db.LoaiPhongs.Find(ph.MaLoaiPhong);

            string[] row1;
            row1 = new string[]
            {
                cbPhong.SelectedValue.ToString(),
                cbPhong.Text,
                ph.MaLoaiPhong,
                lp.DonGia.ToString()
            };
            dgvCT.Rows.Add(row1);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtTenDm.Text == "")
            {
                MessageBox.Show("Bạn chưa đặt tên cho danh mục phòng này.");
                return;
            }
            
            if (dgvCT.Rows.Count - 1 == 0)
            { 
                MessageBox.Show("Chưa có thông tin chi tiết danh mục phòng, không thể tạo danh mục phòng.");
                return;
            }
            string madmp = txtMaDM.Text;
            string tendmp = txtTenDm.Text;

            var checktendmp = from c in db.DanhMucPhongs where c.TenDMP == tendmp select c;
            if (checktendmp.Count() != 0)
            {
                MessageBox.Show("Tên danh mục này đã tồn tại.");
                return;
            }

            var checkdmp = from c in db.DanhMucPhongs where c.MaDMPhong == madmp select c;
            if (checkdmp.Count() != 0)
            {
                MessageBox.Show("Danh mục này đã tồn tại.");
                return;
            }
            var result = from c in db.DanhMucPhongs select c;
            DanhMucPhong dmp = new DanhMucPhong()
            {
                MaDMPhong = "DMP" + (result.Count() + 1),
                Xoa = 0,
                TenDMP = txtTenDm.Text
            };
            db.DanhMucPhongs.Add(dmp);
            for (int i = 0; i < (dgvCT.Rows.Count - 1); i++)
            {
                var re = from c in db.Phongs select c;
                foreach (var a in re)
                {
                    if (dgvCT[0,i].Value.ToString() == a.MaPhong)
                    {
                        a.MaDanhMucPhong = "DMP" + (result.Count() + 1);
                    }
                }
            }
            db.SaveChanges();
            MessageBox.Show("Lập danh mục phòng thành công");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtMaDM.Text = "";
            txtTenDm.Text = "";
            dgvCT.Rows.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Phog.LapPhong lphong = new Phog.LapPhong();
            lphong.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TimKiem tk = new TimKiem(SetValue);
            tk.ShowDialog();
        }
        private void SetValue(String value)
        {
            this.txtMaDM.Text = value;
        }

        private void txtMaDM_TextChanged(object sender, EventArgs e)
        {
            string madmp = txtMaDM.Text;
            DanhMucPhong dmp = db.DanhMucPhongs.Find(madmp);
            txtTenDm.Text = dmp.TenDMP;
            var result = from c in db.Phongs
                         from d in db.LoaiPhongs
                         where (c.MaLoaiPhong == d.MaLoaiPhong && c.MaDanhMucPhong == madmp)
                         select new
                         {
                             c.MaPhong,
                             c.TenPhong,
                             c.MaLoaiPhong,
                             d.DonGia
                         };

            foreach (var i in result)
            {
                string[] row1;
                row1 = new string[]
                {
                i.MaPhong,
                i.TenPhong,
                i.MaLoaiPhong,
                i.DonGia.ToString()
                };
                dgvCT.Rows.Add(row1);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (txtMaDM.Text == "")
            {
                MessageBox.Show("Danh mục này không tồn tại");
                return;
            }
            DanhMucPhong dmp = db.DanhMucPhongs.Find(txtMaDM.Text);
            dmp.Xoa = 1;
            db.SaveChanges();
            MessageBox.Show("Xóa danh mục thành công");
            txtMaDM.Text = "";
            txtTenDm.Text = "";
            dgvCT.Rows.Clear();
        }
    }
}
