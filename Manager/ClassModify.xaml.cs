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
    /// ClassModify.xaml 的交互逻辑
    /// </summary>
    public partial class ClassModify : MetroWindow
    {
        public static String CNO, CNAME, DNAME, CDESC;
        public static float CCREDIT;
        public static bool Result = false;
        public ClassModify()
        {
            InitializeComponent();
            Result = false;
            DataToView();
        }


        private void DataToView()
        {
            TBoxCNO.Text = CNO;
            TBoxCNAME.Text = CNAME;

            CBoxCDEPT.Items.Clear();
            {
                MySqlCommand cmd = new MySqlCommand("select DNAME from d", App.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    String dname = reader.GetString(0);
                    CBoxCDEPT.Items.Add(new ComboBoxItem()
                    {
                        Content = dname
                    });
                    if (dname == DNAME)
                    {
                        CBoxCDEPT.SelectedIndex = CBoxCDEPT.Items.Count - 1;
                    }
                }
                reader.Close();

                if (CBoxCDEPT.SelectedIndex == -1)
                    CBoxCDEPT.SelectedIndex = 0;
            }

            TBoxCDESC.Text = CDESC;
            SliderCCREDIT.Value = CCREDIT;
        }

        private void ViewToData()
        {
            CNO = TBoxCNO.Text;
            CNAME = TBoxCNAME.Text;
            DNAME = (CBoxCDEPT.SelectedItem as ComboBoxItem).Content.ToString();
            CDESC = TBoxCDESC.Text;
            CCREDIT = (float) SliderCCREDIT.Value;
        }

        private void SliderCCREDIT_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        public void SetData(object[] ItemsArray)
        {
            CNO = ItemsArray[0] as String;
            CNAME = ItemsArray[1] as String;
            DNAME = ItemsArray[2] as String;
            CDESC = ItemsArray[3] as String;
            CCREDIT = ItemsArray[4] != null ? (float) ItemsArray[4] : 0;

            DataToView();
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            if (TBoxCNO.Text.Length <= 0)
            {
                this.ShowMessage("编号不能为空", "信息格式错误");
                return;
            }
            if (TBoxCNO.Text.Length > 16)
            {
                this.ShowMessage("编号不能长于16位", "信息格式错误");
                return;
            }

            if (TBoxCNAME.Text.Length <= 1)
            {
                this.ShowMessage("名称不能短于两个字符", "信息格式错误");
                return;
            }
            if (TBoxCNAME.Text.Length > 32)
            {
                this.ShowMessage("名称不能长于32个字符", "信息格式错误");
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
    }
}
