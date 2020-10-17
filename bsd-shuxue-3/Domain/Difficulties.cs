using System;
using System.Collections.Generic;

namespace bsd_shuxue_3.Domain
{
    internal enum DifficultyLevel
    {
        VeryEasy = 10,
        Easy = 20,
        Normal = 30,
        Hard = 40,
        VeryHard = 50,
        Insane = 100,
        All = 0,
    }

    internal class DifficultyLevelItem
    {
        public DifficultyLevelItem(DifficultyLevel level, String title, String image)
        {
            this.Level = level;
            this.Title = title;
            this.Image = image;
        }

        /// <summary>
        /// 难度等级
        /// </summary>
        public DifficultyLevel Level { get; }

        /// <summary>
        /// 描述
        /// </summary>
        public String Title { get; }

        /// <summary>
        /// 图片 URL
        /// </summary>
        public String Image { get; }
    }

    internal class DifficultyLevelRepo
    {
        public static readonly DifficultyLevelItem VeryEasy = new DifficultyLevelItem(
            DifficultyLevel.VeryEasy, "【非常】简单", "./resources/very-easy.png");

        public static readonly DifficultyLevelItem Easy = new DifficultyLevelItem(
            DifficultyLevel.Easy, "简单", "resources/easy.png");

        public static readonly DifficultyLevelItem Normal = new DifficultyLevelItem(
            DifficultyLevel.Normal, "一般", "resources/normal.png");

        public static readonly DifficultyLevelItem Hard = new DifficultyLevelItem(
            DifficultyLevel.Hard, "困难", "resources/hard.png");

        public static readonly DifficultyLevelItem VeryHard = new DifficultyLevelItem(
            DifficultyLevel.VeryHard, "【非常】困难", "resources/very-hard.png");

        public static readonly DifficultyLevelItem Insane = new DifficultyLevelItem(
            DifficultyLevel.Insane, "【超级】困难", "resources/insane.png");

        public static readonly DifficultyLevelItem All = new DifficultyLevelItem(
            DifficultyLevel.All, "全部", "resources/all-items.png");

        public LinkedList<DifficultyLevelItem> List { get; } = new LinkedList<DifficultyLevelItem>();
        public Dictionary<DifficultyLevel, DifficultyLevelItem> Dict { get; } = new Dictionary<DifficultyLevel, DifficultyLevelItem>();

        public DifficultyLevelRepo()
        {
            this.Register(All, VeryEasy, Easy, Normal, Hard, VeryHard, Insane);
        }

        public void Register(params DifficultyLevelItem[] items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (!this.List.Contains(item))
                    {
                        this.List.AddLast(item);
                    }
                    this.Dict[item.Level] = item;
                }
            }
        }

        public DifficultyLevelItem Find(DifficultyLevel level)
        {
            DifficultyLevelItem item = null;
            this.Dict.TryGetValue(level, out item);
            return item;
        }
    }
}