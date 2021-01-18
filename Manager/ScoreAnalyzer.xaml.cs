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

namespace StudentManager.Manager
{
    public partial class ScoreAnalyzer : UserControl
    {
        private MySqlDataAdapter adapter;
        private DataTable dataTable;
        private String _command = null;
        public ScoreAnalyzer()
        {
            InitializeComponent();

            RefreshData();
        }

        public void SetCommand(String command)
        {
            _command = command;

            RefreshData();
        }

        public void RefreshData()
        {
            if(_command == null)
                return ;

            adapter = new MySqlDataAdapter(_command, App.Connection);

            DataSet dataSet = new DataSet();

            adapter.Fill(dataSet);

            dataTable = dataSet.Tables[0];

            datagrid.ItemsSource = dataTable.DefaultView;

            updateTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
