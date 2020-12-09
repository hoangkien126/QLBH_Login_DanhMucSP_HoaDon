using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBH_Login_DanhMucSP_HoaDon{
    public partial class FormDanhMucSanPham : Form
    {
        public FormDanhMucSanPham()
        {
            InitializeComponent();
        }

        //Tạo đối tượng DataContext

        QLBanHangDataContext db = new QLBanHangDataContext();
        private void FormDanhMucSanPham_Load(object sender, EventArgs e)
        {
            HienThiDuLieu();
            TaoDanhSachCombo();
        }
        private void TaoDanhSachCombo()
        {//Tạo danh sách cho Combo box
            var loaiSanPham = from lsp in db.LoaiSanPhams
                              select lsp;//lấy dữ liệu trong bảng Loại sản phẩm
            comboBoxLoaiSanPham.DataSource = loaiSanPham;
            comboBoxLoaiSanPham.DisplayMember = "TenLoai";//tên cột hiển thị
            comboBoxLoaiSanPham.ValueMember = "MaLoai";//tên cột lấy dữ liệu
        }
        private void HienThiDuLieu()
        {//hiển thị dữ liệu trong DataGrid View
            dataGridViewSanPham.Rows.Clear();//xóa các dòng trong DataGrid view
            //Lấy dữ liệu trong bảng Sản phẩm
            var sanPhamQuery = from sp in db.SanPhams
                               select new
                               {
                                   sp.MaSP,
                                   sp.TenSP,
                                   sp.LoaiSanPham.TenLoai,
                                   sp.SoLuong,
                                   sp.DonGia
                               };
            //Duyệt từng dòng sản phẩm lấy được đưa vào Data Gridview
            foreach (var item in sanPhamQuery)
            {
                DataGridViewRow dongMoi = (DataGridViewRow)
                dataGridViewSanPham.Rows[0].Clone();//tạo bản sao của 1 dòng của DataGridView Sản phẩm
                //Thêm dữ liệu vào các cột
                dongMoi.Cells[0].Value = item.MaSP;
                dongMoi.Cells[1].Value = item.TenSP;
                dongMoi.Cells[2].Value = item.TenLoai;
                dongMoi.Cells[3].Value = item.SoLuong;
                dongMoi.Cells[4].Value = item.DonGia;
                dongMoi.Cells[5].Value = "Xóa";
                dongMoi.Cells[6].Value = "Sửa";
                //Thêm dòng mới vào DataGridView
                dataGridViewSanPham.Rows.Add(dongMoi);
            }
        }
        private void dataGridViewSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {//Xóa hoặc sửa dữ liệu của 1 dòng
            //lấy mã sản phẩm của dòng được user click
            string maSanPham = dataGridViewSanPham.Rows[e.RowIndex].Cells[0].Value.ToString();
            //Lấy sản phẩm muốn xóa hoặc sửa
            SanPham spXoaSua = db.SanPhams.SingleOrDefault(sp => sp.MaSP == maSanPham);
            if (e.ColumnIndex == 5)//nếu user click Xóa thì xóa sản phẩm được chọn
            {
                db.SanPhams.DeleteOnSubmit(spXoaSua);
                db.SubmitChanges();
                HienThiDuLieu();
            }
            else if (e.ColumnIndex == 6)//nếu user click Sửa thì hiển thị form sửa t.tin sản phẩm
            {
                FormSuaSanPham mySuaSanPham = new FormSuaSanPham();
                mySuaSanPham.Tag = spXoaSua;
                mySuaSanPham.ShowDialog();
            }
        }
        private void buttonThem_Click(object sender, EventArgs e)
        {
            //Tạo đối tượng Sản phẩm mới
            SanPham spMoi = new SanPham();
            //gán giá trị cho thuộc tính của đối tượng sản phẩm mới là dữ liệu user nhập vào các điều khiển đơn
            spMoi.MaSP = textBoxMa.Text;
            spMoi.TenSP = textBoxTen.Text;
            spMoi.SoLuong =int.Parse( textBoxSoLuong.Text);
            spMoi.DonGia = int.Parse(textBoxDonGia.Text);
            spMoi.MaLoai = comboBoxLoaiSanPham.SelectedValue.ToString();
            //Thêm vào tập hợp Sản phẩm
            db.SanPhams.InsertOnSubmit(spMoi);
            //Lưu vào csdl
            db.SubmitChanges();
            //Hiển thị lại dữ liệu
            HienThiDuLieu();
        }

        private void buttonTim_Click(object sender, EventArgs e)
        {
            dataGridViewSanPham.Rows.Clear();
            var timKiemQuery = from sp in db.SanPhams
                               where sp.TenSP.Contains(textBoxTim.Text)//lấy các sản phẩm tên chứa chuỗi user nhập vào textBoxTim
                                select new
                                {
                                    sp.MaSP,
                                    sp.TenSP,
                                    sp.LoaiSanPham.TenLoai,
                                    sp.SoLuong,
                                    sp.DonGia
                                };

            if (timKiemQuery.Count() == 0)
                MessageBox.Show("Không có sản phẩm nào chứa chuỗi " + textBoxTim.Text);
            else
                foreach (var item in timKiemQuery)
                {
                    DataGridViewRow dongMoi = (DataGridViewRow)
                    dataGridViewSanPham.Rows[0].Clone();
                    dongMoi.Cells[0].Value = item.MaSP;
                    dongMoi.Cells[1].Value = item.TenSP;
                    dongMoi.Cells[2].Value = item.TenLoai;
                    dongMoi.Cells[3].Value = item.SoLuong;
                    dongMoi.Cells[4].Value = item.DonGia;
                    dongMoi.Cells[5].Value = "Xóa";
                    dongMoi.Cells[6].Value = "Sửa";
                    //Thêm dòng mới vào DataGridView
                    dataGridViewSanPham.Rows.Add(dongMoi);
                }

        }
        private void FormDanhMucSanPham_Activated(object sender, EventArgs e)
        {
            HienThiDuLieu();//Hiển thị lại dữ liệu khi form trở thành form hiện hành
        }
    }
}
