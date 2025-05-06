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
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
            populate();
        }

        private void label6_Click(object sender, EventArgs e)//exit
        {
            Application.Exit();
        }
        private void populate()//将数据库的信息显示到数据网格中
        {
            Con.Open();
            string query = "select * from UserTb1";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);//数据库批量数据抓取
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);//SqlDataAdapter和SqlCommandBuilder通常搭配使用，进行批量处理数据库数据，后者参数是前者的对象
            var ds = new DataSet();//虚拟数据库
            sda.Fill(ds);//将刚刚抓取的数据全部存储到虚拟表中
            UserDGV.DataSource = ds.Tables[0];//将数据显示到数据网格中，UserDGV是控件名称
            Con.Close();
        }
        private void Reset()//重置功能：将输入框和下拉列表全部设置为空
        {
           UnameTb.Text = "";
            PhoneTb.Text = "";
            AddTb.Text = "";
            PassTb.Text = "";
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\86130\Documents\WhiteBookShopDb.mdf;Integrated Security=True;Connect Timeout=30");
        //保存按钮
        private void button1_Click(object sender, EventArgs e)
        {
            if (UnameTb.Text == "" || PhoneTb.Text == "" || AddTb.Text == "" || PassTb.Text == "" )//任意一个为空
            {
                MessageBox.Show("信息缺失！");//缺一不可，缺少一个信息就报错
            }
            else//如果每个信息都有数据
            {
                try
                {
                    Con.Open();//保存操作前要先打开数据库，所以先把open和close先写好
                    string query = "insert into UserTb1 values('" + UnameTb.Text + "', '" + PhoneTb.Text + "', '" + AddTb.Text + "', '" +PassTb.Text + "')";
                    SqlCommand cmd = new SqlCommand(query, Con);//sqlcommand对象是一个数据库命令对象，主要功能是向数据库发送 增删改查 操作的sql语句
                    //因此可利用它和表示数据插入的sql语句来创建书目记录的数据库插入操作
                    cmd.ExecuteNonQuery();//执行目标操作，并更新数据库数据
                    MessageBox.Show("用户信息保存成功！");
                    Con.Close();
                    populate();//添加完就更新一下
                    Reset();//添加栏重置，值置为空
                }
                catch (Exception Ex)//如果try中出现异常，就会跳到catch中执行
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void ResetBtn_Click(object sender, EventArgs e)//重置按钮
        {
            Reset();
        }
       
       
        private void DeleteBtn_Click(object sender, EventArgs e)//删除按钮
        {
            if (key == 0)//没有得到书的编号
            {
                MessageBox.Show("信息缺失！");//缺一不可，缺少一个信息就报错
            }
            else//如果每个信息都有数据
            {
                try
                {
                    Con.Open();
                    string query = "delete from UserTb1 where UId = " + key + "";//根据书的编号把选中条目删除
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("用户信息删除成功！");
                    Con.Close();
                    populate();//添加完就更新一下
                    Reset();//添加栏重置，值置为空
                }
                catch (Exception Ex)//如果try中出现异常，就会跳到catch中执行
                {
                    MessageBox.Show(Ex.Message);
                }
                Reset();
            }
        }
        int key = 0;//主键
        private void UserDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)//数据网格选中功能
        {
            UnameTb.Text = UserDGV.SelectedRows[0].Cells[1].Value.ToString();
            PhoneTb.Text = UserDGV.SelectedRows[0].Cells[2].Value.ToString();
            AddTb.Text = UserDGV.SelectedRows[0].Cells[3].Value.ToString();
            PassTb.Text = UserDGV.SelectedRows[0].Cells[4].Value.ToString();
            if (UnameTb.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(UserDGV.SelectedRows[0].Cells[0].Value.ToString()); 
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)//编辑按钮，修改
        {
            if (UnameTb.Text == "" || PhoneTb.Text == "" || AddTb.Text == "" || PassTb.Text == "")
            {
                MessageBox.Show("信息缺失！");//缺一不可，缺少一个信息就报错
            }
            else//如果每个信息都有数据
            {
                try
                {
                    Con.Open();
                    string query = "update UserTb1 set UName = '"+ UnameTb.Text +"', Uphone = '"+ PhoneTb.Text +"', UAdd = '"+ AddTb.Text +"', UPassword = '"+ PassTb.Text +"' where UId = " + key + "";//根据书的编号修改信息
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("用户信息修改成功！");
                    Con.Close();
                    populate();//添加完就更新一下
                    Reset();//添加栏重置，值置为空
                }
                catch (Exception Ex)//如果try中出现异常，就会跳到catch中执行
                {
                    MessageBox.Show(Ex.Message);
                }
                Reset();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label7_Click(object sender, EventArgs e)//账户管理按钮
        {
            DashBoard obj = new DashBoard();
            obj.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)//退出按钮
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
           
        }

        private void label9_Click_1(object sender, EventArgs e)//书籍按钮
        {
            Buying obj = new Buying();
            obj.Show();
            this.Hide();
        }
    }
}
