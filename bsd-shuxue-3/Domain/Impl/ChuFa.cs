using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace bsd_shuxue_3.Domain.Impl
{
    /// <summary>
    /// 除法练习
    /// </summary>
    internal class ChuFa
    {
        /// <summary>
        /// 设置项目
        /// </summary>
        internal class Config : IValidatable
        {
            /// <summary>
            /// 可以使用的最大被除数
            /// </summary>
            [Category("被除数")]
            [Description("可以使用的最大被除数")]
            public uint MaxA { get; set; } = 9999;

            /// <summary>
            /// 可以使用的最小被除数
            /// </summary>
            [Category("被除数")]
            [Description("可以使用的最小被除数")]
            public uint MinA { get; set; } = 100;

            /// <summary>
            /// 可以使用的最大除数
            /// </summary>
            [Category("除数")]
            [Description("可以使用的最大除数")]
            public uint MaxB { get; set; } = 9;

            /// <summary>
            /// 可以使用的最小除数
            /// </summary>
            [Category("被除数")]
            [Description("可以使用的最小除数")]
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
            public uint MinResult { get; set; } = 2;

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
                base.Code = "ChuFa";
                base.Category = "除法";
            }

            protected override string CreateQuestionExpression()
            {
                var a = this.NextNumber(this.Config.MinA, this.Config.MaxA);
                var b = this.NextNumber(this.Config.MinB, this.Config.MaxB);
                a = (a / b) * b;
                return String.Format("{0} / {1}", a, b);
            }

            protected override bool IsValidQuestionAnswer(int answer)
            {
                return answer >= this.Config.MinResult && answer <= this.Config.MaxResult;
            }
        }
    }
}