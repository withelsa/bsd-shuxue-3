using Force.DeepCloner;
using System;
using System.Windows;

namespace bsd_shuxue_3.Domain.Impl
{
    internal class Question : IQuestion
    {
        public String Content { get; set; }

        public int Answer
        {
            get
            {
                var expression = new NCalc.Expression(this.Content);
                Func<int> func = expression.ToLambda<int>();
                return func();
            }
        }
    }

    internal abstract class QuestionFactoryBase<T> : IQuestionFactory, IConfigurable
    {
        public String Code { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        /// <summary>
        /// 配置内容
        /// </summary>
        public T Config { get; set; }

        /// <summary>
        /// 随机数生成器
        /// </summary>
        public Random Random { get; set; } = new Random();

        /// <summary>
        /// 生成下一个布尔值
        /// </summary>
        /// <returns></returns>
        protected bool NextBoolean()
        {
            return this.Random.Next(2) == 0;
        }

        public virtual IQuestion CreateInstance()
        {
            var question = new Question();
            while (true)
            {
                try
                {
                    question.Content = this.CreateQuestionContent();
                    while (!this.IsValidQuestion(question))
                    {
                        question.Content = this.CreateQuestionContent();
                    }
                    return question;
                }
                catch (Exception)
                {
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
                valid = question != null && !String.IsNullOrEmpty(question.Content) && this.IsValidQuestionAnswer(question.Answer);
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
        protected abstract bool IsValidQuestionAnswer(int answer);

        /// <summary>
        /// 生成一个问题
        /// </summary>
        /// <returns></returns>
        protected abstract String CreateQuestionContent();

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