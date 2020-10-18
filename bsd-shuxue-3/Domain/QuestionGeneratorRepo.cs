using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bsd_shuxue_3.Domain
{
    /// <summary>
    /// IQuestionGenerator 的工具类
    /// </summary>
    class QuestionGeneratorRepo
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static QuestionGeneratorRepo Instance { get; private set; } = new QuestionGeneratorRepo();

        /// <summary>
        /// IQuestionGenerator 实例的列表
        /// </summary>
        public IList<IQuestionGenerator> QuestionGenerators { get; private set; } = new List<IQuestionGenerator>();

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private QuestionGeneratorRepo()
        {
            // 禁止外部生成实例
        }
    }
}
