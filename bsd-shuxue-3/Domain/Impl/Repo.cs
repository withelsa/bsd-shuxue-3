using System.Collections.Generic;

namespace bsd_shuxue_3.Domain.Impl
{
    internal class QuestionFactoryRepo
    {
        private static QuestionFactoryRepo instance;

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private QuestionFactoryRepo()
        {
            // 禁止外部生成实例
        }

        static QuestionFactoryRepo()
        {
            instance = new QuestionFactoryRepo();
        }

        public static QuestionFactoryRepo Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// 所有的问题生成器的实例的列表
        /// </summary>
        public IList<IQuestionFactory> QuestionFactories { get; private set; } = new List<IQuestionFactory>();
    }
}