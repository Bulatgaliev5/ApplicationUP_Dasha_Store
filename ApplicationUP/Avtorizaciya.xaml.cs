using ApplicationUP;
using ApplicationUP.Core;
using MySqlConnector;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace AplicationUP.Windows
{
    /// <summary>
    /// Логика взаимодействия для Avtorizaciya.xaml
    /// </summary>
    public partial class Avtorizaciya : Window
    {
        int Role_id;
        string
            name,
            Role_name;
        public Avtorizaciya()
        {
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
            MainWindow main = new MainWindow(this, name, Role_id, Role_name);

            loginTBox.Text = "";
            passTBox.Password = "";
            this.Hide();
            main.Show();
        }

        private void Vhod(object sender, RoutedEventArgs e)
        {
            CheckData();
        }

        private async Task<bool> CheckPass()    
        {
            Connector con = new Connector();

            string sql = " SELECT u.name AS 'UserName', r.role_id AS 'RoleID', r.name AS 'RoleName' FROM пользователи u, роли r WHERE u.login = @login AND u.parol = @pass AND u.role_id = r.role_id";

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
                break;
            }
            //Console.WriteLine(readed["Name"]);
            await con.GetClose();
            return true;
        }
    }
}
