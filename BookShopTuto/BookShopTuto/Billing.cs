using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;//remember!
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookShopTuto
{
    public partial class Billing : Form
    {
        public Billing()
        {
            InitializeComponent();
            populate();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\86130\Documents\WhiteBookShopDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()//将数据库的信息显示到数据网格中
        {
            Con.Open();
            string query = "select * from BookTb1";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);//数据库批量数据抓取
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);//SqlDataAdapter和SqlCommandBuilder通常搭配使用，进行批量处理数据库数据，后者参数是前者的对象
            var ds = new DataSet();//虚拟数据库
            sda.Fill(ds);//将刚刚的数据全部存储到虚拟表中
            BookDGV.DataSource = ds.Tables[0];//将数据显示到数据网格中，BookDGV是控件名称
            Con.Close();
        }
        
        int n = 0;
        double totalAmount = 0;//订单总额
        //购物车列表，不是一个固定的，通用的列表，根据每次结算情况不同会有所不同，所以不必新建后台的数据库表，直接每次运行往里加东西就可以了
        private void AddtoBillBtn_Click(object sender, EventArgs e)//加入购物车按钮
        {
   
            if(BTitleTb.Text =="" || PriceTb.Text == "")
            {
                MessageBox.Show("没选中商品！");
            }
            else if (QtyTb.Text == "")//没选中数量
            {
                MessageBox.Show("没选购买数量！");
            }
            else if (Convert.ToDouble(QtyTb.Text.ToString()) > stock)//买的数量大于库存量
            {
                MessageBox.Show("库存不足！");

            }
            else
            {
                double total = Convert.ToDouble(QtyTb.Text)*Convert.ToDouble(PriceTb.Text);//金额总额:数量*单价
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(BillDGV);
                newRow.Cells[0].Value = n + 1;//订单编号
                newRow.Cells[1].Value = BTitleTb.Text;//品名
                newRow.Cells[2].Value = PriceTb.Text;//单价
                newRow.Cells[3].Value = QtyTb.Text;//数量
                newRow.Cells[4].Value = total;//总价
                BillDGV.Rows.Add(newRow);
                totalAmount += total;//所有订单金额的计算
                BillTotal.Text = "订单总额：" + totalAmount + " 元";
                n++;
            }
            UpdateBook();
            populate();
        }

        private void UpdateBook()//在加入购物车时候更新数据库信息
        {
             
               double numberb = Convert.ToDouble(QtyTb.Text.ToString());//更新的库存量
               stock -= numberb;
            if (stock <= 0) stock += numberb;
            try
            {
                Con.Open();
                string query = "update BookTb1 set BQty = " + stock + " where BId = '" + key + "' ";//更新数量,记住写上where
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                Con.Close();
            }
            catch (Exception Ex)//如果try中出现异常，就会跳到catch中执行
            {
                MessageBox.Show(Ex.Message);
            }
        }

        int key = 0;
        double stock = 0;
        private void BookDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)//选定商品
        {
            BTitleTb.Text = BookDGV.SelectedRows[0].Cells[1].Value.ToString();//把数据网格第0行第一格的数据赋给标题tb的文本内容
            PriceTb.Text = BookDGV.SelectedRows[0].Cells[5].Value.ToString();
            QtyTb.Text = "";
                //BookDGV.SelectedRows[0].Cells[4].Value.ToString();
            if (BTitleTb.Text == "")
            {
                key = 0;
                stock= 0;
            }
            else
            {
                key = Convert.ToInt32(BookDGV.SelectedRows[0].Cells[0].Value.ToString());//把商品id编号转换成int32位赋给key
                stock = Convert.ToDouble(BookDGV.SelectedRows[0].Cells[4].Value.ToString());//该品的库存量
            }
        }
        private void Reset()
        {
            BTitleTb.Text = "";
            QtyTb.Text = "";
            PriceTb.Text = "";
        }

        private void PrintBtn_Click(object sender, EventArgs e)//结算打印按钮
        {
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 410, 600);

            if (BillDGV.Rows[0].Cells[0].Value == null)//没有内容
            {
                MessageBox.Show("没有任何商品加入购物车！");//缺一不可，缺少一个信息就报错
            }
            else//如果每个信息都有数据
            {
                if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }
                try
                {
                    Con.Open();//保存操作前要先打开数据库，所以先把open和close先写好
                    string query = "insert into BillTbl values('" + UserNameLbl.Text + "', " + totalAmount + ")";
                    SqlCommand cmd = new SqlCommand(query, Con);//sqlcommand对象是一个数据库命令对象，主要功能是向数据库发送 增删改查 操作的sql语句
                    //因此可利用它和表示数据插入的sql语句来创建书目记录的数据库插入操作
                    cmd.ExecuteNonQuery();//执行目标操作，并更新数据库数据
                    Con.Close();
                    //populate();//添加完就更新一下
                    //Reset();//添加栏重置，值置为空
                    totalAmount = 0;
                }
                catch (Exception Ex)//如果try中出现异常，就会跳到catch中执行
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void Billing_Load(object sender, EventArgs e)//整个面板，显示当前用户名
        {
            UserNameLbl.Text = Login.UserName;//跨界面调用登陆界面的登录名
        }
        int prodid;
        double prodqty, prodprice, tottal;
        int pos = 60;

        private void label6_Click(object sender, EventArgs e)//exit
        {
            Application.Exit();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)//绘制打印内容
        {
            e.Graphics.DrawString("江安超市", new Font("幼圆", 8, FontStyle.Bold), Brushes.Chocolate, new Point(180,10));//订单名称，表头，内容
            e.Graphics.DrawString("编号   品名       价格     数量     总计", new Font("幼圆", 12, FontStyle.Bold), Brushes.Black, new Point(30,40));
            foreach(DataGridViewRow row in BillDGV.Rows)
            {
                prodid = Convert.ToInt32(row.Cells["Column7"].Value);
                prodname = "" + row.Cells["Column8"].Value;
                prodprice = Convert.ToDouble(row.Cells["Column9"].Value);
                prodqty = Convert.ToDouble(row.Cells["Column10"].Value);
                tottal = Convert.ToDouble(row.Cells["Column11"].Value);
                e.Graphics.DrawString("" + prodid, new Font("幼圆", 8, FontStyle.Regular), Brushes.Black, new Point(45, pos));
                e.Graphics.DrawString("" + prodname, new Font("幼圆", 8, FontStyle.Regular), Brushes.Black, new Point(75, pos));
                e.Graphics.DrawString("" + prodprice, new Font("幼圆", 8, FontStyle.Regular), Brushes.Black, new Point(198, pos));
                e.Graphics.DrawString("" + prodqty, new Font("幼圆", 8, FontStyle.Regular), Brushes.Black, new Point(277, pos));
                e.Graphics.DrawString("" + tottal, new Font("幼圆", 8, FontStyle.Regular), Brushes.Black, new Point(348, pos));
                pos += 20;
            }
            e.Graphics.DrawString("订单总额：￥ " + totalAmount, new Font("幼圆", 12, FontStyle.Bold), Brushes.Black, new Point(60, pos + 50));
            e.Graphics.DrawString("*****************江安超市*****************", new Font("幼圆", 10, FontStyle.Bold), Brushes.Black, new Point(50, pos + 85));
            //小票打印出来后，清空购物车中的内容
            BillDGV.Rows.Clear();
            BillDGV.Refresh();
            BillTotal.Text = "";
            pos = 100;
            //totalAmount = 0;//重置
        }

        string prodname;

        private void ResetBtn_Click(object sender, EventArgs e)//重置按钮
        {
            Reset();
        }

        private void label8_Click(object sender, EventArgs e)//退出按钮
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }
    }
}
