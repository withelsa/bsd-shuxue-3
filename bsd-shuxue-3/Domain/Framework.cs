using System;
using System.Windows;

namespace bsd_shuxue_3.Domain
{
    /// <summary>
    /// 问题
    /// </summary>
    internal interface IQuestion
    {
        /// <summary>
        /// 问题内容
        /// </summary>
        String Content { get; }

        /// <summary>
        /// 问题答案
        /// </summary>
        int Answer { get; }
    }

    /// <summary>
    /// 问题生成器
    /// </summary>
    internal interface IQuestionFactory
    {
        /// <summary>
        /// 生成一个问题
        /// </summary>
        /// <returns></returns>
        IQuestion CreateInstance();

        /// <summary>
        /// 唯一标识
        /// </summary>
        String Code { get; }

        /// <summary>
        /// 简要描述
        /// </summary>
        String Title { get; }

        /// <summary>
        /// 详细描述
        /// </summary>
        String Description { get; }
    }

    /// <summary>
    /// 可配置的接口
    /// </summary>
    internal interface IConfigurable
    {
        /// <summary>
        /// 显示配置对话框
        /// </summary>
        /// <param name="owner">主窗口实例</param>
        /// <returns>是否更新了配置</returns>
        bool? DoConfig(Window owner);
    }
}