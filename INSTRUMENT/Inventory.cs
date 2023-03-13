using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace INSTRUMENT
{   
    /// <summary>
    /// 乐器库类（存储生成的所有乐器，并实现与主窗口的交互）
    /// </summary>
    class Inventory
    {
        private List<string> Headertext;
        private List<Instrument> InstrumentInventory;//存储乐器的列表                                   

        /// <summary>
        /// 构造函数（生成一个空库）
        /// </summary>
        public Inventory()
        {
            Headertext = new List<string> {  "serialNumber", "price","species" ,"builder","model", "type",
            "backWood", "topWood","numstring/style"};
            InstrumentInventory = new List<Instrument>();
        }

        /// <summary>
        /// 向乐器库中存入一个乐器对象
        /// </summary>
        public void StoreAnInstrument(Instrument instrument)
        {          
            this.InstrumentInventory.Add(instrument);//存入乐器对象
        }

        /// <summary>
        /// 删除一个乐器对象（输入参数为显示在datagridview中的列数，对应乐器的序号）
        /// </summary>
        public void DeleteAnInstrument(int rowindex)
        {
            this.InstrumentInventory.RemoveAt(rowindex);
        }

        /// <summary>
        /// 删除库中所有乐器（重置该乐器库）
        /// </summary>
        public void ClearAllData()
        {
            this.InstrumentInventory.Clear();//清除吉他对象
        }

        /// <summary>
        ///通过乐器编号查找乐器
        /// </summary>
        public Inventory GetAnInstrument(string serialNumber)
        {
            Inventory chosenInventory = new Inventory();//建立一个新乐器库来存储查找后的吉他对象
            for (int i = 0; i < InstrumentInventory.Count; i++)
            {
                Instrument instrument = InstrumentInventory[i];
                if (instrument.SerialNumber.Equals(serialNumber))
                {
                    chosenInventory.StoreAnInstrument(instrument);
                    return chosenInventory;
                }
            }
            return null;
        }

        /// <summary>
        /// 通过乐器内部属性查找乐器
        /// </summary>
        public Inventory Inquiry(InstrumentSpec searchspec)
        {
            int index = 0;//如果匹配成功，该标记的值大于0；匹配失败，仍为0；
            Inventory chosenInventory = new Inventory();//建立一个新乐器库来存储查找后的乐器对象
            ///
            ///循环乐器库进行查找
            ///
            for (int i = 0; i < this.InstrumentInventory.Count; i++)
            {
                if (this.InstrumentInventory[i].Spec.matches(searchspec))//查找文本在对应项中匹配成功
                {
                    chosenInventory.StoreAnInstrument(this.InstrumentInventory[i]);//存储该乐器对象
                    index++;
                }
            }

            if (index == 0) return null;//没有匹配到对象返回null
            else 
            {
                chosenInventory.Headertext = this.Headertext;//给新的乐器库表头信息
                return chosenInventory; //匹配成功返回新的乐器库对象
            }
        }
        /// <summary>
        /// 在datagridview中显示乐器库类的数据
        /// </summary>
        public void ShowGrid(DataGridView dataGridView)

        {
            ///
            ///当datagridview中不存在表头时显示表头
            ///
            if  (dataGridView.Columns.Count==0)
            {
                for (int i = 0; i < this.Headertext.Count; i++)
                {
                    dataGridView.Columns.Add(this.Headertext[i], this.Headertext[i]);
                }
            }
            ///
            ///显示数据
            ///
            dataGridView.Rows.Clear();//清除原来显示的所有内容
            for (int i = 0; i < this.InstrumentInventory.Count; i++)
            {
                dataGridView.Rows.Add();//添加一行
                dataGridView.Rows[i].Cells[0].Value = this.InstrumentInventory[i].SerialNumber;
                dataGridView.Rows[i].Cells[1].Value = this.InstrumentInventory[i].Price;
                //循环填入乐器属性数据
                int j = 2;
                Dictionary<string, object>  aruattributes = this.InstrumentInventory[i].Spec.Attributes;
                foreach (string key in aruattributes.Keys)
                {
                    dataGridView.Rows[i].Cells[j].Value = aruattributes[key].ToString();
                    j++;
                }
            }

        }
        /// <summary>
        /// 读取txt数据到库中
        /// </summary>
        public void TxtRead(string filename)
        {
            ///
            ///根据文件名读取文件内容
            ///
            using (StreamReader read = new StreamReader(filename))
            {
                string headline = read.ReadLine();//阅读表头
                ///
                ///存储乐器数据到吉他库中
                ///
                while (!read.EndOfStream)
                {
                    string line = read.ReadLine();
                    string[] data = line.ToString().Split(';');
                    List<object> tdata = new List<object>();
                    //判断各项在枚举内的哪一项
                    for (int i = 2; i < 9; i++)
                    {
                        string compare = data[i].ToUpper();
                        if (i == 2)
                        {
                            Builder_Enum flag;
                            Enum.TryParse<Builder_Enum>(compare, true, out flag);
                            tdata.Add(flag);
                        }
                        if (i == 3)
                        {
                            tdata.Add(data[3]);
                            continue;
                        }
                        if (i == 4)
                        {
                            Type_Enum flag;
                            Enum.TryParse<Type_Enum>(compare, true, out flag);
                            tdata.Add(flag);
                        }
                        if (i == 5 || i == 6)
                        {
                            Wood_Enum flag;
                            Enum.TryParse<Wood_Enum>(compare, true, out flag);
                            tdata.Add(flag);
                        }
                        if (i == 7)
                        {
                            Species_Enum flag;
                            Enum.TryParse<Species_Enum>(compare, true, out flag);
                            tdata.Add(flag);
                        }
                        if (i == 8)
                        {
                            tdata.Add(data[8]);
                            continue;
                        }

                    }

                    InstrumentSpec spec = new InstrumentSpec((Builder_Enum)tdata[0], (string)tdata[1], (Type_Enum)tdata[2], (Wood_Enum)tdata[3], (Wood_Enum)tdata[4],(Species_Enum)tdata[5],(string)tdata[6]);
                    Instrument newinstrument = new Instrument(data[0], data[1], spec);//声明新乐器对象
                    StoreAnInstrument(newinstrument);//存入乐器库
                    newinstrument = null;//释放该对象                
                }
            }

        }
        /// <summary>
        /// 写入乐器库数据到txt中
        /// </summary>
        public void TxtWrite(string filename)
        {
            using (StreamWriter write = new StreamWriter(filename))
            {
                ///
                ///写入表头到文件中
                ///
                string headline = "";
                for (int i = 0; i < this.Headertext.Count; i++)
                {
                    headline = headline + Headertext[i] + ";";
                }
                write.WriteLine(headline);
                ///
                ///写入吉他数据到文件中
                ///
                for (int i = 0; i < this.InstrumentInventory.Count; i++)
                {
                    string line = "";
                    line=line+this.InstrumentInventory[i].SerialNumber+";";
                    line=line+this.InstrumentInventory[i].Price+";";
                    //循环填入乐器属性数据
                    Dictionary<string, object> aruattributes = this.InstrumentInventory[i].Spec.Attributes;
                    foreach (string key in aruattributes.Keys)
                    {
                        line=line+aruattributes[key]+";";
                    }
                    write.WriteLine(line);
                }
            }
        }
    }
}
