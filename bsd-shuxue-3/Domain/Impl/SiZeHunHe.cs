using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace bsd_shuxue_3.Domain.Impl
{
    /// <summary>
    /// 四则混合运算的设置
    /// </summary>
    internal class SiZeHunHeConfig : IValidatable
    {
        /// <summary>
        /// 可以使用的最大数字
        /// </summary>
        [Category("数字范围")]
        [Description("可以使用的最大数字")]
        public uint MaxNumber { get; set; } = 999;

        /// <summary>
        /// 可以使用的最小数字
        /// </summary>
        [Category("数字范围")]
        [Description("可以使用的最小数字")]
        public uint MinNumber { get; set; } = 1;

        /// <summary>
        /// 最大结果
        /// </summary>
        [Category("结果范围")]
        [Description("最大结果")]
        public uint MaxResult { get; set; } = 2000;

        /// <summary>
        /// 最小结果
        /// </summary>
        [Category("结果范围")]
        [Description("最小结果")]
        public uint MinResult { get; set; } = 1;

        public bool Validate(IList<string> messages)
        {
            if (this.MaxNumber < this.MinNumber)
            {
                messages.Add("【最大数字】不能小于【最小数字】");
            }
            if (this.MaxResult < this.MinResult)
            {
                messages.Add("【最大结果】不能小于【最小结果】");
            }
            return messages.Count < 1;
        }
    }

    /// <summary>
    /// 四则混合运算问题的生成器
    /// </summary>
    internal class SiZeHunHeQuestionFactory : QuestionFactoryBase<SiZeHunHeConfig>
    {
        public SiZeHunHeQuestionFactory()
        {
            base.Code = "SiZeHunHe";
            base.Title = "四则混合运算";
            base.Description = String.Join("\r\n", "▶ 100 以内的乘除法", "▶ 1000 以内的加减法", "▶ 带括号");
            base.Config = new SiZeHunHeConfig();
        }

        protected override string CreateQuestionContent()
        {
            String content = null;

            // 决定主操作符
            if (this.NextBoolean())
            {
                // 主运算为加减法，辅运算为乘除法
                content = this.createContent4plusMinus();
            }
            else
            {
                // 主运算为乘除法，辅运算为加减法
                content = this.createContent4multiplyDivide();
            }

            // 返回
            return content;
        }

        private string createContent4multiplyDivide()
        {
            String content = null;
            int n1 = 0, n2 = 0, n3 = 0, subResult = 0;
            String op1 = null, op2 = null;

            // 生成主运算的两个数字
            if (this.NextBoolean())
            {
                // 主运算使用乘法
                op1 = OP_MULTPLY;
                this.nextMultiplyPair(ref n1, ref subResult);
            }
            else
            {
                // 主运算使用除法
                op1 = OP_DIVIDE;
                this.nextDividePair(ref n1, ref subResult);
            }

            // 生成辅运算的两个数字
            if (this.NextBoolean())
            {
                // 辅运算使用加法
                op2 = OP_PLUS;
                if (this.NextBoolean())
                {
                    // 括号用于计算 n1
                    n2 = this.Random.Next((int)this.Config.MinNumber, n1);
                    n3 = n1 - n2;
                    content = String.Format("({0} {1} {2}) {3} {4}", n2, op2, n3, op1, subResult);
                }
                else
                {
                    // 括号用于计算 subResult
                    n2 = this.Random.Next((int)this.Config.MinNumber, subResult);
                    n3 = subResult - n2;
                    content = String.Format("{0} {1} ({2} {3} {4})", n1, op1, n2, op2, n3);
                }
            }
            else
            {
                // 辅运算使用减法
                op2 = OP_MINUS;
                if (this.NextBoolean())
                {
                    // 括号用于计算 n1
                    n2 = this.Random.Next(n1, (int)this.Config.MaxNumber);
                    n3 = n2 - n1;
                    content = String.Format("({0} {1} {2}) {3} {4}", n2, op2, n3, op1, subResult);
                }
                else
                {
                    // 括号用于计算 subResult
                    n2 = this.Random.Next(subResult, (int)this.Config.MaxNumber);
                    n3 = n2 - subResult;
                    content = String.Format("{0} {1} ({2} {3} {4})", n1, op1, n2, op2, n3);
                }
            }

            // 返回
            return content;
        }

        /// <summary>
        /// 主运算为加减法，辅运算为乘除法
        /// </summary>
        /// <returns></returns>
        private string createContent4plusMinus()
        {
            String content = null;
            int n1 = 0, n2 = 0, n3 = 0, subResult = 0;
            String op1 = null, op2 = null;

            // 生成辅运算的两个数字
            if (this.NextBoolean())
            {
                // 辅运算使用乘法
                op2 = OP_MULTPLY;
                this.nextMultiplyPair(ref n2, ref n3);
                subResult = n2 * n3;
            }
            else
            {
                // 辅运算使用除法
                op2 = OP_DIVIDE;
                this.nextDividePair(ref n2, ref n3);
                subResult = n2 / n3;
            }
            content = String.Format("{0} {1} {2}", n2, op2, n3);

            // 生成主运算的数字
            if (this.NextBoolean())
            {
                // 主运算使用加法
                op1 = OP_PLUS;
                n1 = this.Random.Next((int)this.Config.MinNumber, (int)this.Config.MaxNumber);
                if (this.NextBoolean())
                {
                    // 乘法在前
                    content = String.Format("{0} {1} {2}", content, op1, n1);
                }
                else
                {
                    // 乘法在后
                    content = String.Format("{0} {1} {2}", n1, op1, content);
                }
            }
            else
            {
                // 主运算使用减法
                op1 = OP_MINUS;
                if (this.NextBoolean())
                {
                    // 除法在前
                    n1 = this.Random.Next((int)this.Config.MinNumber, subResult);
                    content = String.Format("{0} {1} {2}", content, op1, n1);
                }
                else
                {
                    // 除法在后
                    n1 = this.Random.Next(subResult, (int)this.Config.MaxNumber);
                    content = String.Format("{0} {1} {2}", n1, op1, content);
                }
            }

            // 返回
            return content;
        }

        /// <summary>
        /// 获取一组乘法数字(个位数 * 个位数/十位数/百位数)
        /// </summary>
        /// <param name="a">乘数</param>
        /// <param name="b">被乘数</param>
        private void nextMultiplyPair(ref int a, ref int b)
        {
            a = this.Random.Next(2, 10);
            b = this.Random.Next(2, 100);
            if (this.NextBoolean())
            {
                var temp = a;
                a = b;
                b = temp;
            }
        }

        /// <summary>
        /// 获取一组除法数字(个位数/十位数/百位数 ÷ 个位数)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void nextDividePair(ref int a, ref int b)
        {
            b = this.Random.Next(2, 10);
            a = this.Random.Next(b, 100);
            a = (a / b) * b;
        }

        protected override bool IsValidQuestionAnswer(int answer)
        {
            return answer >= this.Config.MinResult && answer <= this.Config.MaxResult;
        }
    }
}