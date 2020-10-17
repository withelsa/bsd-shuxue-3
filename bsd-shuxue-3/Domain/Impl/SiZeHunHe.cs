using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace bsd_shuxue_3.Domain.Impl
{
    /// <summary>
    /// 四则混合运算
    /// </summary>
    internal class SiZeHunHe
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
            public uint MaxNumber { get; set; } = 999;

            /// <summary>
            /// 可以使用的最小数字
            /// </summary>
            [Category("数字范围")]
            [Description("可以使用的最小数字")]
            public uint MinNumber { get; set; } = 1;

            /// <summary>
            /// 可以使用的最大被乘数
            /// </summary>
            [Category("数字范围")]
            [Description("可以使用的最大被乘数")]
            public uint MaxMultiplyA { get; set; } = 99;

            /// <summary>
            /// 可以使用的最小被乘数
            /// </summary>
            [Category("数字范围")]
            [Description("可以使用的最小被乘数")]
            public uint MinMultiplyA { get; set; } = 10;

            /// <summary>
            /// 可以使用的最大乘数
            /// </summary>
            [Category("数字范围")]
            [Description("可以使用的最大乘数")]
            public uint MaxMultiplyB { get; set; } = 9;

            /// <summary>
            /// 可以使用的最大乘数
            /// </summary>
            [Category("数字范围")]
            [Description("可以使用的最小乘数")]
            public uint MinMultiplyB { get; set; } = 2;

            /// <summary>
            /// 可以使用的最大被除数
            /// </summary>
            [Category("数字范围")]
            [Description("可以使用的最大被除数")]
            public uint MaxDivideA { get; set; } = 99;

            /// <summary>
            /// 可以使用的最小被除数
            /// </summary>
            [Category("数字范围")]
            [Description("可以使用的最小被除数")]
            public uint MinDivideA { get; set; } = 10;

            /// <summary>
            /// 可以使用的最大除数
            /// </summary>
            [Category("数字范围")]
            [Description("可以使用的最大除数")]
            public uint MaxDivideB { get; set; } = 9;

            /// <summary>
            /// 可以使用的最小除数
            /// </summary>
            [Category("数字范围")]
            [Description("可以使用的最小除数")]
            public uint MinDivideB { get; set; } = 2;

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
                return true;
            }
        }

        /// <summary>
        /// 问题生成器
        /// </summary>
        internal class QuestionFactory : QuestionFactoryBase<Config>
        {
            public QuestionFactory()
            {
                base.Code = "SiZeHunHe";
                base.Category = "四则混合运算";
            }

            protected override string CreateQuestionExpression()
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
                        n2 = this.NextNumber(this.Config.MinNumber, n1);
                        n3 = n1 - n2;
                        content = String.Format("({0} {1} {2}) {3} {4}", n2, op2, n3, op1, subResult);
                    }
                    else
                    {
                        // 括号用于计算 subResult
                        n2 = this.NextNumber(this.Config.MinNumber, subResult);
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
                        n2 = this.NextNumber(n1, this.Config.MaxNumber);
                        n3 = n2 - n1;
                        content = String.Format("({0} {1} {2}) {3} {4}", n2, op2, n3, op1, subResult);
                    }
                    else
                    {
                        // 括号用于计算 subResult
                        n2 = this.NextNumber(subResult, this.Config.MaxNumber);
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
                    n1 = this.NextNumber(this.Config.MinNumber, this.Config.MaxNumber);
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
                        n1 = this.NextNumber(this.Config.MinNumber, subResult);
                        content = String.Format("{0} {1} {2}", content, op1, n1);
                    }
                    else
                    {
                        // 除法在后
                        n1 = this.NextNumber(subResult, this.Config.MaxNumber);
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
                a = this.NextNumber(this.Config.MinMultiplyA, this.Config.MaxMultiplyA);
                b = this.NextNumber(this.Config.MinMultiplyB, this.Config.MaxMultiplyB);
                if (this.NextBoolean())
                {
                    // 随机交换乘数和被乘数的顺序
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
                a = this.NextNumber(this.Config.MinDivideA, this.Config.MaxDivideA);
                b = this.NextNumber(this.Config.MinDivideB, this.Config.MaxDivideB);
                a = (a / b) * b;
            }

            protected override bool IsValidQuestionAnswer(int answer)
            {
                return answer >= this.Config.MinResult && answer <= this.Config.MaxResult;
            }
        }
    }
}