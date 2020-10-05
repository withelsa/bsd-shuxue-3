using System;

namespace bsd_shuxue_3.Utils
{
    internal class ItemWrapper<T>
    {
        public ItemWrapper()
        {
            // 默认构造函数
        }

        public ItemWrapper(T data, String text)
        {
            this.Data = data;
            this.Text = text;
        }

        public T Data { get; set; }

        public String Text { get; set; }

        public override string ToString()
        {
            return this.Text;
        }
    }
}