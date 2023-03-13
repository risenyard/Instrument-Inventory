using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;


namespace INSTRUMENT
{
    
    public partial class Form1 : Form
    {
        Inventory myInventory;//声明一个乐器库对象
        public Form1()
        {
            InitializeComponent();
            myInventory = new Inventory();//初始化乐器库对象

        }

        /// <summary>
        /// 导入txt文件，存入乐器信息到乐器库中，并显示在DataGridView中
        /// </summary>
        private void button1_Click(object sender, EventArgs e) 
        {
            ///
            ///OpenFileDialog设置
            ///
            OpenFileDialog openFileDialog1 = new OpenFileDialog();//建立openFileDialog
            openFileDialog1.Title = "导入文本文件";//openFileDialog设置，实现文件选择
            openFileDialog1.Filter = "文本文件 (*.txt) |*.txt|所有文件(*.*) |*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            ///
            ///DialogResult判断，需要值为OK
            ///
            DialogResult result = openFileDialog1.ShowDialog(); //ShowDialog
            if (result == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                myInventory.TxtRead(filename);
            }
            myInventory.ShowGrid(dataGridView1);//调用乐器库的显示函数
        }

        /// <summary>
        /// 根据乐器库的数据导出txt文件
        /// </summary>
        private void button2_Click(object sender, EventArgs e)   
        {
            ///
            ///SaveFileDialog设置
            ///
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();//声明saveFileDialog
            saveFileDialog1.Title = "导出为文本文件";
            saveFileDialog1.Filter = "文本文件 (*.txt) |*.txt";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            ///
            ///DialogResult判断，需要值为OK
            ///
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;//根据文件名写入信息
                myInventory.TxtWrite(filename);
            }
            myInventory.ShowGrid(dataGridView1);
        }
        /// <summary>
        ///删除DataGridView中选中的一行(包括显示情况和后台数据）
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                ///
                ///弹出警告窗口
                ///
                DialogResult result = MessageBox.Show("是否确认删除数据?",
               "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                ///
                ///删除吉他库内数据和显示数据
                ///
                if (result == DialogResult.Yes)
                {
                    int guitarindex = this.dataGridView1.SelectedRows[0].Index;//获取乐器序号
                    myInventory.DeleteAnInstrument(guitarindex);//删除乐器
                    myInventory.ShowGrid(dataGridView1);//重新显示datagridview
                }
                             
            }
            catch { MessageBox.Show("操作无效！请删除有数据的一整列"); }
        }
        /// <summary>
        ///手动输入一组乐器信息数据，存入到乐器库中
        /// </summary>
        private void button4_Click(object sender, EventArgs e) 
        {
            Form2 f2 = new Form2();//声明新窗体
            f2.ShowDialog();//弹出新窗体
            if (f2.DialogResult == DialogResult.OK)//点击确认，执行写入操作
            {
                List<object> tdata = f2.Data;
                InstrumentSpec spec = new InstrumentSpec((Builder_Enum)tdata[2], (string)tdata[3], (Type_Enum)tdata[4], (Wood_Enum)tdata[5], (Wood_Enum)tdata[6],(Species_Enum)tdata[7],(string)tdata[8]);
                Instrument newinstrument = new Instrument((string)tdata[0],(string)tdata[1], spec);//声明新乐器对象
                myInventory.StoreAnInstrument(newinstrument);//存入乐器库
                myInventory.ShowGrid(dataGridView1);//显示到datagridview中
            }
        }
        /// <summary>
        ///清空所有数据（包括显示数据和后台数据）
        /// </summary>
        private void button5_Click(object sender, EventArgs e) 
        {
            DialogResult result = MessageBox.Show("是否确认清空数据？", 
                "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                myInventory.ClearAllData();//清除乐器库数据
                myInventory.ShowGrid(dataGridView1);//重新显示
            }

        }
        /// <summary>
        ///显示DataGridView每一行的序号
        /// </summary>
        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow r = this.dataGridView1.Rows[i];
                r.HeaderCell.Value = string.Format("{0}", i + 1);
            }
            this.dataGridView1.Refresh();
        }
        /// <summary>
        /// 按乐器内部属性值对乐器库进行查找，显示对应乐器信息
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();//声明新窗体
            f3.ShowDialog();//显示窗体
            Inventory chosenInventory=null;//返回乐器库
            ///
            ///点击确认按钮后执行操作
            ///
            if (f3.DialogResult == DialogResult.OK)
            {
                ///
                ///使用新窗口输入的查找类别和匹配文本调用查找函数，根据查找结果建立一个新吉他库
                ///
                List<object> tdata = f3.Data;
                
                InstrumentSpec searchspec = new InstrumentSpec((Builder_Enum)tdata[0], (string)tdata[1], (Type_Enum)tdata[2], (Wood_Enum)tdata[3], (Wood_Enum)tdata[4], (Species_Enum)tdata[5], (string)tdata[6]);
                chosenInventory = myInventory.Inquiry(searchspec);
                //如果查找结果为空            
                if (chosenInventory == null)
                {
                    MessageBox.Show("没有您要查找的内容");
                }
                //如果查找结果不为空
                else
                {
                    chosenInventory.ShowGrid(dataGridView1);//显示储存了我们要找的吉他对象的新吉他库到datagridview中
                    ///
                    ///此时显示的信息处在特殊状况，为防止其它功能干扰程序运行，因此禁用之
                    ///
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button4.Enabled = false;
                    button5.Enabled = false;
                    button6.Enabled = false;
                }
            }
        }
        /// <summary>
        /// 重新显示乐器库中数据到datagridview内（查找某个乐器后，datagridview显示了选定的乐器信息，且禁用了其它按钮，该功能使之回到查找前状态）
        /// </summary>
        private void button7_Click(object sender, EventArgs e)
        {
            myInventory.ShowGrid(dataGridView1);//显示数据
            ///
            ///解禁其它所有功能
            ///
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
        }
        /// <summary>
        /// 实现窗口背景渐变
        /// </summary>
        private void Form1_Shown(object sender, EventArgs e)
        {
            while(true)//永续循环
            {
                Random rd = new Random();//设置随机数使得每次循环呈现的渐变不一样
                int s = rd.Next(0, 254);
                for (int i = 0; i < 254; i++)//改变rgb值
                {
                    this.BackColor = Color.FromArgb(255-i, s, i);
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(6);
                }
                for (int i = 254; i > 0; i--)//改变rgb值
                {
                    this.BackColor = Color.FromArgb(255-i, s, i);
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(6);
                }
            }
            
        }

    }
}
