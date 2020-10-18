using bsd_shuxue_3.Domain.Impl;
using System.Collections.Generic;

namespace bsd_shuxue_3.Domain
{
    internal class QuestionFactoriesInitializer
    {
        /// <summary>
        /// 初始化问题生成器列表
        /// </summary>
        /// <param name="collection"></param>
        public void Initialize(ICollection<IQuestionFactory> collection)
        {
            lianJiaLianJian(collection);
            jiaJianHunHe(collection);
            siZeHunHe(collection);
            chengFa(collection);
            chuFa(collection);
        }

        private void lianJiaLianJian(ICollection<IQuestionFactory> collection)
        {
            // 【连加连减】三位数之内的单次加减
            {
                var questionFactory = new LianJiaLianJian.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数之内的单次加减";
                questionFactory.Level = DifficultyLevel.Easy;
                var config = questionFactory.Config;
                config.MinNumber = 100;
                config.MaxNumber = 999;
                config.MinSteps = config.MaxSteps = 1;
            }
            // 【连加连减】三位数之内的单次加减（较高概率进退位）
            {
                var questionFactory = new LianJiaLianJian.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数之内的单次加减（较高概率进退位）";
                questionFactory.Level = DifficultyLevel.Normal;
                questionFactory.MaxRetry = 10;
                var config = questionFactory.Config;
                config.MinNumber = 100;
                config.MaxNumber = 999;
                config.MinSteps = config.MaxSteps = 1;
                config.JinTuiWeiRatio = 67;
            }
            // 【连加连减】三位数之内的单次加减（极高概率进退位）
            {
                var questionFactory = new LianJiaLianJian.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数之内的单次加减（极高概率进退位）";
                questionFactory.Level = DifficultyLevel.Normal;
                questionFactory.MaxRetry = 100;
                var config = questionFactory.Config;
                config.MinNumber = 100;
                config.MaxNumber = 999;
                config.MinSteps = config.MaxSteps = 1;
                config.JinTuiWeiRatio = 95;
            }
            // 【连加连减】三位数之内的两次连续加减
            {
                var questionFactory = new LianJiaLianJian.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数之内的两次连续加减";
                var config = questionFactory.Config;
                config.MinNumber = 100;
                config.MaxNumber = 999;
                config.MinSteps = config.MaxSteps = 2;
            }
            // 【连加连减】三位数之内的两次连续加减（较高概率进退位）
            {
                var questionFactory = new LianJiaLianJian.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数之内的两次连续加减（较高概率进退位）";
                questionFactory.Level = DifficultyLevel.Hard;
                questionFactory.MaxRetry = 10;
                var config = questionFactory.Config;
                config.MinNumber = 100;
                config.MaxNumber = 999;
                config.MinSteps = config.MaxSteps = 2;
                config.JinTuiWeiRatio = 67;
            }
            // 【连加连减】三位数之内的两次连续加减（极高概率进退位）
            {
                var questionFactory = new LianJiaLianJian.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数之内的两次连续加减（极高概率进退位）";
                questionFactory.Level = DifficultyLevel.Hard;
                questionFactory.MaxRetry = 100;
                var config = questionFactory.Config;
                config.MinNumber = 100;
                config.MaxNumber = 999;
                config.MinSteps = config.MaxSteps = 2;
                config.JinTuiWeiRatio = 95;
            }
            // 【连加连减】四位数之内的两次连续加减
            {
                var questionFactory = new LianJiaLianJian.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "四位数之内的两次连续加减";
                var config = questionFactory.Config;
                config.MinNumber = 100;
                config.MaxNumber = 9999;
                config.MaxResult = 20000;
                config.MinSteps = config.MaxSteps = 2;
            }
            // 【连加连减】四位数之内的两次连续加减（较高概率进退位）
            {
                var questionFactory = new LianJiaLianJian.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "四位数之内的两次连续加减（较高概率进退位）";
                questionFactory.Level = DifficultyLevel.Hard;
                questionFactory.MaxRetry = 10;
                var config = questionFactory.Config;
                config.MinNumber = 100;
                config.MaxNumber = 9999;
                config.MaxResult = 20000;
                config.MinSteps = config.MaxSteps = 2;
                config.JinTuiWeiRatio = 67;
            }
            // 【连加连减】四位数之内的两次连续加减（极高概率进退位）
            {
                var questionFactory = new LianJiaLianJian.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "四位数之内的两次连续加减（极高概率进退位）";
                questionFactory.Level = DifficultyLevel.Hard;
                questionFactory.MaxRetry = 100;
                var config = questionFactory.Config;
                config.MinNumber = 100;
                config.MaxNumber = 9999;
                config.MaxResult = 20000;
                config.MinSteps = config.MaxSteps = 2;
                config.JinTuiWeiRatio = 95;
            }
            // 【连加连减】四位数之内的 2 - 3 次连续加减
            {
                var questionFactory = new LianJiaLianJian.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "四位数之内的 2 - 3 次连续加减";
                questionFactory.Level = DifficultyLevel.Hard;
                var config = questionFactory.Config;
                config.MinNumber = 100;
                config.MaxNumber = 9999;
                config.MaxResult = 99999;
                config.MinSteps = 2;
                config.MaxSteps = 3;
            }
            // 【连加连减】四位数之内的 2 - 3 次连续加减（极高概率进退位）
            {
                var questionFactory = new LianJiaLianJian.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "四位数之内的 2 - 3 次连续加减（极高概率进退位）";
                questionFactory.Level = DifficultyLevel.VeryHard;
                questionFactory.MaxRetry = 100;
                var config = questionFactory.Config;
                config.MinNumber = 100;
                config.MaxNumber = 9999;
                config.MaxResult = 99999;
                config.MinSteps = 2;
                config.MaxSteps = 3;
                config.JinTuiWeiRatio = 95;
            }
        }

        private void jiaJianHunHe(ICollection<IQuestionFactory> collection)
        {
            // 【加减混合】三位数之内的加减混合
            {
                var questionFactory = new JiaJianHunHe.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数之内的加减混合";
                var config = questionFactory.Config;
                config.MinNumber = 100;
                config.MaxNumber = 999;
                config.MinResult = 100;
                config.MaxResult = 9999;
            }
            // 【加减混合】三位数之内的加减混合（括号强化）
            {
                var questionFactory = new JiaJianHunHe.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数之内的加减混合（括号强化）";
                var config = questionFactory.Config;
                config.MinNumber = 100;
                config.MaxNumber = 999;
                config.MinResult = 100;
                config.MaxResult = 9999;
                config.BracketPropability = 200;
            }
            // 【加减混合】四位数之内的加减混合
            {
                var questionFactory = new JiaJianHunHe.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "四位数之内的加减混合";
                var config = questionFactory.Config;
                config.MinNumber = 100;
                config.MaxNumber = 9999;
                config.MinResult = 100;
                config.MaxResult = 99999;
            }
            // 【加减混合】四位数之内的加减混合（括号强化）
            {
                var questionFactory = new JiaJianHunHe.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "四位数之内的加减混合（括号强化）";
                questionFactory.Level = DifficultyLevel.Hard;
                var config = questionFactory.Config;
                config.MinNumber = 100;
                config.MaxNumber = 9999;
                config.MinResult = 100;
                config.MaxResult = 99999;
                config.BracketPropability = 200;
            }
        }

        private void siZeHunHe(ICollection<IQuestionFactory> collection)
        {
            // 【四则混合】三位数加减，两位数乘、除一位数
            {
                var questionFactory = new SiZeHunHe.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数加减，两位数乘、除一位数";
                var config = questionFactory.Config;
                config.MaxNumber = 999;
                config.MinResult = 100;
                config.MaxResult = 9999;
                config.MaxMultiplyA = 99;
                config.MinMultiplyA = 10;
                config.MaxMultiplyB = 9;
                config.MinMultiplyB = 2;
                config.MaxDivideA = 99;
                config.MinDivideA = 10;
                config.MaxDivideB = 9;
                config.MinDivideB = 2;
            }
        }

        private void chengFa(ICollection<IQuestionFactory> collection)
        {
            // 【乘法】两位数 × 一位数
            {
                var questionFactory = new ChengFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "两位数 × 一位数";
                var config = questionFactory.Config;
                config.MaxA = 99;
                config.MinA = 10;
                config.MaxB = 9;
                config.MinB = 2;
                config.MaxResult = 9999;
            }
            // 【乘法】两位数 × 一位数（≥ 5）
            {
                var questionFactory = new ChengFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "两位数 × 一位数（≥ 5）";
                var config = questionFactory.Config;
                config.MaxA = 99;
                config.MinA = 10;
                config.MaxB = 9;
                config.MinB = 5;
                config.MaxResult = 9999;
            }
            // 【乘法】两位数 × 两位数
            {
                var questionFactory = new ChengFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "两位数 × 两位数";
                questionFactory.Level = DifficultyLevel.Hard;
                var config = questionFactory.Config;
                config.MaxA = 99;
                config.MinA = 10;
                config.MaxB = 99;
                config.MinB = 10;
                config.MaxResult = 9999;
            }
            // 【乘法】三位数 × 一位数
            {
                var questionFactory = new ChengFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数 × 一位数";
                var config = questionFactory.Config;
                config.MaxA = 999;
                config.MinA = 100;
                config.MaxB = 9;
                config.MinB = 2;
                config.MaxResult = 9999;
            }
            // 【乘法】三位数 × 一位数（≥ 5）
            {
                var questionFactory = new ChengFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数 × 一位数（≥ 5）";
                questionFactory.Level = DifficultyLevel.Hard;
                var config = questionFactory.Config;
                config.MaxA = 999;
                config.MinA = 100;
                config.MaxB = 9;
                config.MinB = 5;
                config.MaxResult = 9999;
            }
            // 【乘法】三位数 × 两位数
            {
                var questionFactory = new ChengFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数 × 两位数";
                questionFactory.Level = DifficultyLevel.Hard;
                var config = questionFactory.Config;
                config.MaxA = 999;
                config.MinA = 100;
                config.MaxB = 99;
                config.MinB = 10;
                config.MaxResult = 9999;
            }
            // 【乘法】三位数 × 三位数
            {
                var questionFactory = new ChengFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数 × 三位数";
                questionFactory.Level = DifficultyLevel.VeryHard;
                var config = questionFactory.Config;
                config.MaxA = 999;
                config.MinA = 100;
                config.MaxB = 999;
                config.MinB = 100;
                config.MaxResult = 999999;
            }
            // 【乘法】四位数 × 一位数
            {
                var questionFactory = new ChengFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "四位数 × 一位数";
                var config = questionFactory.Config;
                config.MaxA = 9999;
                config.MinA = 1000;
                config.MaxB = 9;
                config.MinB = 2;
                config.MaxResult = 99999;
            }
            // 【乘法】四位数 × 一位数（≥ 5）
            {
                var questionFactory = new ChengFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "四位数 × 一位数（≥ 5）";
                questionFactory.Level = DifficultyLevel.Hard;
                var config = questionFactory.Config;
                config.MaxA = 9999;
                config.MinA = 1000;
                config.MaxB = 9;
                config.MinB = 5;
                config.MaxResult = 99999;
            }
            // 【乘法】四位数 × 两位数
            {
                var questionFactory = new ChengFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "四位数 × 两位数";
                questionFactory.Level = DifficultyLevel.VeryHard;
                var config = questionFactory.Config;
                config.MaxA = 9999;
                config.MinA = 1000;
                config.MaxB = 99;
                config.MinB = 10;
                config.MaxResult = 999999;
            }
        }

        private void chuFa(ICollection<IQuestionFactory> collection)
        {
            // 【除法】两位数 ÷ 一位数
            {
                var questionFactory = new ChuFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "两位数 ÷ 一位数";
                questionFactory.Level = DifficultyLevel.Easy;
                var config = questionFactory.Config;
                config.MaxA = 99;
                config.MinA = 10;
                config.MaxB = 9;
                config.MinB = 2;
                config.MaxResult = 9999;
            }
            // 【除法】两位数 ÷ 一位数
            {
                var questionFactory = new ChuFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "两位数 ÷ 一位数（≥ 5）";
                var config = questionFactory.Config;
                config.MaxA = 99;
                config.MinA = 10;
                config.MaxB = 9;
                config.MinB = 5;
                config.MaxResult = 9999;
            }
            // 【除法】三位数 ÷ 一位数
            {
                var questionFactory = new ChuFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数 ÷ 一位数";
                var config = questionFactory.Config;
                config.MaxA = 999;
                config.MinA = 100;
                config.MaxB = 9;
                config.MinB = 2;
                config.MaxResult = 9999;
            }
            // 【除法】三位数 ÷ 一位数（≥ 5）
            {
                var questionFactory = new ChuFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数 ÷ 一位数（≥ 5）";
                questionFactory.Level = DifficultyLevel.Hard;
                var config = questionFactory.Config;
                config.MaxA = 999;
                config.MinA = 100;
                config.MaxB = 9;
                config.MinB = 5;
                config.MaxResult = 9999;
            }
            // 【除法】三位数 ÷ 两位数
            {
                var questionFactory = new ChuFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "三位数 ÷ 两位数";
                questionFactory.Level = DifficultyLevel.VeryHard;
                var config = questionFactory.Config;
                config.MaxA = 999;
                config.MinA = 100;
                config.MaxB = 99;
                config.MinB = 10;
                config.MaxResult = 9999;
            }
            // 【除法】四位数 ÷ 一位数
            {
                var questionFactory = new ChuFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "四位数 ÷ 一位数";
                var config = questionFactory.Config;
                config.MaxA = 9999;
                config.MinA = 1000;
                config.MaxB = 9;
                config.MinB = 2;
                config.MaxResult = 9999;
            }
            // 【除法】四位数 ÷ 一位数（≥ 5）
            {
                var questionFactory = new ChuFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "四位数 ÷ 一位数（≥ 5）";
                questionFactory.Level = DifficultyLevel.Hard;
                var config = questionFactory.Config;
                config.MaxA = 9999;
                config.MinA = 1000;
                config.MaxB = 9;
                config.MinB = 5;
                config.MaxResult = 9999;
            }
            // 【除法】四位数 ÷ 两位数
            {
                var questionFactory = new ChuFa.QuestionFactory();
                collection.Add(questionFactory);
                questionFactory.Title = "四位数 ÷ 两位数";
                questionFactory.Level = DifficultyLevel.VeryHard;
                var config = questionFactory.Config;
                config.MaxA = 9999;
                config.MinA = 1000;
                config.MaxB = 99;
                config.MinB = 10;
                config.MaxResult = 9999;
            }
        }
    }
}