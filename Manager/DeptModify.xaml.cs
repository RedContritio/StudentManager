using MahApps.Metro.Controls;
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
    /// DeptModify.xaml 的交互逻辑
    /// </summary>
    public partial class DeptModify : MetroWindow
    {
        public static String DNO, DNAME;
        public static bool Result = false;
        public DeptModify()
        {
            InitializeComponent();

            Result = false;
        }

        private void DataToView()
        {
            TBoxDNO.Text = DNO;
            TBoxDNAME.Text = DNAME;
        }

        private void ViewToData()
        {
            DNO = TBoxDNO.Text;
            DNAME = TBoxDNAME.Text;
        }

        public void SetData(object[] ItemsArray)
        {
            DNO = ItemsArray[0] as String;
            DNAME = ItemsArray[1] as String;

            DataToView();
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            if (TBoxDNO.Text.Length <= 0)
            {
                this.ShowMessage("编号不能为空", "信息格式错误");
                return;
            }
            if (TBoxDNO.Text.Length > 16)
            {
                this.ShowMessage("编号不能长于16位", "信息格式错误");
                return;
            }

            if (TBoxDNAME.Text.Length <= 1)
            {
                this.ShowMessage("名称不能短于两个字符", "信息格式错误");
                return;
            }
            if (TBoxDNAME.Text.Length > 32)
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
