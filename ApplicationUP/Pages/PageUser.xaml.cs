using ApplicationUP.Core;
using ApplicationUP.Template;
using MySqlConnector;
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
using static ApplicationUP.Core.CoreSys;

namespace ApplicationUP.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageUser.xaml
    /// </summary>
    public partial class PageUser : Page, PageLoadUpdate
    {
        IList<User>
             user = new List<User>();
        MainWindow
             window;
        string
            Loginuser;
        public PageUser(MainWindow main, string loginuser)
        {
            InitializeComponent();
            window = main;
            Loginuser = loginuser;

            Load();
        }
        public async void Load()
        {

            await LoadGoods();
        }
        private async Task LoadGoods()
        {
            Connector
                conn = new Connector();
            string
                sql = "SELECT * FROM `пользователи` WHERE login=@login";
            MySqlCommand
                cmd = new MySqlCommand(sql, conn.GetConn());
            cmd.Parameters.Add(new MySqlParameter("@login", Loginuser));
            await conn.GetOpen();
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                user.Clear();
                await conn.GetClose();
                return;
            }

            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {
                // Добавление элемента в коллекцию списка товаров на основе класса (Экземпляр класс создается - объект)
                user.Add(new User()
                {
                    Login = reader["login"].ToString(),
                    Name = reader["name"].ToString(),
                    Parol = reader["parol"].ToString(),
                    Numaber_tel = reader["numaber_tel"].ToString(),
                    Role_id = Convert.ToInt32(reader["role_id"]),
                });

                this.DataContext = user;

            }

            // Асинхронное отключение от БД
            await conn.GetClose();
            // Возращение true
            return;
        }
        private void Exit(object sender, MouseButtonEventArgs e)
        {
            window.Close();
        }

        private async void UpdateUser(object sender, MouseButtonEventArgs e)
        {
            bool result = await UpdateGoods();
            if (result)
            {
                MessageBox.Show("Данные изменились", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                window.UpdateUser(InputName.Text);
            }
            else
                MessageBox.Show("Данные не изменились. Заполните все поля", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Information);

           // window.PageOformlenieClose();
        }

        public void ReloadData()
        {
            user.Clear();
            Load();
        }

        private async Task<bool> UpdateGoods()
        {

            if (IsValidText(InputName))
            {
                InputName.Focusable = true;
                return false;
            }
            else if (!InputNumber_tel.IsMaskFull)
            {
                InputNumber_tel.Focusable = true;
                return false;
            }

            string
                sql =
                "UPDATE `пользователи` " +
                "SET `name`=@1, `numaber_tel`=@2 " +
                "WHERE `login`=@login;";

            Connector
                conn = new Connector();

            MySqlCommand
                cmd = new MySqlCommand(sql, conn.GetConn());

            cmd.Parameters.Add(new MySqlParameter("@1", InputName.Text));
            cmd.Parameters.Add(new MySqlParameter("@2", InputNumber_tel.Text));
            cmd.Parameters.Add(new MySqlParameter("@login", Loginuser));

            await conn.GetOpen();

            if (await cmd.ExecuteNonQueryAsync() == 1)
            {
                await conn.GetClose();
                return true;
            }

            await conn.GetClose();
            return false;
        }

        private bool IsValidText(TextBox box)
        {
            return string.IsNullOrEmpty(box.Text) || box.Text.Length < 1;
        }
    }
}
