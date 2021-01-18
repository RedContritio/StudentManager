using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
    /// StudentList.xaml 的交互逻辑
    /// </summary>
    public partial class StudentList : UserControl
    {
        private MySqlDataAdapter adapter;
        private DataTable dataTable;
        public StudentList()
        {
            InitializeComponent();

            RefreshData();
        }

        private void RefreshData()
        {
            adapter = new MySqlDataAdapter("select SNO, SNAME, SSEX, SSEC, DNAME, SBIRTH, SLOC, STEL, SUSER from s, d where s.SDEPT = d.DNO", App.Connection);

            DataSet dataSet = new DataSet();

            adapter.Fill(dataSet);

            dataTable = dataSet.Tables[0];

            datagrid.ItemsSource = dataTable.DefaultView;

            updateTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void ButtonModify_Click(object sender, RoutedEventArgs e)
        {
            if(datagrid.SelectedItem == null)
            {
                App.mainWindow.ShowMessage("请选中要修改的行", "非法操作");
                return ;
            }

            StudentModify modifyWindow = new StudentModify();
            object[] itemsArray = (datagrid.SelectedItem as DataRowView).Row.ItemArray;
            modifyWindow.SetData(itemsArray);
            modifyWindow.ShowDialog();
            if(StudentModify.Result)
            {
                foreach (DataRowView it in datagrid.Items)
                {
                    if(it == datagrid.SelectedItem)
                        continue;
                    if((it.Row.ItemArray[0] as String) == StudentModify.SNO)
                    {
                        App.mainWindow.ShowMessage("指定的学号与现有学号发生冲突", "非法操作");
                        return ;
                    }
                }

                String DNO;
                {
                    MySqlCommand cmd = new MySqlCommand($"select DNO from d where DNAME=@dname", App.Connection);
                    cmd.Parameters.Add(new MySqlParameter("@dname", StudentModify.SDEPT));
                    DNO = (string) cmd.ExecuteScalar();
                }

                {
                    MySqlCommand cmd = new MySqlCommand("update s set SNO=@nsno, SNAME=@name, SSEX=@sex, SSEC=@sec, SDEPT=@sdept, SBIRTH=@birth, SUSER=@user, SLOC=@loc, STEL=@tel where SNO=@sno", App.Connection);
                    cmd.Parameters.Add(new MySqlParameter("@sno", itemsArray[0] as String));
                    cmd.Parameters.Add(new MySqlParameter("@nsno", StudentModify.SNO));
                    cmd.Parameters.Add(new MySqlParameter("@name", StudentModify.SNAME));
                    cmd.Parameters.Add(new MySqlParameter("@sex", StudentModify.SSEX));
                    cmd.Parameters.Add(new MySqlParameter("@sec", StudentModify.SSEC));
                    cmd.Parameters.Add(new MySqlParameter("@sdept", DNO));
                    cmd.Parameters.Add(new MySqlParameter("@birth", StudentModify.SBIRTH.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new MySqlParameter("@user", StudentModify.SUSER));
                    cmd.Parameters.Add(new MySqlParameter("@loc", StudentModify.SLOC));
                    cmd.Parameters.Add(new MySqlParameter("@tel", StudentModify.STEL));
                    cmd.ExecuteNonQuery();
                }

                if (StudentModify.SUSER != null)
                {
                    //MySqlCommand cmd = new MySqlCommand($"call grant_init('{StudentModify.SUSER}');", App.Connection);
                    MySqlCommand cmd = new MySqlCommand("call grant_init(@user);", App.Connection);
                    cmd.Parameters.Add(new MySqlParameter("@user", StudentModify.SUSER));
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        App.mainWindow.ShowMessage($"无法对用户 {StudentModify.SUSER} 进行授权。", "权限异常");
                    }
                }
                RefreshData();
            }
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            StudentModify modifyWindow = new StudentModify();
            object[] itemsArray = new object[] { null, null, null, null, null,
                                                null, null, null, null, null};
            modifyWindow.SetData(itemsArray);
            modifyWindow.ShowDialog();
            if (StudentModify.Result)
            {
                foreach (DataRowView it in datagrid.Items)
                {
                    if (it == datagrid.SelectedItem)
                        continue;
                    if ((it.Row.ItemArray[0] as String) == StudentModify.SNO)
                    {
                        App.mainWindow.ShowMessage("指定的学号与现有学号发生冲突", "非法操作");
                        return;
                    }
                }

                String DNO;
                {
                    MySqlCommand cmd = new MySqlCommand($"select DNO from d where DNAME=@dname", App.Connection);
                    cmd.Parameters.Add(new MySqlParameter("@dname", StudentModify.SDEPT));
                    DNO = (string)cmd.ExecuteScalar();
                }

                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO `s` (`SNO`, `SNAME`, `SSEX`, `SSEC`, `SDEPT`, `SBIRTH`, `SUSER`, `SLOC`, `STEL`) VALUES (@sno, @name, @sex, @sec, @sdept, @birth, @user, @loc, @tel);", App.Connection);
                    cmd.Parameters.Add(new MySqlParameter("@sno", StudentModify.SNO));
                    cmd.Parameters.Add(new MySqlParameter("@name", StudentModify.SNAME));
                    cmd.Parameters.Add(new MySqlParameter("@sex", StudentModify.SSEX));
                    cmd.Parameters.Add(new MySqlParameter("@sec", StudentModify.SSEC));
                    cmd.Parameters.Add(new MySqlParameter("@sdept", DNO));
                    cmd.Parameters.Add(new MySqlParameter("@birth", StudentModify.SBIRTH.ToString("yyyy-MM-dd")));
                    cmd.Parameters.Add(new MySqlParameter("@user", StudentModify.SUSER));
                    cmd.Parameters.Add(new MySqlParameter("@loc", StudentModify.SLOC));
                    cmd.Parameters.Add(new MySqlParameter("@tel", StudentModify.STEL));
                    cmd.ExecuteNonQuery();
                }

                if(StudentModify.SUSER != null)
                {
                    MySqlCommand cmd = new MySqlCommand("call grant_init(@user);", App.Connection);
                    cmd.Parameters.Add(new MySqlParameter("@user", StudentModify.SUSER));
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        App.mainWindow.ShowMessage($"无法对用户 {StudentModify.SUSER} 进行授权。", "权限异常");
                    }
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

            {
                MySqlCommand cmd = new MySqlCommand($"select COUNT(*) from sc where SNO=@sno", App.Connection);
                cmd.Parameters.Add(new MySqlParameter("@sno", SNO));
                long cnt = (long) cmd.ExecuteScalar();
                if(cnt > 0)
                {
                    App.mainWindow.ShowMessage("由于成绩表中有该学生的成绩，因此无法删除该学生记录", "非法操作");
                    return;
                }
            }

            {
                MySqlCommand cmd = new MySqlCommand("DELETE FROM s WHERE(`SNO` = @sno);", App.Connection);
                cmd.Parameters.Add(new MySqlParameter("@sno", SNO));
                cmd.ExecuteNonQuery();
            }

            RefreshData();
        }
    }
}
