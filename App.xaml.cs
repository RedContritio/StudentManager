using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;

namespace StudentManager
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow mainWindow;

        public static MySqlConnection Connection;

        private static string admin_only_username = "s2018302289";
        public static string username = "";
        public static string passwd = "GaussDB@123";

        public static bool isAdmin = false;

        public static bool fastEdit = false;

        public static bool Connect(String connStr)
        {
            if(Connection != null && Connection.State == ConnectionState.Open)
            {
                MessageDialogResult result = App.mainWindow.ShowMessageOKCancel("确认更换账号吗？", "更换账户");

                if (result != MessageDialogResult.Affirmative)
                {
                    return false;
                }
                App.Disconnect();
            }

            isAdmin = false;
            Connection = new MySqlConnection(connStr);

            try
            {
                Connection.Open();
                CheckAuthority();
            }
            catch (MySqlException ex)
            {
                App.mainWindow.ShowMessage(ex.Message, "SQL异常");
            }
            finally
            {
                mainWindow.LogStateChanged.Invoke();
            }
            return Connection.State == ConnectionState.Open;
        }

        public static void Disconnect()
        {
            if(Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
            isAdmin = false;
            mainWindow.LogStateChanged.Invoke();
        }

        public static void CheckAuthority()
        {
            if (Connection.State != ConnectionState.Open)
            {
                return;
            }

            //MySqlCommand cmd = new MySqlCommand("select Select_priv, Insert_priv, Update_priv, Delete_priv from mysql.user where user() REGEXP(concat('^', User, '@[^@]+$'))", App.Connection);

            //try
            //{
            //    MySqlDataReader reader = cmd.ExecuteReader();

            //    if (reader.Read())
            //    {
            //        for(int i=0; i<reader.FieldCount; ++i)
            //        {
            //            if(reader.GetString(i) != "Y")
            //            {
            //                return;
            //            }
            //        }
            //    }
            //    reader.Close();

            //    isAdmin = true;
            //}
            //catch (Exception ex)
            //{
            //}

            if(username == admin_only_username)
            {
                isAdmin = true;
            }

        }
    }
}
