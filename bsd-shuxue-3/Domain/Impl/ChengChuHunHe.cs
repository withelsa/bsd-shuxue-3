using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace bsd_shuxue_3.Domain.Impl
{
    /// <summary>
    /// 乘除混合
    /// </summary>
    internal class ChengChuHunHe
    {
        /// <summary>
        /// 配置项目
        /// </summary>
        internal class Config : IValidatable
        {
            /// <summary>
            /// 可以使用的最大被乘数
            /// </summary>
            [Category("被乘数")]
            [Description("可以使用的最大被乘数")]
            public uint MaxA { get; set; } = 99;

            /// <summary>
            /// 可以使用的最小被乘数
            /// </summary>
            [Category("被乘数")]
            [Description("可以使用的最小被乘数")]
            public uint MinA { get; set; } = 10;

            /// <summary>
            /// 可以使用的最大乘数
            /// </summary>
            [Category("乘数")]
            [Description("可以使用的最大乘数")]
            public uint MaxB { get; set; } = 9;

            /// <summary>
            /// 可以使用的最小乘数
            /// </summary>
            [Category("被乘数")]
            [Description("可以使用的最小乘数")]
            public uint MinB { get; set; } = 2;

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
            public uint MinResult { get; set; } = 10;

            /// <summary>
            /// 先除后乘的比例
            /// </summary>
            [Category("其他")]
            [Description("先除后乘的比例")]
            public uint DivideFirstRatio { get; set; } = 85;

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
                base.Code = "ChengChuHunHe";
                base.Category = "乘除混合";
            }

            protected override string CreateQuestionExpression()
            {
                var content = "";

                // 计算被乘数
                var a = this.NextNumber(this.Config.MinA, this.Config.MaxA);
                var b = this.NextNumber(this.Config.MinB, this.Config.MaxB); // 乘数
                var c = this.NextNumber(this.Config.MinB, this.Config.MaxB); // 除数

                // 计算乘数和除数
                if (this.Config.DivideFirstRatio >= this.NextPercentage())
                {
                    // 先除后乘
                    a = (a / c) * c;
                    content = String.Format("{0} / {1} * {2}", a, c, b);
                }
                else
                {
                    // 先乘后除
                    content = String.Format("{0} * {1} / {2}", a, b, c);
                }

                // 判定是否合规
                if (b == c || (a * b % c != 0))
                {
                    throw new InvalidQuestionException();
                }

                // 返回结果
                return content;
            }

            protected override bool IsValidQuestionAnswer(Decimal answer)
            {
                return answer >= this.Config.MinResult && answer <= this.Config.MaxResult;
            }
        }
    }
}