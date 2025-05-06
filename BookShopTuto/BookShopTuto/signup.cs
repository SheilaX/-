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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BookShopTuto
{
    public partial class signup : Form
    {
        public signup()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\86130\Documents\WhiteBookShopDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void button1_Click(object sender, EventArgs e)//注册按钮
        {
            if (UNameTb.Text == "" || UPassTb.Text == "")//任意一个为空
            {
                MessageBox.Show("信息缺失！");//缺一不可，缺少一个信息就报错
            }
            else//如果每个信息都有数据
            {
                try
                {
                    Con.Open();//保存操作前要先打开数据库，所以先把open和close先写好
                    string query = "insert into [UserTb1] values('" + UNameTb.Text + "'," + phone.Text + " ,'" + add.Text + "' ,'" + UPassTb.Text + "' )";
                    SqlCommand cmd = new SqlCommand(query, Con);//sqlcommand对象是一个数据库命令对象，主要功能是向数据库发送 增删改查 操作的sql语句
                    //因此可利用它和表示数据插入的sql语句来创建书目记录的数据库插入操作
                    cmd.ExecuteNonQuery();//执行目标操作，并更新数据库数据
                    MessageBox.Show("用户信息注册成功！");
                    Con.Close();
                    // populate();//添加完就更新一下
                    //Reset();//添加栏重置，值置为空
                }
                catch (Exception Ex)//如果try中出现异常，就会跳到catch中执行
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void label6_Click(object sender, EventArgs e)//x
        {
            Login m = new Login();
            m.Show();
            this.Hide();
        }
    }
}
