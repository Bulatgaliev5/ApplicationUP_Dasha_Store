using ApplicationUP.Core;
using MySqlConnector;
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

namespace ApplicationUP.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAutoresaziya.xaml
    /// </summary>
    public partial class PageAutoresaziya : Page
    {
        PageRegistraziya
             pageRegistraziya;
        int Role_id;
        string
            name,
            Role_name,
            UserLogin;
        public PageAutoresaziya()
        {
            pageRegistraziya = new PageRegistraziya();
            InitializeComponent();
        }
        private async void CheckData()
        {
            bool state = await CheckPass();

            if (!state)
            {
                return;
            }
            //  MessageBox.Show(string.Format("{0} - {1} - {2}", name, role_id, role_name));
            MainWindow main = new MainWindow( name, Role_id, Role_name, UserLogin);

            loginTBox.Text = "";
            passTBox.Password = "";

            main.Show();
        }
        private void Zaregestrirovatsa(object sender, RoutedEventArgs e)
        {
           NavigationService.Navigate(pageRegistraziya);
        }



        private void Vhod(object sender, RoutedEventArgs e)
        {
            CheckData();
        }
        private async Task<bool> CheckPass()
        {
            Connector con = new Connector();

            string sql = "SELECT u.name AS 'UserName', r.role_id AS 'RoleID', r.name AS 'RoleName',u.login AS 'UserLogin'  FROM пользователи u, роли r WHERE BINARY u.login = @login AND u.parol = @pass AND u.role_id = r.role_id";

            MySqlCommand cmd = new MySqlCommand(sql, con.GetConn());
            cmd.Parameters.Add(new MySqlParameter("@login", loginTBox.Text));
            cmd.Parameters.Add(new MySqlParameter("@pass", passTBox.Password));

            await con.GetOpen();

            MySqlDataReader readed = await cmd.ExecuteReaderAsync();
            if (!readed.HasRows)
            {
                await con.GetClose();
                MessageBox.Show("Не правильный логин или пароль ", "Ошибка", MessageBoxButton.OK
                , MessageBoxImage.Error);
                return false;
            }
            while (await readed.ReadAsync())
            {
                name = Convert.ToString(readed["UserName"]);
                Role_name = Convert.ToString(readed["RoleName"]);
                Role_id = Convert.ToInt32(readed["RoleID"]);
                UserLogin = Convert.ToString(readed["UserLogin"]);
                break;
            }
            //Console.WriteLine(readed["Name"]);
            await con.GetClose();
            return true;
        }
    }
}
