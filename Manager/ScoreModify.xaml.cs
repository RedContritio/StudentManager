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
    /// ScoreModify.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreModify : MetroWindow
    {
        public static String SNO, SNAME, CNO, CNAME;
        public static float? Score;
        public static bool Result = false;
        public ScoreModify()
        {
            InitializeComponent();
            Result = false;
            DataToView();
        }

        private void DataToView()
        {
            TBoxSNO.Text = SNO;

            UpdateCBoxSNAME();

            TBoxCNO.Text = CNO;

            UpdateCBoxCNAME();

            CkBoxInvalidScore.IsChecked = (Score == null);
            if(Score != null)
            {
                SliderScore.Value = (float) Score;
            }
        }

        private void ViewToData()
        {
            SNO = TBoxSNO.Text;
            SNAME = (CBoxSNAME.SelectedItem as ComboBoxItem).Content as String;
            CNO = TBoxCNO.Text;
            CNAME = (CBoxCNAME.SelectedItem as ComboBoxItem).Content as String;
            if(CkBoxInvalidScore.IsChecked == true)
                Score = null;
            else
                Score = (float) SliderScore.Value;
        }

        private void TBoxSNO_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateCBoxSNAME();
        }

        private void CBoxSNAME_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CBoxSNAME?.SelectedItem != null && TBoxSNO != null)
                TBoxSNO.Text = (CBoxSNAME.SelectedItem as ComboBoxItem).ToolTip as String;
        }

        public void SetData(object[] ItemsArray)
        {
            SNO = ItemsArray[0] as String;
            CNO = ItemsArray[2] as String;
            Score = ItemsArray[5] as float?;

            DataToView();
        }

        private void TBoxCNO_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateCBoxCNAME();
        }

        private void CBoxCNAME_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBoxCNAME?.SelectedItem != null && TBoxCNO != null)
                TBoxCNO.Text = (CBoxCNAME.SelectedItem as ComboBoxItem).ToolTip as String;
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            if (TBoxSNO.Text.Length <= 0)
            {
                this.ShowMessage("学号不能为空", "信息格式错误");
                return;
            }
            if (TBoxSNO.Text.Length > 16)
            {
                this.ShowMessage("学号不能长于16位", "信息格式错误");
                return;
            }
            if(CBoxSNAME.SelectedItem == null)
            {
                this.ShowMessage("不能对未添加的学生设置成绩", "信息格式错误");
                return;
            }

            if (TBoxCNO.Text.Length <= 0)
            {
                this.ShowMessage("课程编号不能为空", "信息格式错误");
                return;
            }
            if (TBoxCNO.Text.Length > 16)
            {
                this.ShowMessage("课程不能长于16位", "信息格式错误");
                return;
            }
            if (CBoxCNAME.SelectedItem == null)
            {
                this.ShowMessage("不能对未添加的课程设置成绩", "信息格式错误");
                return;
            }

            ViewToData();
            Result = true;
            Close();
        }

        private void CkBoxInvalidScore_CheckChanged(object sender, RoutedEventArgs e)
        {
            SliderScore.IsEnabled = TBoxScore.IsEnabled = (CkBoxInvalidScore.IsChecked == false);
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateCBoxSNAME()
        {
            CBoxSNAME.Items.Clear();
            {
                MySqlCommand cmd = new MySqlCommand("select SNO, SNAME from s where SNO like @regsno", App.Connection);
                cmd.Parameters.Add(new MySqlParameter("@regsno", $"%{TBoxSNO.Text}%"));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    String refsno = reader.GetString(0);
                    String sname = reader.GetString(1);
                    CBoxSNAME.Items.Add(new ComboBoxItem()
                    {
                        Content = sname,
                        ToolTip = refsno
                    });

                    if (TBoxSNO.Text == refsno)
                    {
                        CBoxSNAME.SelectedIndex = CBoxSNAME.Items.Count - 1;
                    }
                }
                reader.Close();
            }
        }

        private void UpdateCBoxCNAME()
        {
            CBoxCNAME.Items.Clear();
            {
                MySqlCommand cmd = new MySqlCommand("select CNO, CNAME from c where CNO like @regcno", App.Connection);
                cmd.Parameters.Add(new MySqlParameter("@regcno", $"%{TBoxCNO.Text}%"));
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    String refcno = reader.GetString(0);
                    String cname = reader.GetString(1);
                    CBoxCNAME.Items.Add(new ComboBoxItem()
                    {
                        Content = cname,
                        ToolTip = refcno
                    });

                    if (TBoxCNO.Text == refcno)
                    {
                        CBoxCNAME.SelectedIndex = CBoxCNAME.Items.Count - 1;
                    }
                }
                reader.Close();
            }
        }
    }
}
