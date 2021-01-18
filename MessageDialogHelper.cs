using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager
{
    public static class MessageDialogHelper
    {
        private static MetroDialogSettings translatedSettings = new MetroDialogSettings();

        static MessageDialogHelper()
        {
            translatedSettings.AffirmativeButtonText = "确认";
            translatedSettings.NegativeButtonText = "取消";
        }

        public static MessageDialogResult ShowMessage(this MetroWindow window, string message, string title)
        {
            return window.ShowModalMessageExternal(title, message, MessageDialogStyle.Affirmative, translatedSettings);
        }
        public static MessageDialogResult ShowMessageOKCancel(this MetroWindow window, string message, string title)
        {
            return window.ShowModalMessageExternal(title, message, MessageDialogStyle.AffirmativeAndNegative, translatedSettings);
        }
    }
}
