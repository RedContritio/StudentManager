using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace StudentManager.Manager
{
    /// <summary>
    /// StudentModify.xaml 的交互逻辑
    /// </summary>
    public partial class StudentModify : MetroWindow
    {
        public static String SNO, SNAME, SSEX, SSEC, SDEPT, SLOC, STEL;
        public static DateTime SBIRTH;
        public static String SUSER;
        public static bool Result = false;

        private void CkBoxUser_Checked(object sender, RoutedEventArgs e)
        {
            if(CBoxSUSER != null)
            {
                CBoxSUSER.IsEnabled = (CkBoxUser?.IsChecked == true);
                TBoxSUSER.IsEnabled = (CkBoxUser?.IsChecked == true);
            }
        }

        private void CkBoxTUser_Checked(object sender, RoutedEventArgs e)
        {
            if (CBoxSUSER != null)
                CBoxSUSER.Visibility = (CkBoxTUser?.IsChecked == true) ? Visibility.Collapsed : Visibility.Visible;
            if (TBoxSUSER != null)
                TBoxSUSER.Visibility = (CkBoxTUser?.IsChecked == true) ? Visibility.Visible : Visibility.Collapsed;

            if(CkBoxTUser.IsChecked == true)
                TBoxSUSER.Text = CBoxSUSER.Text;
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            if(TBoxSNO.Text.Length <= 0)
            {
                this.ShowMessage("学号不能为空", "信息格式错误");
                return ;
            }
            if (TBoxSNO.Text.Length > 16)
            {
                this.ShowMessage("学号不能长于16位", "信息格式错误");
                return;
            }

            if (TBoxSNAME.Text.Length <= 1)
            {
                this.ShowMessage("姓名不能短于两个字符", "信息格式错误");
                return;
            }
            if (TBoxSNAME.Text.Length > 16)
            {
                this.ShowMessage("姓名不能长于16个字符", "信息格式错误");
                return;
            }

            if (TBoxSSEC.Text.Length > 8)
            {
                this.ShowMessage("入学年份不能长于8个字符", "信息格式错误");
                return;
            }

            if (TBoxSLOC.Text.Length > 64)
            {
                this.ShowMessage("籍贯不能长于64个字符", "信息格式错误");
                return;
            }
            if (TBoxSTEL.Text.Length > 32)
            {
                this.ShowMessage("联系电话不能长于32个字符", "信息格式错误");
                return;
            }

            ViewToData();
            Result = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public StudentModify()
        {
            InitializeComponent();

            Result = false;
        }

        private void DataToView()
        {
            TBoxSNO.Text = SNO;
            TBoxSNAME.Text = SNAME;
            foreach (ComboBoxItem it in CBoxSSEX.Items)
            {
                if (it.Content.ToString() == SSEX)
                {
                    CBoxSSEX.SelectedItem = it;
                    break;
                }
            }
            if (CBoxSSEX.SelectedIndex == -1)
                CBoxSSEX.SelectedIndex = 0;

            TBoxSSEC.Text = SSEC;

            CBoxSDEPT.Items.Clear();
            {
                MySqlCommand cmd = new MySqlCommand("select DNAME from d", App.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    String dname = reader.GetString(0);
                    CBoxSDEPT.Items.Add(new ComboBoxItem()
                    {
                        Content = dname
                    });
                    if(dname == SDEPT)
                    {
                        CBoxSDEPT.SelectedIndex = CBoxSDEPT.Items.Count - 1;
                    }
                }
                reader.Close();

                if(CBoxSDEPT.SelectedIndex == -1)
                    CBoxSDEPT.SelectedIndex = 0;
            }

            DPickerSBIRTH.SelectedDate = SBIRTH;

            DPickerSBIRTH.DisplayDateEnd = DateTime.Now;

            TBoxSLOC.Text = SLOC;
            TBoxSTEL.Text = STEL;

            CkBoxUser.IsChecked = (SUSER != null);
            TBoxSUSER.IsEnabled = (CkBoxUser.IsChecked == true);
            {
                MySqlCommand cmd = new MySqlCommand("select User from allow_user", App.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                bool existCurrentData = false;
                while (reader.Read())
                {
                    String uname = reader.GetString(0);
                    CBoxSUSER.Items.Add(new ComboBoxItem()
                    {
                        Content = uname
                    });
                    if (SUSER != null && uname == SUSER)
                    {
                        CBoxSUSER.SelectedIndex = CBoxSUSER.Items.Count - 1;
                        existCurrentData = true;
                    }
                }
                reader.Close();
                if(existCurrentData)
                    return;
            }

            CkBoxTUser.IsChecked = true;
            TBoxSUSER.Text = SUSER;
        }
        private void ViewToData()
        {
            SNO = TBoxSNO.Text;
            SNAME = TBoxSNAME.Text;
            SSEX = (CBoxSSEX.SelectedItem as ComboBoxItem).Content.ToString();
            SSEC = TBoxSSEC.Text;
            SDEPT = (CBoxSDEPT.SelectedItem as ComboBoxItem).Content.ToString();

            SBIRTH = (DateTime) DPickerSBIRTH.SelectedDate;

            SLOC = TBoxSLOC.Text;
            STEL = TBoxSTEL.Text;

            SUSER = null;
            if(CkBoxUser.IsChecked == true)
            {
                if(CkBoxTUser.IsChecked == true)
                    SUSER = TBoxSUSER.Text;
                else if(CBoxSUSER.SelectedItem != null)
                    SUSER = (CBoxSUSER.SelectedItem as ComboBoxItem).Content.ToString();
            }
        }

        public void SetData(object[] ItemsArray)
        {
            SNO = ItemsArray[0] as String;
            SNAME = ItemsArray[1] as String;
            SSEX = ItemsArray[2] as String;
            SSEC = ItemsArray[3] as String;
            SDEPT = ItemsArray[4] as String;
            SBIRTH = ItemsArray[5] != null ? (DateTime) ItemsArray[5] : DateTime.Now;
            SLOC = ItemsArray[6] as String;
            STEL = ItemsArray[7] as String;

            SUSER = ItemsArray[8] as String;

            DataToView();
        }
    }
}
