using System;
using System.Windows.Forms;

namespace bsd_shuxue_3.Utils
{
    public class MsgBox
    {
        private const string CAPTION_ERROR = "错误:";
        private const string CAPTION_INFO = "信息:";
        private const string CAPTION_QUESTION = "确认:";
        private const string CAPTION_WARN = "警告:";
        private const MessageBoxButtons DEFAULT_BUTTONS = MessageBoxButtons.OK;

        public static DialogResult Error(MessageBoxButtons buttons, String format, params Object[] args)
        {
            return DoMessageBox(CAPTION_ERROR, format, args, MessageBoxIcon.Error, buttons);
        }

        public static DialogResult Error(String format, params Object[] args)
        {
            return DoMessageBox(CAPTION_ERROR, format, args, MessageBoxIcon.Error, DEFAULT_BUTTONS);
        }

        public static DialogResult Info(MessageBoxButtons buttons, String format, params Object[] args)
        {
            return DoMessageBox(CAPTION_INFO, format, args, MessageBoxIcon.Information, buttons);
        }

        public static DialogResult Info(String format, params Object[] args)
        {
            return DoMessageBox(CAPTION_INFO, format, args, MessageBoxIcon.Information, DEFAULT_BUTTONS);
        }

        public static DialogResult Question(MessageBoxButtons buttons, String format, params Object[] args)
        {
            return DoMessageBox(CAPTION_QUESTION, format, args, MessageBoxIcon.Question, buttons);
        }

        public static DialogResult Question(String format, params Object[] args)
        {
            return DoMessageBox(CAPTION_QUESTION, format, args, MessageBoxIcon.Question, MessageBoxButtons.YesNo);
        }

        public static DialogResult Warn(MessageBoxButtons buttons, String format, params Object[] args)
        {
            return DoMessageBox(CAPTION_WARN, format, args, MessageBoxIcon.Warning, buttons);
        }

        public static DialogResult Warn(String format, params Object[] args)
        {
            return DoMessageBox(CAPTION_WARN, format, args, MessageBoxIcon.Warning, DEFAULT_BUTTONS);
        }

        private static DialogResult DoMessageBox(String caption, String format, object[] args, MessageBoxIcon icon,
                                                                            MessageBoxButtons buttons)
        {
            var text = String.Format(format, args);
            return MessageBox.Show(text, caption, buttons, icon);
        }
    }
}