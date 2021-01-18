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
    /// <summary>
    /// ClassList.xaml 的交互逻辑
    /// </summary>
    public partial class ClassList : UserControl
    {
        private MySqlDataAdapter adapter;
        private DataTable dataTable;
        public ClassList()
        {
            InitializeComponent();

            RefreshData();
        }

        private void RefreshData()
        {
            adapter = new MySqlDataAdapter("select CNO, CNAME, DNAME, CDESC, CCREDIT from c, d where c.CDEPT = d.DNO", App.Connection);

            DataSet dataSet = new DataSet();

            adapter.Fill(dataSet);

            dataTable = dataSet.Tables[0];

            datagrid.ItemsSource = dataTable.DefaultView;

            updateTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void ButtonModify_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid.SelectedItem == null)
            {
                App.mainWindow.ShowMessage("请选中要修改的行", "非法操作");
                return;
            }

            ClassModify modifyWindow = new ClassModify();
            object[] itemsArray = (datagrid.SelectedItem as DataRowView).Row.ItemArray;
            modifyWindow.SetData(itemsArray);
            modifyWindow.ShowDialog();
            if (ClassModify.Result)
            {
                foreach (DataRowView it in datagrid.Items)
                {
                    if (it == datagrid.SelectedItem)
                        continue;
                    if ((it.Row.ItemArray[0] as String) == ClassModify.CNO)
                    {
                        App.mainWindow.ShowMessage("指定的编号与现有编号发生冲突", "非法操作");
                        return;
                    }
                }

                String DNO;
                {
                    MySqlCommand cmd = new MySqlCommand($"select DNO from d where DNAME=@dname", App.Connection);
                    cmd.Parameters.Add(new MySqlParameter("@dname", ClassModify.DNAME));
                    DNO = (string)cmd.ExecuteScalar();
                }

                {
                    MySqlCommand cmd = new MySqlCommand("update c set CNO=@ncno, CNAME=@name, CDEPT=@dno, CDESC=@desc, CCREDIT=@credit where CNO=@cno", App.Connection);
                    cmd.Parameters.Add(new MySqlParameter("@cno", itemsArray[0] as String));
                    cmd.Parameters.Add(new MySqlParameter("@ncno", ClassModify.CNO));
                    cmd.Parameters.Add(new MySqlParameter("@name", ClassModify.CNAME));
                    cmd.Parameters.Add(new MySqlParameter("@dno", DNO));
                    cmd.Parameters.Add(new MySqlParameter("@desc", ClassModify.CDESC));
                    cmd.Parameters.Add(new MySqlParameter("@credit", ClassModify.CCREDIT));
                    cmd.ExecuteNonQuery();
                }

                RefreshData();
            }
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            ClassModify modifyWindow = new ClassModify();
            object[] itemsArray = new object[] { null, null, null, null, null, null, null };
            modifyWindow.SetData(itemsArray);
            modifyWindow.ShowDialog();
            if (ClassModify.Result)
            {
                foreach (DataRowView it in datagrid.Items)
                {
                    if (it == datagrid.SelectedItem)
                        continue;
                    if ((it.Row.ItemArray[0] as String) == ClassModify.CNO)
                    {
                        App.mainWindow.ShowMessage("指定的编号与现有编号发生冲突", "非法操作");
                        return;
                    }
                }

                String DNO;
                {
                    MySqlCommand cmd = new MySqlCommand($"select DNO from d where DNAME=@dname", App.Connection);
                    cmd.Parameters.Add(new MySqlParameter("@dname", ClassModify.DNAME));
                    DNO = (string)cmd.ExecuteScalar();
                }

                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO `c` (`CNO`, `CNAME`, `CDEPT`, `CDESC`, `CCREDIT`) VALUES (@cno, @name, @dept, @desc, @credit);", App.Connection);
                    cmd.Parameters.Add(new MySqlParameter("@cno", ClassModify.CNO));
                    cmd.Parameters.Add(new MySqlParameter("@name", ClassModify.CNAME));
                    cmd.Parameters.Add(new MySqlParameter("@dept", DNO));
                    cmd.Parameters.Add(new MySqlParameter("@desc", ClassModify.CDESC));
                    cmd.Parameters.Add(new MySqlParameter("@credit", ClassModify.CCREDIT));
                    cmd.ExecuteNonQuery();
                }

                RefreshData();
            }
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid.SelectedItem == null)
            {
                App.mainWindow.ShowMessage("请选中要删除的行", "非法操作");
                return;
            }

            object[] itemsArray = (datagrid.SelectedItem as DataRowView).Row.ItemArray;
            String CNO = itemsArray[0] as String;

            {
                MySqlCommand cmd = new MySqlCommand($"select COUNT(*) from sc where CNO=@cno", App.Connection);
                cmd.Parameters.Add(new MySqlParameter("@cno", CNO));
                long cnt = (long)cmd.ExecuteScalar();
                if (cnt > 0)
                {
                    App.mainWindow.ShowMessage("由于存在该课程成绩，无法删除该课程记录", "非法操作");
                    return;
                }
            }

            {
                MySqlCommand cmd = new MySqlCommand("DELETE FROM c WHERE(`CNO` = @cno);", App.Connection);
                cmd.Parameters.Add(new MySqlParameter("@cno", CNO));
                cmd.ExecuteNonQuery();
            }

            RefreshData();
        }
    }
}
