using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// ClassInfo.xaml 的交互逻辑
    /// </summary>
    public partial class ClassInfo : UserControl
    {
        private MySqlDataAdapter adapter;
        private DataTable dataTable;
        public ClassInfo()
        {
            InitializeComponent();

            adapter = new MySqlDataAdapter("select CNO 课程编号, CNAME 名称, DNAME 开课学院, CDESC 课程介绍, CCREDIT 学分 from view_c_s", App.Connection);

            DataSet dataSet = new DataSet();

            adapter.Fill(dataSet);

            dataTable = dataSet.Tables[0];

            datagrid.ItemsSource = dataTable.DefaultView;

            updateTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
