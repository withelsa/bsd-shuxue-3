using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace bsd_shuxue_3.Domain.Impl
{
    /// <summary>
    /// 加减混合运算的设置
    /// </summary>
    internal class JiaJianHunHeConfig
    {
        /// <summary>
        /// 可以使用的最大数字
        /// </summary>
        [Description("可以使用的最大数字")]
        public int MaxNumber { get; set; } = 999;

        /// <summary>
        /// 可以使用的最小数字
        /// </summary>
        [Description("可以使用的最小数字")]
        public int MinNumber { get; set; } = 100;

        /// <summary>
        /// 最大结果
        /// </summary>
        [Description("最大结果")]
        public int MaxResult { get; set; } = 1000;

        /// <summary>
        /// 最小结果
        /// </summary>
        [Description("最小结果")]
        public int MinResult { get; set; } = 1;

        /// <summary>
        /// 使用括号的概率
        /// </summary>
        [Description("使用括号的概率")]
        public int BracketPropability = 60;
    }

    /// <summary>
    /// 加减混合运算问题的生成器
    /// </summary>
    internal class JiaJianHunHeQuestionFactory : QuestionFactoryBase<JiaJianHunHeConfig>
    {
        /// <summary>
        /// 加号
        /// </summary>
        public const String OP_PLUS = "+";

        /// <summary>
        /// 减号
        /// </summary>
        public const String OP_MINUS = "-";

        public JiaJianHunHeQuestionFactory()
        {
            base.Code = "JiaJianHunHe";
            base.Title = "加减混合运算";
            base.Description = base.Title;
            base.Config = new JiaJianHunHeConfig();
        }

        protected override string CreateQuestionContent()
        {
            String content = null;

            // 生成三个数
            HashSet<int> numbersUsed = new HashSet<int>();
            int n1 = this.nextNumber(numbersUsed);
            int n2 = this.nextNumber(numbersUsed);
            int n3 = this.nextNumber(numbersUsed);

            // 生成两个符号
            String op1 = nextOperator();
            String op2 = nextOperator();

            // 判断是否使用括号
            if (this.useBracket())
            {
                // 决定使用括号
                if (this.NextBoolean())
                {
                    // 括号加在 n1 和 n2
                    if (op1 == OP_MINUS && n1 < n2)
                    {
                        throw new InvalidQuestionException("被减数小于减数");
                    }
                    content = String.Format("({0} {1} {2}) {3} {4}", n1, op1, n2, op2, n3);
                }
                else
                {
                    // 括号加在 n2 和 n3
                    if (op2 == OP_MINUS && n2 < n3)
                    {
                        throw new InvalidQuestionException("被减数小于减数");
                    }
                    content = String.Format("{0} {1} ({2} {3} {4})", n1, op1, n2, op2, n3);
                }
            }
            else
            {
                // 不使用括号
                if (op1 == OP_MINUS && n1 < n2)
                {
                    throw new InvalidQuestionException("被减数小于减数");
                }
                content = String.Format("{0} {1} {2} {3} {4}", n1, op1, n2, op2, n3);
            }

            // 返回
            return content;
        }

        /// <summary>
        /// 判断是否使用括号
        /// </summary>
        /// <returns></returns>
        private bool useBracket()
        {
            return this.Random.Next(0, 100) >= this.Config.BracketPropability;
        }

        /// <summary>
        /// 生成一个运算符
        /// </summary>
        /// <returns></returns>
        private String nextOperator()
        {
            return base.NextBoolean() ? OP_PLUS : OP_MINUS;
        }

        /// <summary>
        /// 生成一个数字
        /// </summary>
        /// <returns></returns>
        private int nextNumber(HashSet<int> numbersUsed)
        {
            while (true)
            {
                int number = this.Random.Next(this.Config.MinNumber, this.Config.MaxNumber + 1);
                if (!numbersUsed.Contains(number))
                {
                    numbersUsed.Add(number);
                    return number;
                }
            }
        }

        protected override bool IsValidQuestionAnswer(int answer)
        {
            return answer >= this.Config.MinResult && answer <= this.Config.MaxResult;
        }
    }
}