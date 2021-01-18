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
    /// DeptList.xaml 的交互逻辑
    /// </summary>
    public partial class DeptList : UserControl
    {
        private MySqlDataAdapter adapter;
        private DataTable dataTable;

        public DeptList()
        {
            InitializeComponent();

            RefreshData();
        }


        private void RefreshData()
        {
            adapter = new MySqlDataAdapter("select DNO, DNAME from d", App.Connection);

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

            DeptModify modifyWindow = new DeptModify();
            object[] itemsArray = (datagrid.SelectedItem as DataRowView).Row.ItemArray;
            modifyWindow.SetData(itemsArray);
            modifyWindow.ShowDialog();
            if (DeptModify.Result)
            {
                foreach (DataRowView it in datagrid.Items)
                {
                    if (it == datagrid.SelectedItem)
                        continue;
                    if ((it.Row.ItemArray[0] as String) == DeptModify.DNO)
                    {
                        App.mainWindow.ShowMessage("指定的编号与现有编号发生冲突", "非法操作");
                        return;
                    }
                }

                {
                    MySqlCommand cmd = new MySqlCommand("update d set DNO=@ndno, DNAME=@name where DNO=@dno", App.Connection);
                    cmd.Parameters.Add(new MySqlParameter("@dno", itemsArray[0] as String));
                    cmd.Parameters.Add(new MySqlParameter("@ndno", DeptModify.DNO));
                    cmd.Parameters.Add(new MySqlParameter("@name", DeptModify.DNAME));
                    cmd.ExecuteNonQuery();
                }

                RefreshData();
            }
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            DeptModify modifyWindow = new DeptModify();
            object[] itemsArray = new object[] { null, null, null, null, null, null, null };
            modifyWindow.SetData(itemsArray);
            modifyWindow.ShowDialog();
            if (DeptModify.Result)
            {
                foreach (DataRowView it in datagrid.Items)
                {
                    if (it == datagrid.SelectedItem)
                        continue;
                    if ((it.Row.ItemArray[0] as String) == DeptModify.DNO)
                    {
                        App.mainWindow.ShowMessage("指定的编号与现有编号发生冲突", "非法操作");
                        return;
                    }
                }

                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO `d` (`DNO`, `DNAME`) VALUES (@dno, @name);", App.Connection);
                    cmd.Parameters.Add(new MySqlParameter("@dno", DeptModify.DNO));
                    cmd.Parameters.Add(new MySqlParameter("@name", DeptModify.DNAME));
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
            String DNO = itemsArray[0] as String;

            {
                MySqlCommand cmd = new MySqlCommand($"select COUNT(*) from s where SDEPT=@dno", App.Connection);
                cmd.Parameters.Add(new MySqlParameter("@dno", DNO));
                long cnt = (long)cmd.ExecuteScalar();
                if (cnt > 0)
                {
                    App.mainWindow.ShowMessage("由于存在该学院学生，无法删除该学院记录", "非法操作");
                    return;
                }
            }

            {
                MySqlCommand cmd = new MySqlCommand($"select COUNT(*) from c where CDEPT=@dno", App.Connection);
                cmd.Parameters.Add(new MySqlParameter("@dno", DNO));
                long cnt = (long)cmd.ExecuteScalar();
                if (cnt > 0)
                {
                    App.mainWindow.ShowMessage("由于存在该学院课程，无法删除该学院记录", "非法操作");
                    return;
                }
            }

            {
                MySqlCommand cmd = new MySqlCommand("DELETE FROM d WHERE(`DNO` = @dno);", App.Connection);
                cmd.Parameters.Add(new MySqlParameter("@dno", DNO));
                cmd.ExecuteNonQuery();
            }

            RefreshData();
        }
    }
}
