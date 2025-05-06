using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookShopTuto
{
    public partial class DashBoard : Form
    {
        public DashBoard()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\86130\Documents\WhiteBookShopDb.mdf;Integrated Security=True;Connect Timeout=30");


        private void label5_Click(object sender, EventArgs e)//用户按钮
        {
            Users obj = new Users();
            obj.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)//退出按钮
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label9_Click_2(object sender, EventArgs e)//书籍按钮
        {
            Buying obj = new Buying();
            obj.Show();
            this.Hide();
        }

        private void DashBoard_Load(object sender, EventArgs e)//面板
        {
            Con.Open();
            //SqlDataAdapter sda = new SqlDataAdapter("select sum(BQty) from BookTb1",Con);
            //DataTable dt = new DataTable();//存到虚拟数据表中
            //sda.Fill(dt);
            //BookStockLbl.Text = dt.Rows[0][0].ToString();

            SqlDataAdapter sda1 = new SqlDataAdapter("select sum(Amount) from BillTbl", Con);
            DataTable dt1 = new DataTable();//存到虚拟数据表中
            sda1.Fill(dt1);
            AmountLbl.Text = dt1.Rows[0][0].ToString();

            SqlDataAdapter sda2 = new SqlDataAdapter("select Count(*) from UserTb1", Con);
            DataTable dt2 = new DataTable();//存到虚拟数据表中
            sda2.Fill(dt2);
            UserTotalLbl.Text = dt2.Rows[0][0].ToString();
            Con.Close();
        }

        private void label6_Click(object sender, EventArgs e)//exit
        {
            Application.Exit();
        }
    }
}
