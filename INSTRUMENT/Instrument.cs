using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSTRUMENT
{
    public class Instrument
    {
        /// <summary>
        /// 乐器属性字段及只读属性
        /// </summary>
        protected string serialNumber;
        protected string price;
        protected InstrumentSpec spec;

        public string SerialNumber { get { return serialNumber; } }
        public string Price { get { return price; } }
        public InstrumentSpec Spec { get { return spec; } }

        /// <summary>
        /// 乐器构造函数（传入一个字符串数组来赋予属性）
        /// </summary>
        public Instrument(string serialNumber, string price, InstrumentSpec spec)
        {
            this.serialNumber = serialNumber;
            this.price = price;
            this.spec = spec;
        }
    }
}

