using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bsd_shuxue_3.Domain
{
    /// <summary>
    /// 考试题
    /// </summary>
    interface IQuestion
    {
        /// <summary>
        /// 生成器实例
        /// </summary>
        IQuestionGenerator Generator { get; }

        /// <summary>
        /// 题目内容
        /// </summary>
        String Content { get; }

        /// <summary>
        /// 正确答案
        /// </summary>
        String Answer { get; }
    }
}
