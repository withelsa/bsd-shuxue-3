using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace bsd_shuxue_3.Domain.Impl
{
    /// <summary>
    /// 连加连减练习
    /// </summary>
    internal class LianJiaLianJian
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
            [Description("可以使用的最大数字")]
            public uint MaxNumber { get; set; } = 9999;

            /// <summary>
            /// 可以使用的最小数字
            /// </summary>
            [Category("数字范围")]
            [Description("可以使用的最小数字")]
            public uint MinNumber { get; set; } = 100;

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
            public uint MinSteps { get; set; } = 2;

            /// <summary>
            /// 使用括号的概率
            /// </summary>
            [Category("其他")]
            [Description("最大加减次数")]
            public uint MaxSteps { get; set; } = 2;

            /// <summary>
            /// 进位退位的比例(0 的时候不特别计算进位和退位)
            /// </summary>
            [Category("其他")]
            [Description("进位退位的比例")]
            public uint JinTuiWeiRatio { get; set; } = 0;

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
        /// 问题生成器
        /// </summary>
        internal class QuestionFactory : QuestionFactoryBase<Config>
        {
            public int MaxRetry { get; set; } = 10;

            public QuestionFactory()
            {
                base.Code = "LianJiaLianJian";
                base.Category = "连加连减";
            }

            protected override string CreateQuestionExpression()
            {
                // 计算加减操作次数
                var steps = this.NextNumber(this.Config.MinSteps, this.Config.MaxSteps);

                // 生成第一个数
                var sb = new StringBuilder();
                HashSet<int> usedNumbers = new HashSet<int>();
                int firstNum = this.nextNumber(usedNumbers);
                int result = firstNum;
                sb.Append(firstNum.ToString());

                // 补全连加连减操作，中途避免出现负数
                for (var i = 0; i < steps; i++)
                {
                    var plus = this.NextBoolean();
                    if (plus)
                    {
                        // 添加加法运算
                        var num = this.nextPlusNumber(result, usedNumbers);
                        result += num;
                        sb.Append(String.Format(" + {0}", num));
                    }
                    else
                    {
                        // 添加减法运算
                        var num = this.nextMinusNumber(result, usedNumbers);
                        result -= num;
                        sb.Append(String.Format(" - {0}", num));
                    }
                }

                // 返回
                return sb.ToString();
            }

            /// <summary>
            /// 计算下一个减数
            /// </summary>
            /// <param name="result"></param>
            /// <param name="usedNumbers"></param>
            /// <returns></returns>
            private int nextMinusNumber(int result, HashSet<int> usedNumbers)
            {
                // 扔骰子判定是否需要退位
                var tuiwei = result > 10 &&
                    (this.Config.JinTuiWeiRatio < 1 || this.Config.JinTuiWeiRatio >= this.NextPercentage());

                // 生成一个加数
                var num = this.NextNumber(this.Config.MinNumber, Math.Min(result, this.Config.MaxNumber));
                for (var i = 0; i < this.MaxRetry; i++)
                {
                    // 判断是否符合进位条件
                    // 符合条件的话返回，否则重试
                    if (tuiwei && !this.IsTuiWei(result, num))
                    {
                        num = this.NextNumber(this.Config.MinNumber, Math.Min(result, this.Config.MaxNumber));
                    }
                    else
                    {
                        break;
                    }
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
            private int nextPlusNumber(int result, HashSet<int> usedNumbers)
            {
                // 扔骰子判定是否需要进位
                var jinwei = result > 10 &&
                    (this.Config.JinTuiWeiRatio < 1 || this.Config.JinTuiWeiRatio >= this.NextPercentage());

                // 生成一个加数
                var num = this.nextNumber(usedNumbers);
                for (var i = 0; i < this.MaxRetry; i++)
                {
                    // 判断是否符合进位条件
                    // 符合条件的话返回，否则重试
                    if (jinwei && !this.IsJinWei(result, num))
                    {
                        num = this.nextNumber(usedNumbers);
                    }
                    else
                    {
                        break;
                    }
                }

                // 返回
                return num;
            }

            /// <summary>
            /// 生成一个数字
            /// </summary>
            /// <returns></returns>
            private int nextNumber(HashSet<int> usedNumbers)
            {
                return this.NextNumber(this.Config.MinNumber, this.Config.MaxNumber, usedNumbers);
            }

            protected override bool IsValidQuestionAnswer(Decimal answer)
            {
                return answer >= this.Config.MinResult && answer <= this.Config.MaxResult;
            }
        }
    }
}