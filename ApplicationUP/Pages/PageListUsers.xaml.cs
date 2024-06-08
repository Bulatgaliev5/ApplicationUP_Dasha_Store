using ApplicationUP.Template;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ApplicationUP.Core;
using MySqlConnector;
using System.Windows.Input;
using static ApplicationUP.Core.CoreSys;
using System.Xml.Linq;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Data;
using System.Windows.Controls.Primitives;

namespace ApplicationUP.Pages
{
    public partial class PageListUser : Page, PageLoadUpdate
    {
        // Объявление интерфейса коллекции на основе класса Товаров
        // Ининциализация коллекции на основе класса
        IList<User>
            UserList = new List<User>();

        PageZakazUser PageZakaz;
        // Конструктор
        public PageListUser()
        {
            InitializeComponent();

            // Коллекция товаров используется как источник данных для ItemsControl
            UserData.ItemsSource = UserList;
            // Вызов метода загрузки данных
            Load();
        }

        public async void Load()
        {
            // Список товаров опусташается


            // Обновление данных колекции
            //  GoodsData.Items.Refresh();

            // Вызов метода загрузки данных
            await LoadGoods();

            // Обновление данных колекции

            UserData.Items.Refresh();

        }

        public void ReloadData()
        {
            UserList.Clear();
            Load();
        }

        private async Task LoadGoods()
        {
            Connector
                conn = new Connector();
            string
                sql = "SELECT * FROM пользователи WHERE role_id=2";

            MySqlCommand
                cmd = new MySqlCommand(sql, conn.GetConn());
            await conn.GetOpen();
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                // Список товаров опусташается
                UserList.Clear();
                // Асинхронное отключение от БД
                await conn.GetClose();
                // Возращение false
                return;
            }

            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {
                // Добавление элемента в коллекцию списка товаров на основе класса (Экземпляр класс создается - объект)
                UserList.Add(new User()
                {
                    Login = reader["login"].ToString(),
                    Parol = reader["parol"].ToString(),
                    Numaber_tel = Convert.ToString(reader["numaber_tel"]),
                    Name = Convert.ToString(reader["name"]),
                });




            }

            // Асинхронное отключение от БД
            await conn.GetClose();
            UserData.ItemsSource = UserList;
            // Возращение true
            return;
        }

        private bool IsValidText(TextBox box)
        {
            return string.IsNullOrEmpty(box.Text) || box.Text.Length < 1;
        }

        private void openkuserlistzakaz(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border boreder && boreder.DataContext is User user)
            {
                PageZakaz = new PageZakazUser(user.Login);
                NavigationService.Navigate(PageZakaz);
            }

        }

        private async void btndelete(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.DataContext is User g)
            {
                if (MessageBox.Show(
                    string.Format("Вы действительно собираетесь удалить профиль пользователя - {0}", g.Name),
                    "Внимание!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                    ) != MessageBoxResult.Yes)
                    return;

                if (!await UserDeleteSQL(g.Login))
                {
                    MessageBox.Show("Данный профиль не удален", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("Данный профиль удален", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                CleanListData(g.Login);
            }
        }

        private async Task<bool> UserDeleteSQL(string login)

        {
            string
                sql = "DELETE FROM пользователи WHERE login=@login";

            Connector
                conn = new Connector();

            MySqlCommand
                cmd = new MySqlCommand(sql, conn.GetConn());

            cmd.Parameters.Add(new MySqlParameter("@login", login));

            await conn.GetOpen();

            if (await cmd.ExecuteNonQueryAsync() == 1)
            {
                await conn.GetClose();
                return true;
            }

            await conn.GetClose();
            return false;
        }

        private void CleanListData(string login)
        {
            foreach (User user in UserList)
            {
                if (user.Login != login)
                    continue;

                UserList.Remove(user);
                UserData.Items.Refresh();
                return;
            }
        }
    }
}
