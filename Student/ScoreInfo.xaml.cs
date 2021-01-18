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
    /// ScoreInfo.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreInfo : UserControl
    {
        private MySqlDataAdapter adapter;
        private DataTable dataTable;
        public ScoreInfo()
        {
            InitializeComponent();

            adapter = new MySqlDataAdapter("select CNAME 课程, CCREDIT 学分, SCORE 成绩 from view_sc_s", App.Connection);

            DataSet dataSet = new DataSet();

            adapter.Fill(dataSet);

            dataTable = dataSet.Tables[0];

            datagrid.ItemsSource = dataTable.DefaultView;

            double sum = 0;
            double sum_w = 0;
            int cnt = 0;
            double cnt_w = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                if(row["成绩"] != DBNull.Value)
                {
                    double sc = (float) row["成绩"], cre = (float) row["学分"];
                    sum += sc;
                    sum_w += sc * cre;
                    ++cnt;
                    cnt_w += cre;
                }
            }

            avgScore.Content = sum / cnt;
            avgWScore.Content = sum_w / cnt_w;
            updateTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
