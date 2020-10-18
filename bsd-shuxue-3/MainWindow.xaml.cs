using bsd_shuxue_3.Domain;
using bsd_shuxue_3.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;

namespace bsd_shuxue_3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 内部类

        private class QuestionFactoryItem
        {
            public IQuestionFactory Factory { get; set; }

            public DifficultyLevelItem Difficulty { get; set; }
        }

        #endregion 内部类

        #region 成员变量

        /// <summary>
        /// 难度等级
        /// </summary>
        private DifficultyLevelRepo difficultyLevelRepo = new DifficultyLevelRepo();

        /// <summary>
        /// 原始的题目生成器列表
        /// </summary>
        private LinkedList<QuestionFactoryItem> questionFactoryItems = new LinkedList<QuestionFactoryItem>();

        ///// <summary>
        ///// 题目生成器的列表
        ///// </summary>
        //private List<QuestionFactoryItem> itemSourceQuestionFactories = new List<QuestionFactoryItem>();

        /// <summary>
        /// 题目分类的列表
        /// </summary>
        private List<ItemWrapper<String>> itemSourceQuestionCategories = new List<ItemWrapper<String>>();

        /// <summary>
        /// 已出题目列表
        /// </summary>
        private List<IQuestion> questions = new List<IQuestion>();

        /// <summary>
        /// 用于显示的题目列表
        /// </summary>
        private List<IQuestion> questions2show = new List<IQuestion>();

        #endregion 成员变量

        public MainWindow()
        {
            InitializeComponent();
        }

        #region 初始化处理

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title += String.Format(" - 版本({0})", BuildVersion.BUILD_VERSION);
            this.initQuestionFactories();
            this.initComboBoxDifficulties();
            this.initComboBoxQuestionCategories();
            this.filterComboBoxQuestionFactories();
            this.isAnswerVisible = true;
            this.updateShowHideAnswerButtons();
        }

        /// <summary>
        /// 初始化难度等级 ComboBox
        /// </summary>
        private void initComboBoxDifficulties()
        {
            var list = new List<DifficultyLevelItem>(difficultyLevelRepo.List.Count);
            foreach (var item in this.questionFactoryItems)
            {
                var difficulty = this.difficultyLevelRepo.Find(item.Factory.Level);
                if (difficulty == null)
                {
                    difficulty = DifficultyLevelRepo.Normal;
                }
                if (!list.Contains(difficulty))
                {
                    list.Add(difficulty);
                }
            }

            // 按照难度等级排序
            if (list.Count > 1)
            {
                list.Sort((item1, item2) => item1.Level.CompareTo(item2.Level));
                list.Insert(0, DifficultyLevelRepo.All);
            }

            // 绑定到 ComboBox
            this.comboDifficulties.ItemsSource = list;
            this.comboDifficulties.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化出题工具类列表
        /// </summary>
        private void initQuestionFactories()
        {
            var questionFactories = new LinkedList<IQuestionFactory>();
            var initializer = new QuestionFactoriesInitializer();
            initializer.Initialize(questionFactories);

            foreach (var questionFactory in questionFactories)
            {
                var item = new QuestionFactoryItem();
                item.Factory = questionFactory;
                var difficulty = this.difficultyLevelRepo.Find(questionFactory.Level);
                item.Difficulty = difficulty != null ? difficulty : DifficultyLevelRepo.Normal;
                this.questionFactoryItems.AddLast(item);
            }
        }

        /// <summary>
        /// 初始化问题分类 ComboBox
        /// </summary>
        private void initComboBoxQuestionCategories()
        {
            // 遍历所有定义的 IQuestionFactory 类型
            List<String> categories = new List<string>(this.questionFactoryItems.Count);
            foreach (var item in this.questionFactoryItems)
            {
                var category = item.Factory.Category.Trim();
                if (!categories.Contains(category))
                {
                    categories.Add(category);
                }
            }
            categories.Sort();

            // 生成 ComboBox 所需数据
            foreach (var category in categories)
            {
                var itemWrapper = new ItemWrapper<String>()
                {
                    Data = category,
                    Text = category,
                };
                this.itemSourceQuestionCategories.Add(itemWrapper);
            }

            if (this.itemSourceQuestionCategories.Count > 0)
            {
                var categoryAll = new ItemWrapper<string>() { Text = "全部" };
                this.itemSourceQuestionCategories.Insert(0, categoryAll);
            }

            // 绑定到 ComboBox
            this.comboQuestionCategories.ItemsSource = this.itemSourceQuestionCategories;
            this.comboQuestionCategories.SelectedIndex = 0;
        }

        #endregion 初始化处理

        #region 问题生成器过滤和处理

        private void comboDifficulties_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.filterComboBoxQuestionFactories();
        }

        private void comboQuestionCategories_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.filterComboBoxQuestionFactories();
        }

        /// <summary>
        /// 过滤符合条件的问题生成器
        /// </summary>
        private void filterComboBoxQuestionFactories()
        {
            // 过滤出符合条件的问题生成器
            var level = this.comboDifficulties.SelectedItem as DifficultyLevelItem;
            var categoryItem = this.comboQuestionCategories.SelectedItem as ItemWrapper<String>;
            var questionFactories = new List<QuestionFactoryItem>(this.questionFactoryItems.Count);
            foreach (var item in this.questionFactoryItems)
            {
                if (this.match(item, level, categoryItem))
                {
                    questionFactories.Add(item);
                }
            }

            // 绑定到 ComboBox
            this.comboQuestionFactories.ItemsSource = questionFactories;
            this.comboQuestionFactories.SelectedIndex = 0;
        }

        /// <summary>
        /// 检查指定的问题生成器是否符合指定过滤条件
        /// </summary>
        /// <param name="item"></param>
        /// <param name="level"></param>
        /// <param name="categoryItem"></param>
        /// <returns></returns>
        private bool match(QuestionFactoryItem item, DifficultyLevelItem level, ItemWrapper<string> categoryItem)
        {
            // 检查难度
            if (level != null && level.Level != DifficultyLevel.All)
            {
                if ((int)item.Difficulty.Level != (int)level.Level)
                {
                    return false;
                }
            }

            // 检查分类
            String category = categoryItem != null ? categoryItem.Data : null;
            if (!String.IsNullOrEmpty(category))
            {
                if (category != item.Factory.Category)
                {
                    return false;
                }
            }

            // 返回
            return true;
        }

        private void comboQuestionFactories_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.updateComboQuestionFactoriesTooltip();
        }

        private void updateComboQuestionFactoriesTooltip()
        {
            this.comboQuestionFactories.ToolTip = "";
            var questionFactory = this.comboQuestionFactories.SelectedItem as QuestionFactoryItem;
            var tooltip = "";
            if (questionFactory != null)
            {
                tooltip = String.Join("\r\n",
                    String.Format("难度：\t{0}", questionFactory.Difficulty.Title),
                    String.Format("分类：\t{0}", questionFactory.Factory.Category),
                    String.Format("详情：\t{0}", questionFactory.Factory.Description)
                    );
            }
            this.comboQuestionFactories.ToolTip = tooltip;
        }

        #endregion 问题生成器过滤和处理

        #region 内部函数

        private QuestionFactoryItem currentQuestionFactoryItem()
        {
            return this.comboQuestionFactories.SelectedItem as QuestionFactoryItem;
        }

        private IQuestionFactory currentQuestionFactory()
        {
            var item = this.currentQuestionFactoryItem();
            return item != null ? item.Factory : null;
        }

        /// <summary>
        /// 更新题目一览文本框的显示
        /// </summary>
        private void updateQuestionTextBox()
        {
            this.txtQuestions.Clear();

            if (this.questions2show.Count < 1)
            {
                return;
            }
            var lines = new List<String>(this.questions2show.Count);
            for (var i = 0; i < this.questions2show.Count; i++)
            {
                var question = this.questions2show[i];
                String line = null;
                if (this.isAnswerVisible)
                {
                    line = String.Format("【第 {0} 题】\t{1} = {2}", i + 1, question.Content, question.Answer);
                }
                else
                {
                    line = String.Format("【第 {0} 题】\t{1} = ？", i + 1, question.Content);
                }
                lines.Add(line);
            }

            var text = String.Join("\r\n", lines);
            this.txtQuestions.Text = text;
        }

        #endregion 内部函数

        #region 配置当前问题生成器

        private void ConfigQuestionFactory_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var questionFactory = this.currentQuestionFactory();
            var configurable = questionFactory as IConfigurable;
            if (configurable != null && configurable.CanConfig)
            {
                configurable.DoConfig(this);
            }
        }

        private void ConfigQuestionFactory_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            var questionFactory = this.currentQuestionFactory();
            var configurable = questionFactory as IConfigurable;
            e.CanExecute = configurable != null && configurable.CanConfig;
        }

        #endregion 配置当前问题生成器

        #region 题目数量 TextBox 的输入内容校验

        /// <summary>
        /// 一次生成问题的最大数量
        /// </summary>
        private int maxQuestionCount = 100;

        /// <summary>
        /// 数字类正则表达式
        /// </summary>
        private Regex regexNumber = new Regex("[0-9]+");

        private void txtQuestionCount_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var valid = false;
            if (this.regexNumber.IsMatch(e.Text))
            {
                try
                {
                    var text = this.txtQuestionCount.Text + e.Text;
                    var value = int.Parse(text);
                    valid = value <= this.maxQuestionCount;
                }
                catch (Exception)
                {
                    // 忽略本错误
                }
            }
            e.Handled = !valid;
        }

        #endregion 题目数量 TextBox 的输入内容校验

        #region 生成问题

        private void CreateQuestions_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            // 获取题目生成器
            var questionFactory = this.currentQuestionFactory();
            if (questionFactory == null)
            {
                return;
            }

            // 获取出题数量
            var count = 10;
            if (!int.TryParse(this.txtQuestionCount.Text, out count))
            {
                MsgBox.Error("【出题数量】没有指定，或者指定的数值无效");
                this.txtQuestionCount.SelectAll();
                this.txtQuestionCount.Focus();
                return;
            }

            // 开始出题
            for (int i = 0; i < count; i++)
            {
                var question = questionFactory.CreateInstance();
                this.questions.Add(question);
                this.questions2show.Add(question);
            }

            // 更新题目显示
            this.updateQuestionTextBox();
        }

        private void CreateQuestions_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            var questionFactory = this.currentQuestionFactory();
            e.CanExecute = questionFactory != null;
        }

        #endregion 生成问题

        #region 打乱题目顺序、恢复题目顺序

        private void ShuffleQuestions_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var random = new Random();
            this.questions2show.Clear();
            this.questions.ForEach(question =>
            {
                if (this.questions2show.Count < 1)
                {
                    this.questions2show.Add(question);
                }
                else
                {
                    var index = random.Next(0, this.questions2show.Count + 1);
                    this.questions2show.Insert(index, question);
                }
            });
            this.updateQuestionTextBox();
        }

        private void ShuffleQuestions_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.questions.Count > 0;
        }

        private void ReorderQuestions_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.questions2show.Clear();
            this.questions2show.AddRange(this.questions);
            this.updateQuestionTextBox();
        }

        private void ReorderQuestions_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.questions.Count > 0;
        }

        #endregion 打乱题目顺序、恢复题目顺序

        #region 清空问题

        private void ClearQuestions_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.questions.Clear();
            this.questions2show.Clear();
            this.txtQuestions.Clear();
        }

        private void ClearQuestions_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.questions.Count > 0;
        }

        #endregion 清空问题

        #region 复制问题到剪贴板

        private void CopyQuestions_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            copyQuestions();
        }

        private void copyQuestions()
        {
            System.Windows.Clipboard.SetText(this.txtQuestions.Text);
            MsgBox.Info("所有题目都已经复制到剪贴板");
        }

        private void CopyQuestions_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.txtQuestions?.Text?.Length > 0;
        }

        private void txtQuestions_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.txtQuestions?.Text?.Length > 0)
            {
                copyQuestions();
            }
        }

        #endregion 复制问题到剪贴板

        #region 设置字体

        private void SetQuestionsFont_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var font = new Font(this.txtQuestions.FontFamily.Source, (float)this.txtQuestions.FontSize);
            var fontDialog = new FontDialog();
            fontDialog.Font = font;
            fontDialog.ShowEffects = false;
            fontDialog.ShowColor = false;
            fontDialog.ShowApply = false;
            if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtQuestions.FontFamily = new System.Windows.Media.FontFamily(fontDialog.Font.Name);
                this.txtQuestions.FontSize = fontDialog.Font.Size;
            }
        }

        private void SetQuestionsFont_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion 设置字体

        #region 显示、隐藏答案

        private bool isAnswerVisible = false;

        private void updateShowHideAnswerButtons()
        {
            this.btnShowAnswer.Visibility = this.isAnswerVisible ? Visibility.Collapsed : Visibility.Visible;
            this.btnHideAnswer.Visibility = this.isAnswerVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnShowAnswer_Click(object sender, RoutedEventArgs e)
        {
            this.isAnswerVisible = true;
            this.updateShowHideAnswerButtons();
            this.updateQuestionTextBox();
        }

        private void btnHideAnswer_Click(object sender, RoutedEventArgs e)
        {
            this.isAnswerVisible = false;
            this.updateShowHideAnswerButtons();
            this.updateQuestionTextBox();
        }

        #endregion 显示、隐藏答案
    }
}