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
using System.Windows.Shapes;

namespace ApplicationUP.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageRegistraziya.xaml
    /// </summary>
    public partial class PageRegistraziya : Page
    {
        public PageRegistraziya()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Registr(object sender, RoutedEventArgs e)
        {
            bool res = await CheckPassCopy();
            if (res)
            {
                bool result = await CheckLogin();
                if (result)
                {
                    var r = await RegisterProfile();
                    if (r)
                    {
                        NavigationService.GoBack();
                    }
            
                }
            }
        }

        private void Nazad(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private async Task<bool> CheckPassCopy()
        {
            if (passTBox.Password == passcopyTBox.Password)
            {

                return true;

            }
            MessageBox.Show("Пароли не совпадают!", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Information);
            return false;

        }
        private async Task<bool> CheckLogin()
        {
            Connector con = new Connector();

            string sql = "SELECT * FROM пользователи WHERE login = @login";

            MySqlCommand cmd = new MySqlCommand(sql, con.GetConn());
            cmd.Parameters.Add(new MySqlParameter("@login", loginTBox.Text));

            await con.GetOpen();

            MySqlDataReader readed = await cmd.ExecuteReaderAsync();

            if (readed.HasRows)
            {
                await con.GetClose();
                MessageBox.Show("Такой логин уже существует. Придумайте другой логин", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            await con.GetClose();

            return true;

        }

        private async Task<bool> RegisterProfile()
        {
            Connector con = new Connector();
            if (string.IsNullOrEmpty(loginTBox.Text) | string.IsNullOrEmpty(passTBox.Password)
                | string.IsNullOrEmpty(passcopyTBox.Password) | string.IsNullOrEmpty(nameTBox.Text) 
                | !numberTBox.IsMaskFull)
            {
                await con.GetClose();
                MessageBox.Show("Заполните все поля", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }


            string sql = "INSERT INTO `пользователи` (`login`, `parol`, `numaber_tel`, `role_id`, name) " +
                "VALUES (@login, @pass, @Number_phone, @role_id, @name)";
            MySqlCommand cmd = new MySqlCommand(sql, con.GetConn());
            cmd.Parameters.Add(new MySqlParameter("@name", nameTBox.Text));
            cmd.Parameters.Add(new MySqlParameter("@login", loginTBox.Text));
            cmd.Parameters.Add(new MySqlParameter("@pass", passTBox.Password));
            cmd.Parameters.Add(new MySqlParameter("@Number_phone", numberTBox.Text));
            cmd.Parameters.Add(new MySqlParameter("@role_id", 2));
            await con.GetOpen();
            await cmd.ExecuteNonQueryAsync();
            await con.GetClose();
            MessageBox.Show("Вы зарегестрировались! Пожалуйста авторизуйтесь", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Information);

            return true;

        }
    }
}
