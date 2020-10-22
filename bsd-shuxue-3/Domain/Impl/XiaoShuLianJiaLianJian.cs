using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace bsd_shuxue_3.Domain.Impl
{
    /// <summary>
    /// 小数的连加连减练习
    /// </summary>
    internal class XiaoShuLianJiaLianJian
    {
        /// <summary>
        /// 设置项目
        /// </summary>
        internal class Config : IValidatable
        {
            /// <summary>
            /// 可以使用的最大数字
            /// </summary>
            [Category("数字范围")]
            [Description("可以使用的最大整数")]
            public uint MaxNumber { get; set; } = 9999;

            /// <summary>
            /// 可以使用的最小数字
            /// </summary>
            [Category("数字范围")]
            [Description("可以使用的最小整数")]
            public uint MinNumber { get; set; } = 100;

            /// <summary>
            /// 可以使用的最小数字
            /// </summary>
            [Category("数字范围")]
            [Description("可以使用的小数位数")]
            public uint Decimals { get; set; } = 1;

            /// <summary>
            /// 最大结果
            /// </summary>
            [Category("结果范围")]
            [Description("最大结果")]
            public uint MaxResult { get; set; } = 10000;

            /// <summary>
            /// 最小结果
            /// </summary>
            [Category("结果范围")]
            [Description("最小结果")]
            public uint MinResult { get; set; } = 1;

            /// <summary>
            /// 使用括号的概率
            /// </summary>
            [Category("其他")]
            [Description("最少加减次数")]
            public uint MinSteps { get; set; } = 1;

            /// <summary>
            /// 使用括号的概率
            /// </summary>
            [Category("其他")]
            [Description("最大加减次数")]
            public uint MaxSteps { get; set; } = 1;

            /// <summary>
            /// 小数部分的进位退位比例(0 的时候不特别计算进位和退位)
            /// </summary>
            [Category("其他")]
            [Description("小数部分的进位退位比例")]
            public uint XiaoShuJinTuiWeiRatio { get; set; } = 50;

            /// <summary>
            /// 整数部分的进位退位比例(0 的时候不特别计算进位和退位)
            /// </summary>
            [Category("其他")]
            [Description("整数部分的进位退位比例")]
            public uint ZhengShuJinTuiWeiRatio { get; set; } = 50;

            public bool Validate(IList<string> messages)
            {
                return true;
            }
        }

        /// <summary>
        /// 问题生成器
        /// </summary>
        internal class QuestionFactory : QuestionFactoryBase<Config>
        {
            public int MaxRetry { get; set; } = 100;

            public QuestionFactory()
            {
                base.Code = "XiaoShuLianJiaLianJian";
                base.Category = "小数连加连减";
            }

            protected override string CreateQuestionExpression()
            {
                // 计算加减操作次数
                var steps = this.NextNumber(this.Config.MinSteps, this.Config.MaxSteps);

                // 计算小数位数
                var xiaoShuMax = 1;
                for (var i = 0; i < this.Config.Decimals; i++)
                {
                    xiaoShuMax *= 10;
                }

                // 生成第一个数
                var sb = new StringBuilder();
                HashSet<int> usedNumbers = new HashSet<int>();
                int firstNum = this.nextNumber(xiaoShuMax, usedNumbers);
                var format = "{0}.{1:d" + this.Config.Decimals.ToString() + "}";
                int result = firstNum;
                this.appendDecimal(sb, firstNum, xiaoShuMax, format);

                // 补全连加连减操作，中途避免出现负数
                for (var i = 0; i < steps; i++)
                {
                    var plus = this.NextBoolean();
                    if (plus)
                    {
                        // 添加加法运算
                        var num = this.nextPlusNumber(result, xiaoShuMax, usedNumbers);
                        result += num;
                        sb.Append(" + ");
                        this.appendDecimal(sb, num, xiaoShuMax, format);
                    }
                    else
                    {
                        // 添加减法运算
                        var num = this.nextMinusNumber(result, xiaoShuMax, usedNumbers);
                        sb.Append(" - ");
                        this.appendDecimal(sb, num, xiaoShuMax, format);
                    }
                }

                // 换算成小数
                return sb.ToString();
            }

            private void appendDecimal(StringBuilder sb, int number, int xiaoShuMax, String format)
            {
                sb.AppendFormat(format, number / xiaoShuMax, number % xiaoShuMax);
            }

            /// <summary>
            /// 计算下一个减数
            /// </summary>
            /// <param name="result"></param>
            /// <param name="usedNumbers"></param>
            /// <returns></returns>
            private int nextMinusNumber(int result, int xiaoShuMax, HashSet<int> usedNumbers)
            {
                var zhengShu = result / xiaoShuMax;
                var xiaoShu = result % xiaoShuMax;
                var zhengShuJinWei = this.Config.ZhengShuJinTuiWeiRatio >= this.NextPercentage();
                var xiaoShuJinWei = this.Config.XiaoShuJinTuiWeiRatio >= this.NextPercentage();

                // 生成一个加数
                var num = this.nextNumber(xiaoShuMax, usedNumbers, result);
                for (var i = 0; i < this.MaxRetry; i++)
                {
                    num = this.nextNumber(xiaoShuMax, usedNumbers);

                    // 判断是否符合整数进位条件
                    if (zhengShuJinWei && !this.IsTuiWei(zhengShu, num / xiaoShuMax))
                    {
                        continue;
                    }
                    // 判断是否小数进位条件
                    if (xiaoShuJinWei && !this.IsTuiWei(xiaoShu, num % xiaoShuMax))
                    {
                        continue;
                    }
                    // 都符合条件的话，使用本加数
                    break;
                }

                // 返回
                return num;
            }

            /// <summary>
            /// 计算下一个加数
            /// </summary>
            /// <param name="result"></param>
            /// <param name="usedNumbers"></param>
            /// <returns></returns>
            private int nextPlusNumber(int result, int xiaoShuMax, HashSet<int> usedNumbers)
            {
                var zhengShu = result / xiaoShuMax;
                var xiaoShu = result % xiaoShuMax;
                var zhengShuJinWei = this.Config.ZhengShuJinTuiWeiRatio >= this.NextPercentage();
                var xiaoShuJinWei = this.Config.XiaoShuJinTuiWeiRatio >= this.NextPercentage();

                // 生成一个加数
                var num = this.nextNumber(xiaoShuMax, usedNumbers);
                for (var i = 0; i < this.MaxRetry; i++)
                {
                    num = this.nextNumber(xiaoShuMax, usedNumbers);

                    // 判断是否符合整数进位条件
                    if (zhengShuJinWei && !this.IsJinWei(zhengShu, num / xiaoShuMax))
                    {
                        continue;
                    }
                    // 判断是否小数进位条件
                    if (xiaoShuJinWei && !this.IsJinWei(xiaoShu, num % xiaoShuMax))
                    {
                        continue;
                    }
                    // 都符合条件的话，使用本加数
                    break;
                }

                // 返回
                return num;
            }

            /// <summary>
            /// 生成一个数字
            /// </summary>
            /// <returns></returns>
            private int nextNumber(int xiaoShuMax, HashSet<int> usedNumbers, int maxValue = int.MaxValue)
            {
                var i = this.NextNumber(this.Config.MinNumber, Math.Min(maxValue, this.Config.MaxNumber), usedNumbers); // 整数部分
                var f = this.NextNumber(1, xiaoShuMax); // 小数部分
                return i * xiaoShuMax + f;
            }

            protected override bool IsValidQuestionAnswer(Decimal answer)
            {
                return answer >= this.Config.MinResult && answer <= this.Config.MaxResult;
            }
        }
    }
}