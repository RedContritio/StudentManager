using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentManager
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static MainWindow instance = null;

        public static bool isStudent = true;
        public static bool Logged
        {
            get { return (App.Connection.State == System.Data.ConnectionState.Open); }
        } 

        public delegate void CallbackEvent();

        public CallbackEvent LogStateChanged = null;

        public MainWindow()
        {
            InitializeComponent();
            App.mainWindow = this;
            instance = this;

            ChangeUserView(App.isAdmin);

            LogStateChanged += Default_LogStateChanged;
        }

        public void Default_LogStateChanged()
        {
            NavBar.IsEnabled = Logged;
            foreach (TreeViewItem it in NavBar.Items)
            {
                it.IsSelected = false;
                it.IsExpanded = false;
            }
            TabPages.Items.Clear(); 
            ChangeUserView(App.isAdmin);
            MenuLogout.IsEnabled = Logged;
            UpdateMenuCloseTab();
        }

        private void MenuLogin_Click(object sender, RoutedEventArgs e)
        {
            LogDialog logDialog = new LogDialog();
            logDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            logDialog.Owner = this;
            logDialog.ShowDialog();
        }
        private void MenuLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageDialogResult result = this.ShowMessageOKCancel("确认登出账号吗？", "警告");

            if (result == MessageDialogResult.Affirmative)
            {
                App.Disconnect();
            }
        }
        private void TreeViewBasicInfo_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            TabItemCreate("学籍信息", new Student.BasicInfo());
        }

        private void TreeViewScoreInfo_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            TabItemCreate("课程成绩", new Student.ScoreInfo());
        }

        private void TreeViewClassInfo_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            TabItemCreate("开课查询", new Student.ClassInfo());
        }

        private void MenuNavBar_Checked(object sender, RoutedEventArgs e)
        {
            if (NavBar != null)
                NavBar.Visibility = Visibility.Visible;
        }

        private void MenuNavBar_Unchecked(object sender, RoutedEventArgs e)
        {
            if (NavBar != null)
                NavBar.Visibility = Visibility.Collapsed;
        }

        private void TreeViewStudentList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            TabItemCreate("学生情况", new Manager.StudentList());
        }
        private void TreeViewClassList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            TabItemCreate("课程情况", new Manager.ClassList());
        }
        private void TreeViewDeptList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            TabItemCreate("学院信息", new Manager.DeptList());
        }
        private void TreeViewScoreList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            TabItemCreate("学生成绩", new Manager.ScoreList());
        }
        private void TreeViewClassScoreStat_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Manager.ScoreAnalyzer analyzer = new Manager.ScoreAnalyzer();
            analyzer.SetCommand("select CNAME `课程`, COUNT(*) `考试人次`, avg(CSCORE) `平均成绩`, min(CSCORE) `最低分`, max(CSCORE) `最高分`  from c, sc where c.CNO = sc.CNO and CSCORE is not null group by c.CNO;");
            TabItemCreate("课程成绩分析", analyzer);
        }
        private void TreeViewDeptScoreStat_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Manager.ScoreAnalyzer analyzer = new Manager.ScoreAnalyzer();
            analyzer.SetCommand("select DNAME `专业`, COUNT(*) `考试人次`, avg(CSCORE) `平均成绩`, min(CSCORE) `最低分`, max(CSCORE) `最高分` from s, sc, d where s.SNO = sc.SNO and S.SDEPT = d.DNO and CSCORE is not null group by SDEPT;");
            TabItemCreate("专业成绩分析", analyzer);
        }
        private void TreeViewPersonScoreStat_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Manager.ScoreAnalyzer analyzer = new Manager.ScoreAnalyzer();
            analyzer.SetCommand("select s.SNO `学号`, SNAME `姓名`, COUNT(*) `考试课程数量`, avg(CSCORE) `平均成绩`, min(CSCORE) `最低分`, max(CSCORE) `最高分`  from s, sc where s.SNO = sc.SNO and CSCORE is not null group by s.SNO;");
            TabItemCreate("个人成绩分析", analyzer);
        }
        private void TreeViewDeptClassScoreStat_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Manager.ScoreAnalyzer analyzer = new Manager.ScoreAnalyzer();
            analyzer.SetCommand("select CNAME `课程`, DNAME `专业`, COUNT(*) `考试人次`, avg(CSCORE) `平均成绩`, min(CSCORE) `最低分`, max(CSCORE) `最高分` from s, c, sc, d where s.SNO = sc.SNO and c.CNO = sc.CNO and S.SDEPT = d.DNO and CSCORE is not null group by SDEPT, c.CNO;");
            TabItemCreate("课程、专业成绩分析", analyzer);
        }
        public static void TabItemCreate(string header, UIElement content)
        {
            if(instance == null || instance.TabPages == null)
                return ;

            foreach (TabItem it in instance.TabPages.Items)
            {
                if (header.Equals(it.Header))
                {
                    instance.TabPages.Items.Remove(it);
                    break;
                }
            }

            TabItem tabItem = new TabItem() { Header = header };

            tabItem.Content = content;

            instance.TabPages.Items.Add(tabItem);

            instance.TabPages.SelectedIndex = instance.TabPages.Items.Count - 1;

            App.mainWindow.UpdateMenuCloseTab();
        }

        public void ChangeUserView(bool isAdmin)
        {
            bool isUser = !isAdmin;
            for(int i=0; i<NavBar.Items.Count; ++i)
            {
                TreeViewItem it = NavBar.Items[i] as TreeViewItem;

                if(ForceShowAll.IsChecked || isUser == (i < 3))
                    it.Visibility = Visibility.Visible;
                else
                    it.Visibility = Visibility.Collapsed;
            }

            if(isAdmin)
                ForceShowAll.Visibility = Visibility.Visible;
            else
                ForceShowAll.Visibility = Visibility.Collapsed;

            //if(isUser)
            //    MenuOption.Visibility = Visibility.Collapsed;
            //else
            //    MenuOption.Visibility = Visibility.Visible;
        }

        private void MenuFastEdit_Checked(object sender, RoutedEventArgs e)
        {
            App.fastEdit = true;
        }

        private void MenuFastEdit_Unchecked(object sender, RoutedEventArgs e)
        {
            App.fastEdit = false;
        }

        private void ForceShowAll_Checked(object sender, RoutedEventArgs e)
        {
            ChangeUserView(App.isAdmin);
        }

        private void MenuCloseTabs_Click(object sender, RoutedEventArgs e)
        {
            TabPages.Items.Clear();

            UpdateMenuCloseTab();
        }

        private void MenuCloseTab_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = sender as MenuItem;
            foreach (TabItem tab in TabPages.Items)
            {
                if(tab.Header == menu.Header)
                {
                    TabPages.Items.Remove(tab);
                    MenuCloseTab.Items.Remove(menu);
                    break;
                }
            }
        }
        private void UpdateMenuCloseTab()
        {
            MenuCloseTab.Items.Clear();
            foreach (TabItem tab in TabPages.Items)
            {
                MenuItem menu = new MenuItem()
                {
                    Header = tab.Header,
                };
                menu.Click += MenuCloseTab_Click;
                MenuCloseTab.Items.Add(menu);
            }
        }
    }
}
