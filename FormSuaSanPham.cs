using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBH_Login_DanhMucSP_HoaDon
{
    public partial class FormSuaSanPham : Form
    {
        public FormSuaSanPham()
        {
            InitializeComponent();
        }
        private void FormSuaSanPham_Load(object sender, EventArgs e)
        {
            SanPham spChon = (SanPham)this.Tag;//sản phẩm sửa được truyền từ form Danh mục sản phẩm
            //lấy thông tin của sản phẩm được sửa hiển thị lên các điều khiển             
            textBoxMa.Text = spChon.MaSP;
            textBoxTen.Text = spChon.TenSP;
            textBoxSoLuong.Text = spChon.SoLuong.ToString();
            textBoxDonGia.Text = spChon.DonGia.ToString();
            textBoxMaLoai.Text = spChon.MaLoai;
        }
        private void buttonLuu_Click(object sender, EventArgs e)
        {
            QLBanHangDataContext db = new QLBanHangDataContext();
            SanPham spSua = db.SanPhams.SingleOrDefault(sp => sp.MaSP == textBoxMa.Text);
            spSua.TenSP = textBoxTen.Text;
            spSua.SoLuong = int.Parse(textBoxSoLuong.Text);
            spSua.DonGia = int.Parse(textBoxDonGia.Text);
            spSua.MaLoai = textBoxMaLoai.Text;//chú ý khi test phải nhập mã loại đã có trong bảng Loại sản phẩm
            //Lưu vào csdl
            db.SubmitChanges();
        }

        private void buttonDongForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
