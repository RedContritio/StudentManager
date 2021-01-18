using MySql.Data.MySqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentManager.Student
{
    /// <summary>
    /// BasicInfo.xaml 的交互逻辑
    /// </summary>
    public partial class BasicInfo : UserControl
    {
        private DateTime birthday;
        public BasicInfo()
        {
            InitializeComponent();

            RefreshData();

            MySqlCommand cmd = new MySqlCommand("update s set SUSER=@user where SNO=@sno", App.Connection);
            MySqlParameter snovalue = new MySqlParameter("@sno", TBoxSNO.Text);
            MySqlParameter user = new MySqlParameter("@user", App.username);
            cmd.Parameters.Add(snovalue);
            cmd.Parameters.Add(user);
            cmd.ExecuteNonQuery();

            return ;
        }

        private void ButtonModify_Click(object sender, RoutedEventArgs e)
        {
            ButtonModify.IsEnabled = false;
            ButtonApply.IsEnabled = true;
            ButtonRevert.IsEnabled = true;


            TBoxSNAME.IsReadOnly = false;
            TBoxSLOC.IsReadOnly = false;
            TBoxSTEL.IsReadOnly = false;

            TBoxSSEX.Visibility = Visibility.Hidden;
            CBoxSSEX.Visibility = Visibility.Visible;
            foreach (ComboBoxItem it in CBoxSSEX.Items)
            {
                if(it.Content.ToString() == TBoxSSEX.Text)
                {
                    CBoxSSEX.SelectedItem = it;
                    break;
                }
            }

            TBoxSBIRTH.Visibility = Visibility.Hidden;
            DPickerSBIRTH.Visibility = Visibility.Visible;
            DPickerSBIRTH.SelectedDate = birthday;
            DPickerSBIRTH.DisplayDateEnd = DateTime.Now;
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            if(TBoxSNAME.Text?.Length <= 1)
            {
                App.mainWindow.ShowMessage("姓名不能为空或长度不大于一个字符。", "信息格式错误");
                return;
            }
            if (TBoxSNAME.Text?.Length > 16)
            {
                App.mainWindow.ShowMessage("姓名不能超过 16 个字符。", "信息格式错误");
                return;
            }
            if (CBoxSSEX.SelectedItem == null)
            {
                App.mainWindow.ShowMessage("性别不能为空。", "信息格式错误");
                return;
            }
            if (TBoxSLOC.Text?.Length > 64)
            {
                App.mainWindow.ShowMessage("籍贯不能超过 64 个字符。", "信息格式错误");
                return;
            }
            if (TBoxSTEL.Text?.Length > 32)
            {
                App.mainWindow.ShowMessage("籍贯不能超过 32 个字符。", "信息格式错误");
                return;
            }

            DateTime date = (DateTime)DPickerSBIRTH.SelectedDate;

            MySqlCommand cmd = new MySqlCommand("update s set SNAME=@name, SSEX=@sex, SBIRTH=@birth, SLOC=@loc, STEL=@tel where SNO=@sno", App.Connection);
            cmd.Parameters.Add(new MySqlParameter("@sno", TBoxSNO.Text));
            cmd.Parameters.Add(new MySqlParameter("@name", TBoxSNAME.Text));
            cmd.Parameters.Add(new MySqlParameter("@sex", (CBoxSSEX.SelectedItem as ComboBoxItem).Content));
            cmd.Parameters.Add(new MySqlParameter("@birth", date.ToString("yyyy-MM-dd")));
            cmd.Parameters.Add(new MySqlParameter("@loc", TBoxSLOC.Text));
            cmd.Parameters.Add(new MySqlParameter("@tel", TBoxSTEL.Text));
            cmd.ExecuteNonQuery();

            RefreshData();

            ButtonModify.IsEnabled = true;
            ButtonApply.IsEnabled = false;
            ButtonRevert.IsEnabled = false;
        }

        private void ButtonRevert_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();

            ButtonModify.IsEnabled = true;
            ButtonApply.IsEnabled = false;
            ButtonRevert.IsEnabled = false;
        }

        private void RefreshData()
        {
            MySqlCommand cmd = new MySqlCommand("select * from view_s_s", App.Connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                TBoxSNO.Text = reader.GetString(0);
                TBoxSNAME.Text = reader.GetString(1);
                TBoxSSEX.Text = reader.GetString(2);
                TBoxSSEC.Text = reader.GetString(3);
                TBoxSDEPT.Text = reader.GetString(4);
                birthday = reader.GetDateTime(5);
                TBoxSLOC.Text = reader.GetString(6);
                TBoxSTEL.Text = reader.GetString(7);
                TBoxSBIRTH.Text = birthday.ToLongDateString();
            }

            reader.Close();

            TBoxSNO.IsReadOnly = true;
            TBoxSNAME.IsReadOnly = true;
            TBoxSSEX.IsReadOnly = true;
            TBoxSSEC.IsReadOnly = true;
            TBoxSDEPT.IsReadOnly = true;
            TBoxSBIRTH.IsReadOnly = true;
            TBoxSLOC.IsReadOnly = true;
            TBoxSTEL.IsReadOnly = true;

            TBoxSNO.Visibility = Visibility.Visible;
            TBoxSNAME.Visibility = Visibility.Visible;
            TBoxSSEX.Visibility = Visibility.Visible;
            TBoxSSEC.Visibility = Visibility.Visible;
            TBoxSDEPT.Visibility = Visibility.Visible;
            TBoxSBIRTH.Visibility = Visibility.Visible;

            CBoxSSEX.Visibility = Visibility.Hidden;
            DPickerSBIRTH.Visibility = Visibility.Hidden;
        }
    }
}
