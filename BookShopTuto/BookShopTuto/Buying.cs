using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;//用于sql sever有关操作的类，记得添加！

namespace BookShopTuto
{
    public partial class Buying : Form
    {
        public Buying()
        {
            InitializeComponent();
            populate();//在数据网格中显示，这是进入界面就会显示的，所以放在这里
        }
        //新建全局数据库对象，使用该对象来操作数据库
        //参数类型为string，叫做连接字符串，获取或设置用于打开sql server数据库的字符串
        //一个到sql server 数据库的链接创建：
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\86130\Documents\WhiteBookShopDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()//将数据库的信息显示到数据网格中
        {
            Con.Open();
            string query = "select * from BookTb1";
            SqlDataAdapter sda = new SqlDataAdapter(query,Con);//数据库批量数据抓取
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);//SqlDataAdapter和SqlCommandBuilder通常搭配使用，进行批量处理数据库数据，后者参数是前者的对象
            var ds = new DataSet();//虚拟数据库
            sda.Fill(ds);//将刚刚的数据全部存储到虚拟表中
            BookDGV.DataSource = ds.Tables[0];//将数据显示到数据网格中，BookDGV是控件名称
            Con.Close();
        }
        private void SaveBtn_Click(object sender, EventArgs e)//保存功能的实现
        {
            if(BTitleTb.Text == "" || BAutTb.Text == "" || QtyTb.Text == "" || PriceTb.Text == "" || BCatCb.SelectedIndex == -1)//任意一个为空
            {
                MessageBox.Show("信息缺失！");//缺一不可，缺少一个信息就报错
            }
            else//如果每个信息都有数据
            {
                try
                {
                    Con.Open();//保存操作前要先打开数据库，所以先把open和close先写好
                    string query = "insert into BookTb1 values('" + BTitleTb.Text+"', '"+BAutTb.Text+ "', '"+ BCatCb.SelectedItem.ToString() + "', "+QtyTb.Text+", "+PriceTb.Text+ ", '"+ protimeTb.Text +"', '"+ warrantyTb.Text +"', '"+ specTb.Text +"')";
                    SqlCommand cmd = new SqlCommand(query, Con);//sqlcommand对象是一个数据库命令对象，主要功能是向数据库发送 增删改查 操作的sql语句
                    //因此可利用它和表示数据插入的sql语句来创建书目记录的数据库插入操作
                    cmd.ExecuteNonQuery();//执行目标操作，并更新数据库数据
                    MessageBox.Show("信息保存成功！");
                    Con.Close();
                    populate();//添加完就更新一下
                    Reset();//添加栏重置，值置为空
                }
                catch(Exception Ex)//如果try中出现异常，就会跳到catch中执行
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

       
        private void Filter1()//过滤商品类型信息
        {
            Con.Open();
            string query = "select * from BookTb1 where BCat = '" + CatCbSearchCb.Text.ToString() + "' ";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);//数据库批量数据抓取
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);//SqlDataAdapter和SqlCommandBuilder通常搭配使用，进行批量处理数据库数据，后者参数是前者的对象
            var ds = new DataSet();//虚拟数据库
            sda.Fill(ds);//将刚刚的数据全部存储到虚拟表中
            BookDGV.DataSource = ds.Tables[0];//将数据显示到数据网格中，BookDGV是控件名称
            Con.Close();
        }

        private void CatCbSearchCb_SelectionChangeCommitted(object sender, EventArgs e)//选定书目下拉列表功能
        {
            Filter1();
        }

        private void button5_Click(object sender, EventArgs e)//更新按钮
        {
            populate();//更新就是重新提取全部数据库记录，在对数据库数据进行增删查改之后列表的更新
            Reset();
            CatCbSearchCb.SelectedIndex = -1;//把选类型的下拉列表也设置为空
        }

        private void Reset()//重置功能：将输入框和下拉列表全部设置为空
        {
            BTitleTb.Text = "";
            BAutTb.Text = "";
            BCatCb.SelectedIndex = -1;
            QtyTb.Text = "";
            PriceTb.Text = "";
            protimeTb.Text = "";
            warrantyTb.Text = "";
            specTb.Text = "";
        }
        private void ResetBtn_Click(object sender, EventArgs e)//重置按钮
        {
            Reset();
            CatCbSearchCb.SelectedIndex=-1;//把选类型的下拉列表也设置为空
        }

        int key = 0;//主键
        private void BookDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)//数据网格选中功能
        {
            BTitleTb.Text = BookDGV.SelectedRows[0].Cells[1].Value.ToString();//把数据网格第0行第一格的数据赋给标题tb的文本内容
            BAutTb.Text = BookDGV.SelectedRows[0].Cells[2].Value.ToString();
            BCatCb.SelectedItem = BookDGV.SelectedRows[0].Cells[3].Value.ToString();
            QtyTb.Text = BookDGV.SelectedRows[0].Cells[4].Value.ToString();
            PriceTb.Text = BookDGV.SelectedRows[0].Cells[5].Value.ToString();
            protimeTb.Text = BookDGV.SelectedRows[0].Cells[6].Value.ToString();
            warrantyTb.Text = BookDGV.SelectedRows[0].Cells[7].Value.ToString();
            specTb.Text = BookDGV.SelectedRows[0].Cells[8].Value.ToString();
            if(BTitleTb.Text=="")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(BookDGV.SelectedRows[0].Cells[0].Value.ToString());//把书id编号转换成int32位赋给key
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)//删除按钮，把在数据网格中选中的条目从数据库中删除
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
                    string query = "delete from BookTb1 where BId = "+ key +"";//根据书的编号把选中条目删除
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("信息删除成功！");
                    Con.Close();
                    populate();//添加完就更新一下
                    Reset();//添加栏重置，值置为空
                }
                catch (Exception Ex)//如果try中出现异常，就会跳到catch中执行
                {
                    MessageBox.Show(Ex.Message);
                }
                Reset();
                CatCbSearchCb.SelectedIndex = -1;//把选类型的下拉列表也设置为空
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)//编辑按钮，修改选中条目信息
        {
            if (BTitleTb.Text == "" || BAutTb.Text == "" || QtyTb.Text == "" || PriceTb.Text == "" || BCatCb.SelectedIndex == -1)//没有得到书的编号
            {
                MessageBox.Show("信息缺失！");//缺一不可，缺少一个信息就报错
            }
            else//如果每个信息都有数据
            {
                try
                {
                    Con.Open();
                    string query = "update BookTb1 set Btitle = '"+ BTitleTb.Text +" ', BAuthor = '"+ BAutTb.Text +"' , BCat = '"+ BCatCb.SelectedItem.ToString() +"', BQty = '"+ QtyTb.Text +"', BPrice = '"+ PriceTb.Text +"' , Bprotime = '"+ protimeTb.Text +"', Bddl = '"+ warrantyTb.Text +"', Bspec = '"+ specTb.Text +"' where BId = '"+ key +"'";//根据书的编号把选中条目删除,记住写上where
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("信息更新完成！");
                    Con.Close();
                    populate();//添加完就更新一下
                    Reset();//添加栏重置，值置为空
                }
                catch (Exception Ex)//如果try中出现异常，就会跳到catch中执行
                {
                    MessageBox.Show(Ex.Message);
                }
                Reset();
                CatCbSearchCb.SelectedIndex = -1;//把选类型的下拉列表也设置为空
            }
        }

        private void label5_Click(object sender, EventArgs e)//用户按钮
        {
            Users obj = new Users();
            obj.Show();
            this.Hide();
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

        private void label6_Click(object sender, EventArgs e)//exit
        {
            Application.Exit();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void titlebox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Buying_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“whiteBookShopDbDataSet1.BookTb1”中。您可以根据需要移动或移除它。
            this.bookTb1TableAdapter1.Fill(this.whiteBookShopDbDataSet1.BookTb1);
            // TODO: 这行代码将数据加载到表“whiteBookShopDbDataSet.BookTb1”中。您可以根据需要移动或移除它。
            this.bookTb1TableAdapter.Fill(this.whiteBookShopDbDataSet.BookTb1);

        }

        private void CatCbSearchCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
