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
    /// ScoreList.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreList : UserControl
    {
        private MySqlDataAdapter adapter;
        private DataTable dataTable;
        public ScoreList()
        {
            InitializeComponent();

            RefreshData();
        }


        private void RefreshData()
        {
            String SelectFilter = "s.SNO = sc.SNO and c.CNO = sc.CNO and s.SDEPT = d.DNO";

            if(CkBoxDEPTFilter.IsChecked == true && CBoxDEPTFilter.SelectedItem != null && (CBoxDEPTFilter.SelectedItem as ComboBoxItem).ToolTip != null)
            {
                SelectFilter += $" and d.DNO = '{(CBoxDEPTFilter.SelectedItem as ComboBoxItem).ToolTip}'";
            }

            adapter = new MySqlDataAdapter($"select s.SNO, SNAME, c.CNO, CNAME, CCREDIT, CSCORE SCORE from s, c, sc, d where {SelectFilter}", App.Connection);

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

            ScoreModify modifyWindow = new ScoreModify();
            object[] itemsArray = (datagrid.SelectedItem as DataRowView).Row.ItemArray;
            modifyWindow.SetData(itemsArray);
            modifyWindow.ShowDialog();
            if (ScoreModify.Result)
            {
                foreach (DataRowView it in datagrid.Items)
                {
                    if (it == datagrid.SelectedItem)
                        continue;
                    if ((it.Row.ItemArray[0] as String) == ScoreModify.SNO && (it.Row.ItemArray[2] as String) == ScoreModify.CNO)
                    {
                        App.mainWindow.ShowMessage("已经存在相同学生相同课程的记录", "非法操作");
                        return;
                    }
                }

                {
                    MySqlCommand cmd = new MySqlCommand("update sc set SNO=@nsno, CNO=@ncno, CSCORE=@score where SNO=@sno and CNO=@cno", App.Connection);
                    cmd.Parameters.Add(new MySqlParameter("@sno", itemsArray[0] as String));
                    cmd.Parameters.Add(new MySqlParameter("@cno", itemsArray[2] as String));
                    cmd.Parameters.Add(new MySqlParameter("@nsno", ScoreModify.SNO));
                    cmd.Parameters.Add(new MySqlParameter("@ncno", ScoreModify.CNO));
                    cmd.Parameters.Add(new MySqlParameter("@score", ScoreModify.Score));
                    cmd.ExecuteNonQuery();
                }

                RefreshData();
            }
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            ScoreModify modifyWindow = new ScoreModify();
            object[] itemsArray = new object[] { null, null, null, null, null, null, null };
            modifyWindow.SetData(itemsArray);
            modifyWindow.ShowDialog();
            if (ScoreModify.Result)
            {
                foreach (DataRowView it in datagrid.Items)
                {
                    if (it == datagrid.SelectedItem)
                        continue;
                    if ((it.Row.ItemArray[0] as String) == ScoreModify.SNO && (it.Row.ItemArray[2] as String) == ScoreModify.CNO)
                    {
                        App.mainWindow.ShowMessage("已经存在相同学生相同课程的记录", "非法操作");
                        return;
                    }
                }

                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO `sc` (`SNO`, `CNO`, `CSCORE`) VALUES (@sno, @cno, @score);", App.Connection);
                    cmd.Parameters.Add(new MySqlParameter("@sno", ScoreModify.SNO));
                    cmd.Parameters.Add(new MySqlParameter("@cno", ScoreModify.CNO));
                    cmd.Parameters.Add(new MySqlParameter("@score", ScoreModify.Score));
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
            String SNO = itemsArray[0] as String;
            String CNO = itemsArray[2] as String;

            {
                MySqlCommand cmd = new MySqlCommand("DELETE FROM s WHERE(`SNO` = @sno and `CNO` = @cno);", App.Connection);
                cmd.Parameters.Add(new MySqlParameter("@sno", SNO));
                cmd.Parameters.Add(new MySqlParameter("@cno", CNO));
                cmd.ExecuteNonQuery();
            }

            RefreshData();
        }

        private void CheckBoxDEPTFilter_CheckChanged(object sender, RoutedEventArgs e)
        {
            if(CkBoxDEPTFilter.IsChecked == true)
            {
                CBoxDEPTFilter.IsEnabled = true;
            }
            else
            {
                CBoxDEPTFilter.IsEnabled = false;
            }
            UpdateDEPTFilter();
        }

        private void CBoxDEPTFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshData();
        }

        private void UpdateDEPTFilter()
        {
            String selectStr = CBoxDEPTFilter.SelectedItem != null ? ((CBoxDEPTFilter.SelectedItem as ComboBoxItem).Content as String) : null;
            CBoxDEPTFilter.Items.Clear();

            MySqlCommand cmd = new MySqlCommand("select DNO, DNAME from d", App.Connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                String dno = reader.GetString(0);
                String dname = reader.GetString(1);
                CBoxDEPTFilter.Items.Add(new ComboBoxItem()
                {
                    Content = dname,
                    ToolTip = dno
                });
            }
            reader.Close();

            if(selectStr == null)
                return ;
            foreach (ComboBoxItem it in CBoxDEPTFilter.Items)
            {
                if(it.Content as String == selectStr)
                {
                    CBoxDEPTFilter.SelectedItem = it;
                    break;
                }
            }
        }
    }
}
