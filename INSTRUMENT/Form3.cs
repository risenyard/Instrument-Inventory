using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INSTRUMENT
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 声明一个返回的列表
        /// </summary>
        private List<object> data;
        public List<object> Data
        {
            get { return this.data; }
        }
        /// <summary>
        /// 点击确认按钮的发生事件
        /// </summary>>
        private void button1_Click(object sender, EventArgs e)
        {
            ///
            ///如果所有文本框都不为空，将输入信息写入列表，返回OK
            ///
            if ( this.textBox1.Text != "" && this.textBox2.Text != ""&& this.comboBoxSpecies.Text != "" && comboBox1.Text != ""&& comboBox2.Text != "" && comboBox3.Text != "" && comboBox4.Text != "")
            {
                //添加乐器属性
                data = new List<object>();
                Builder_Enum flag1;
                Enum.TryParse<Builder_Enum>(comboBox1.Text, true, out flag1);
                data.Add(flag1);
                data.Add(this.textBox1.Text);
                Type_Enum flag2;
                Enum.TryParse<Type_Enum>(comboBox2.Text, true, out flag2);
                data.Add(flag2);
                Wood_Enum flag3;
                Enum.TryParse<Wood_Enum>(comboBox3.Text, true, out flag3);
                data.Add(flag3);
                Wood_Enum flag4;
                Enum.TryParse<Wood_Enum>(comboBox4.Text, true, out flag4);
                data.Add(flag4);
                Species_Enum flag5;
                Enum.TryParse<Species_Enum>(comboBoxSpecies.Text, true, out flag5);
                data.Add(flag5);
                data.Add(textBox2.Text);
                this.DialogResult = DialogResult.OK;
            }
            ///
            ///如果有至少一个文本框为空，则弹出提醒，继续停留在该窗口
            ///
            else MessageBox.Show("请完整填充所有空缺");
        }
    }
}
