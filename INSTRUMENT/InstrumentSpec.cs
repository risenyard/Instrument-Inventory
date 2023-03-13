using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSTRUMENT
{
    /// <summary>
    /// 乐器属性抽象类
    /// </summary>
    public class InstrumentSpec
    {
        //包含乐器属性的字典及属性
        protected Dictionary<string, object> attributes = new Dictionary<string, object>();
        public Dictionary<string, object> Attributes { get { return attributes; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        public InstrumentSpec(Builder_Enum builder, string model, Type_Enum type,
        Wood_Enum backWood, Wood_Enum topWood,Species_Enum species,string numstringorstyle)
        {
            attributes.Add("species", species);
            attributes.Add("builder",builder);
            attributes.Add("model",model);
            attributes.Add("type",type);
            attributes.Add("backWood",backWood);
            attributes.Add("topWood",topWood);
            if (species == Species_Enum.GUITAR)
                attributes.Add("numString", numstringorstyle);
            if (species == Species_Enum.MANDOLIN)
                attributes.Add("style", numstringorstyle);
        }

        ///
        /// 返回乐器属性名
        /// 
        public object getattribute(string attributeName)
        {
            if (this.attributes.ContainsKey(attributeName))
                return this.attributes[attributeName];
            else
                return null;
        }

        /// <summary>
        /// 比较函数
        /// </summary>
         public Boolean matches(InstrumentSpec otherSpec)
        {
            Dictionary<string, object> otherattributes = otherSpec.Attributes;
            foreach(string key in otherattributes.Keys)
            {
                if(this.attributes.ContainsKey(key))
                {
                    if(!this.attributes[key].Equals(otherattributes[key]))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }    
    }
}
