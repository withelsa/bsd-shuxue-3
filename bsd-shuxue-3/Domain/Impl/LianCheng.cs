using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace bsd_shuxue_3.Domain.Impl
{
    /// <summary>
    /// 连续乘法
    /// </summary>
    internal class LianCheng
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
            /// 最小连成次数
            /// </summary>
            [Category("连成次数")]
            [Description("最小连成次数")]
            public uint MinSteps { get; set; } = 2;

            /// <summary>
            /// 最大连成次数
            /// </summary>
            [Category("连成次数")]
            [Description("最大连成次数")]
            public uint MaxSteps { get; set; } = 2;

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
                base.Code = "LianCheng";
                base.Category = "连乘";
            }

            protected override string CreateQuestionExpression()
            {
                LinkedList<String> numbers = new LinkedList<string>();

                // 计算被乘数
                var a = this.NextNumber(this.Config.MinA, this.Config.MaxA);
                if (a % 10 == 0)
                {
                    // 不能出现 10 结尾的被乘数（太简单）
                    throw new InvalidQuestionException();
                }
                numbers.AddLast(a.ToString());

                // 计算连乘次数
                var stepCount = this.NextNumber(this.Config.MinSteps, this.Config.MaxSteps);
                for (var step = 0; step < stepCount; step++)
                {
                    var b = this.NextNumber(this.Config.MinB, this.Config.MaxB);
                    numbers.AddLast(b.ToString());
                }

                // 校验数字是否合规
                // 不能既包含 5 又包含 2 （太简单）
                if (numbers.Contains("5") && numbers.Contains("2"))
                {
                    throw new InvalidQuestionException();
                }

                // 返回
                return String.Join(" * ", numbers);
            }

            protected override bool IsValidQuestionAnswer(Decimal answer)
            {
                return answer >= this.Config.MinResult && answer <= this.Config.MaxResult;
            }
        }
    }
}