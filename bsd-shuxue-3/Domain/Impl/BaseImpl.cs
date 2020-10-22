using Force.DeepCloner;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace bsd_shuxue_3.Domain.Impl
{
    internal class Question : IQuestion
    {
        /// <summary>
        /// 显示的问题
        /// </summary>
        private String content;

        /// <summary>
        /// 实际的计算表达式
        /// </summary>
        private String expresion;

        public String Content
        {
            get
            {
                return this.content;
            }
        }

        public String Expression
        {
            get
            {
                return this.expresion;
            }
            set
            {
                this.expresion = value;
                this.updateContent();
            }
        }

        protected virtual void updateContent()
        {
            this.content = String.IsNullOrEmpty(this.expresion) ? this.expresion :
                this.expresion
                .Replace(' ', ' ')
                .Replace('+', '＋')
                .Replace('-', '－')
                .Replace('*', '×')
                .Replace('/', '÷');
        }

        public Decimal Answer
        {
            get
            {
                var expression = new NCalc.Expression(this.Expression);
                Func<Decimal> func = expression.ToLambda<Decimal>();
                return func();
            }
        }

        public IQuestionFactory Factory { get; set; }
    }

    internal abstract class QuestionFactoryBase<T> : IQuestionFactory, IConfigurable
        where T : new()
    {
        /// <summary>
        /// 加号
        /// </summary>
        public const String OP_PLUS = "+";

        /// <summary>
        /// 减号
        /// </summary>
        public const String OP_MINUS = "-";

        /// <summary>
        /// 乘号
        /// </summary>
        public const String OP_MULTPLY = "*";

        /// <summary>
        /// 除号
        /// </summary>
        public const String OP_DIVIDE = "/";

        public String Category { get; set; }

        public String Code { get; set; }

        public String Title { get; set; }

        private string description;

        public String Description
        {
            get
            {
                return String.IsNullOrEmpty(this.description) ? this.Title : this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <summary>
        /// 配置内容
        /// </summary>
        public T Config { get; set; } = new T();

        /// <summary>
        /// 随机数生成器
        /// </summary>
        protected Random Random { get; } = new Random();

        /// <summary>
        /// 是否可以配置
        /// </summary>
        public bool CanConfig { get; set; } = true;

        /// <summary>
        /// 难度
        /// </summary>
        public DifficultyLevel Level { get; set; } = DifficultyLevel.Normal;

        /// <summary>
        /// 生成下一个布尔值
        /// </summary>
        /// <returns></returns>
        protected bool NextBoolean()
        {
            return this.Random.Next(2) == 0;
        }

        /// <summary>
        /// 生成下一个数值
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        protected int NextNumber(object minValue, object maxValue)
        {
            var min = Convert.ToInt32(minValue);
            var max = Convert.ToInt32(maxValue);
            return max > min ? this.Random.Next(min, max + 1) :
                this.Random.Next(max, min + 1);
        }

        /// <summary>
        /// 生成下一个数值（不重复）
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="usedNumbers"></param>
        /// <returns></returns>
        protected int NextNumber(object min, object max, HashSet<int> usedNumbers)
        {
            while (true)
            {
                var number = this.NextNumber(min, max);
                if (usedNumbers != null && usedNumbers.Contains(number))
                {
                    continue;
                }
                return number;
            }
        }

        protected int NextPercentage()
        {
            return this.Random.Next(0, 101);
        }

        /// <summary>
        /// 判断是否是进位加法
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected bool IsJinWei(int a, int b)
        {
            while (true)
            {
                // 判断最后一位数
                var lastA = a % 10;
                var lastB = b % 10;
                if (lastA + lastB >= 10)
                {
                    // 如果最后一位数的和大于等于 10，那么发生进位
                    return true;
                }

                // 否则判断前一位数
                a = a / 10;
                b = b / 10;
                if (a < 1 || b < 1)
                {
                    // 如果 a 或者 b 中有一个已经算到了第一位数，那么退出计算
                    return false;
                }
            }
        }

        /// <summary>
        /// 判断是否是退位减法
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected bool IsTuiWei(int a, int b)
        {
            while (true)
            {
                // 判断最后一位数
                var lastA = a % 10;
                var lastB = b % 10;
                if (lastA < lastB)
                {
                    // 如果被减数的最后一位数小于减数的最后一位数，那么发生退位
                    return true;
                }

                // 否则判断前一位数
                a = a / 10;
                b = b / 10;
                if (a < 1 || b < 1)
                {
                    // 如果 a 或者 b 中有一个已经算到了第一位数，那么退出计算
                    return false;
                }
            }
        }

        public virtual IQuestion CreateInstance()
        {
            var question = new Question();
            question.Factory = this;
            while (true)
            {
                try
                {
                    question.Expression = this.CreateQuestionExpression();
                    while (!this.IsValidQuestion(question))
                    {
                        question.Expression = this.CreateQuestionExpression();
                    }
                    return question;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("创建问题时发生错误:\r\n{0}", ex);
                }
            }
        }

        /// <summary>
        /// 判断生成的问题是否有效
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        protected bool IsValidQuestion(Question question)
        {
            var valid = false;
            try
            {
                valid = question != null && !String.IsNullOrEmpty(question.Expression) && this.IsValidQuestionAnswer(question.Answer);
            }
            catch (Exception)
            {
            }
            return valid;
        }

        /// <summary>
        /// 判断生成的问题的结果是否有效
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        protected abstract bool IsValidQuestionAnswer(Decimal answer);

        /// <summary>
        /// 生成一个问题
        /// </summary>
        /// <returns></returns>
        protected abstract String CreateQuestionExpression();

        public virtual bool? DoConfig(Window owner)
        {
            var propertyWindow = new PropertyGridWindow();
            propertyWindow.Owner = owner;
            propertyWindow.Title = String.Format("{0}的配置:", this.Title);
            T config = this.cloneConfig();
            propertyWindow.SelectedObject = config;
            var ret = propertyWindow.ShowDialog();
            if (ret == true)
            {
                this.Config = config;
            }
            return ret;
        }

        protected virtual T cloneConfig()
        {
            return this.Config != null ? this.Config.DeepClone() : default(T);
        }
    }

    /// <summary>
    /// 代表无效问题的异常
    /// </summary>
    internal class InvalidQuestionException : Exception
    {
        public InvalidQuestionException()
        {
        }

        public InvalidQuestionException(String message) : base(message)
        {
        }

        public InvalidQuestionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}