using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudentManager
{
    /// <summary>
    /// LogDialog.xaml 的交互逻辑
    /// </summary>
    public partial class LogDialog : MetroWindow
    {
        public LogDialog()
        {
            InitializeComponent();

            userBox.Text = App.username;
            passwdBox.Password = App.passwd;
        }

        private void ButtonLog_Click(object sender, RoutedEventArgs e)
        {
            if(!(userBox.Text?.Length > 0))
            {
                this.ShowMessage("请输入用户名", "提示");
                return;
            }
            // 密码可以为空
            //if (passwdBox.Password?.Length <= 0)
            //{
            //  this.ShowMessage("请输入密码", "提示");
            //}
            String passwdStr = passwdBox.Password?.Length > 0 ? ($" password={passwdBox.Password};") : "";
            //if(App.Connect($"server=127.0.0.1;port=3306;user={userBox.Text};{passwdStr} database=sm;"))

            App.username = userBox.Text;
            App.passwd = passwdBox.Password;
            if (App.Connect($"server=139.9.119.34;port=3306;user={userBox.Text};{passwdStr} database=sm_2018302289;"))
                {
                if (App.isAdmin)
                {
                    this.ShowMessage($"管理员 {userBox.Text}，欢迎回来！", "登陆成功");
                }
                else
                {
                    this.ShowMessage($"{userBox.Text}，欢迎回来！", "登陆成功");
                }
                this.Close();
            }
            else
            {
                this.ShowMessage("用户名或密码错误！", "登录失败");
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
