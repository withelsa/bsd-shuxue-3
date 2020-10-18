using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bsd_shuxue_3.Domain
{
    /// <summary>
    /// 考试题生成器
    /// </summary>
    interface IQuestionGenerator
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        String Id { get; }

        /// <summary>
        /// 标题（简要描述）
        /// </summary>
        String Title { get; }

        /// <summary>
        /// 完整描述
        /// </summary>
        String Description { get; }

        /// <summary>
        /// 生成一个考试题
        /// </summary>
        /// <returns></returns>
        IQuestion Generate();
    }
}
