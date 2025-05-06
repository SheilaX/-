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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)//exit关闭界面按钮
        {
            Application.Exit();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\86130\Documents\WhiteBookShopDb.mdf;Integrated Security=True;Connect Timeout=30");
        public static string UserName = "";//全局静态字符串变量，用于在订单结算界面显示当前用户名
        private void button1_Click(object sender, EventArgs e)//登录按钮
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select count(*) from UserTb1 where Uname='"+ UNameTb.Text +"'and Upassword='"+ UPassTb.Text +"'", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows[0][0].ToString() == "1")//系统用户
            {
                UserName = UNameTb.Text;
                Con.Close();
                Billing obj = new Billing();//新建订单结算界面对象
                obj.Show();//打开订单结算界面
                this.Hide();//隐藏当前界面
            }
            else
            {
                MessageBox.Show("用户名或密码错误！！！");
            }
            Con.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            AdminLogin obj = new AdminLogin();
            obj.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)//注册
        {
            signup n = new signup();
            n.Show();
            this.Hide();
        }
    }
}
