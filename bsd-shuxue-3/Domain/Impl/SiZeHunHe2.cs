using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace bsd_shuxue_3.Domain.Impl
{
    /// <summary>
    /// 四则混合运算(2)
    /// </summary>
    internal class SiZeHunHe2
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

            /// <summary>
            /// 最小加减次数
            /// </summary>
            [Category("混合计算")]
            [Description("最小加减次数")]
            public uint MinPlusMinusSteps { get; set; } = 1;

            /// <summary>
            /// 最大加减次数
            /// </summary>
            [Category("混合计算")]
            [Description("最大加减次数")]
            public uint MaxPlusMinusSteps { get; set; } = 1;

            /// <summary>
            /// 乘除比例
            /// </summary>
            [Category("混合计算")]
            [Description("乘除比例")]
            public uint MultiplyDivideRatio { get; set; } = 80;

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
            public int MaxRetry { get; set; } = 10;

            public QuestionFactory()
            {
                base.Code = "SiZeHunHe2";
                base.Category = "四则混合运算（2）";
            }

            protected override string CreateQuestionExpression()
            {
                var sb = new StringBuilder();
                var result = 0;

                // 判定加减计算的次数
                var stepCount = 1 + this.NextNumber(this.Config.MinPlusMinusSteps, this.Config.MaxPlusMinusSteps);

                // 生成各加减计算步骤的值
                for (var step = 0; step < stepCount; step++)
                {
                    this.nextStep(step, ref sb, ref result);
                }

                // 检查是否包含乘法或者除法
                var content = sb.ToString();
                if (content.IndexOf('*') < 0 && content.IndexOf('/') < 0)
                {
                    throw new InvalidQuestionException();
                }

                // 返回
                return content;
            }

            protected void nextStep(int step, ref StringBuilder sb, ref int result)
            {
                var isFirstStep = step < 1; // 是否是第一个步骤
                var usePlusOp = isFirstStep || this.NextBoolean(); // 本步骤是否使用加法

                // 判定是使用乘除法，还是直接使用数字
                var maxNumber = usePlusOp ? this.Config.MaxNumber : Math.Min(this.Config.MaxNumber, result);
                var stepValue = 0;
                var stepExpression = "";
                for (var retryIndex = 0; retryIndex < this.MaxRetry; retryIndex++)
                {
                    // 尝试生成一组数字
                    if (this.Config.MultiplyDivideRatio >= this.NextPercentage())
                    {
                        // 使用乘除法
                        var a = 0;
                        var b = 0;
                        if (this.NextBoolean())
                        {
                            // 使用乘法
                            this.nextMultiplyPair(ref a, ref b);
                            stepValue = a * b;
                            stepExpression = String.Format("{0} * {1}", a, b);
                        }
                        else
                        {
                            // 使用除法
                            this.nextDividePair(ref a, ref b);
                            if (a <= b)
                            {
                                stepValue = -1;
                                continue;
                            }
                            stepValue = a / b;
                            stepExpression = String.Format("{0} / {1}", a, b);
                        }
                    }
                    else
                    {
                        // 直接使用数字
                        stepValue = this.NextNumber(this.Config.MinNumber, maxNumber);
                        stepExpression = stepValue.ToString();
                    }

                    // 检查本组数字是否合规
                    if (usePlusOp)
                    {
                        if (result + stepValue <= this.Config.MaxResult)
                        {
                            // 如果是加法，那么校验和是否在最大范围之内
                            break;
                        }
                    }
                    else
                    {
                        if (result >= stepValue)
                        {
                            // 如果是减法，那么校验差是否大于零
                            break;
                        }
                    }
                }

                // 更新 StringBuilder 和 result
                if (stepValue < 0)
                {
                    throw new InvalidQuestionException();
                }
                if (usePlusOp)
                {
                    // 按照加法更新
                    if (isFirstStep)
                    {
                        sb.Append(stepExpression);
                    }
                    else
                    {
                        sb.AppendFormat(" + {0}", stepExpression);
                    }
                    result += stepValue;
                }
                else
                {
                    if (result < stepValue)
                    {
                        throw new InvalidQuestionException();
                    }

                    // 按照减法更新
                    sb.AppendFormat(" - {0}", stepExpression);
                    result -= stepValue;
                }
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
                //if (this.NextBoolean())
                //{
                //    // 随机交换乘数和被乘数的顺序
                //    var temp = a;
                //    a = b;
                //    b = temp;
                //}
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

            protected override bool IsValidQuestionAnswer(Decimal answer)
            {
                return answer >= this.Config.MinResult && answer <= this.Config.MaxResult;
            }
        }
    }
}