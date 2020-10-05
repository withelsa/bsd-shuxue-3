using bsd_shuxue_3.Domain;
using bsd_shuxue_3.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;

namespace bsd_shuxue_3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 题目生成器列表
        /// </summary>
        private List<ItemWrapper<IQuestionFactory>> questionFactories = new List<ItemWrapper<IQuestionFactory>>();

        /// <summary>
        /// 已出题目列表
        /// </summary>
        private List<IQuestion> questions = new List<IQuestion>();

        /// <summary>
        /// 用于显示的题目列表
        /// </summary>
        private List<IQuestion> questions2show = new List<IQuestion>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.initQuestionFactories();
        }

        /// <summary>
        /// 初始化出题工具类列表
        /// </summary>
        private void initQuestionFactories()
        {
            // 遍历所有定义的 IQuestionFactory 类型
            var questionFactoryType = typeof(IQuestionFactory);
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                // 排除非 IQuestionFactory 类型
                if (type.IsAbstract || !type.IsClass || !questionFactoryType.IsAssignableFrom(type))
                {
                    continue;
                }

                try
                {
                    // 添加实例
                    var questionFactory = Activator.CreateInstance(type) as IQuestionFactory;
                    if (questionFactory == null)
                    {
                        continue;
                    }

                    var itemWrapper = new ItemWrapper<IQuestionFactory>();
                    itemWrapper.Data = questionFactory;
                    itemWrapper.Text = questionFactory.Title;
                    this.questionFactories.Add(itemWrapper);
                }
                catch (Exception)
                {
                }
            }

            // 按照字母序排序
            this.questionFactories.Sort((item1, item2) => item1.Text.CompareTo(item2.Text));

            // 更新 ComboBox 显示
            this.comboQuestionFactories.ItemsSource = this.questionFactories;
            this.comboQuestionFactories.SelectedIndex = 0;
            this.updateComboQuestionFactoriesTooltip();
        }

        private void btnConfigQuestionFactory_Click(object sender, RoutedEventArgs e)
        {
            var questionFactory = this.getCurrentQuestionFactory();
            var configurable = questionFactory as IConfigurable;
            if (configurable != null)
            {
                configurable.DoConfig(this);
            }
        }

        private IQuestionFactory getCurrentQuestionFactory()
        {
            IQuestionFactory questionFactory = null;
            var itemWrapper = this.comboQuestionFactories.SelectedItem as ItemWrapper<IQuestionFactory>;
            if (itemWrapper != null)
            {
                questionFactory = itemWrapper.Data;
            }
            return questionFactory;
        }

        private void btnCreateQuestions_Click(object sender, RoutedEventArgs e)
        {
            // 获取题目生成器
            var questionFactory = this.getCurrentQuestionFactory();
            if (questionFactory == null)
            {
                return;
            }

            // 获取出题数量
            var count = 10;
            if (!int.TryParse(this.txtQuestionCount.Text, out count))
            {
                this.txtQuestionCount.Text = count.ToString();
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

        private void btnClearQuestions_Click(object sender, RoutedEventArgs e)
        {
            this.questions.Clear();
            this.questions2show.Clear();
            this.txtQuestions.Clear();
        }

        private void btnCopyQuestions_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText(this.txtQuestions.Text);
        }

        private void btnShuffleQuestions_Click(object sender, RoutedEventArgs e)
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

        private void btnReorderQuestions_Click(object sender, RoutedEventArgs e)
        {
            this.questions2show.Clear();
            this.questions2show.AddRange(this.questions);
            this.updateQuestionTextBox();
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
                var line = String.Format("【第 {0} 题】\t{1} = {2}", i + 1, question.Content, question.Answer);
                lines.Add(line);
            }

            var text = String.Join("\r\n", lines);
            this.txtQuestions.Text = text;
        }

        private void btnSetQuestionsFont_Click(object sender, RoutedEventArgs e)
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

        private void comboQuestionFactories_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.updateComboQuestionFactoriesTooltip();
        }

        private void updateComboQuestionFactoriesTooltip()
        {
            this.comboQuestionFactories.ToolTip = "";
            var questionFactory = this.getCurrentQuestionFactory();
            if (questionFactory != null)
            {
                this.comboQuestionFactories.ToolTip = String.IsNullOrEmpty(questionFactory.Description) ?
                    questionFactory.Title : questionFactory.Description;
            }
        }
    }
}