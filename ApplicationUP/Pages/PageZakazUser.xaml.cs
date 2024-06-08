using ApplicationUP.Core;
using ApplicationUP.Template;
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
    /// Логика взаимодействия для PageZakazUser.xaml
    /// </summary>
    public partial class PageZakazUser : Page
    {
        public string Login;
        IList<ZakazUser>
            zakazlist = new List<ZakazUser>();

        public PageZakazUser(string login)
        {
            InitializeComponent();
            Login = login;
            Load(Login);
        }
        public async void Load(string login_user)
        {
            // Список товаров опусташается
            zakazlist.Clear();
            // Обновление данных колекции
            zakazlistData.Items.Refresh();

            // Вызов метода загрузки данных
            await LoadZakaz(login_user);

            if (zakazlist.Count==0)
            {
                zakazlistData.Visibility = Visibility.Collapsed;
                NoItemsZakazVisible.Visibility = Visibility.Visible;
            }
            else
            {
                zakazlistData.Visibility = Visibility.Visible;
                NoItemsZakazVisible.Visibility = Visibility.Collapsed;
            }

            await LoadUser(login_user);

            // Обновление данных колекции
            zakazlistData.Items.Refresh();
        }
        public void ReloadData()
        {
            Load(Login);
        }
        private async Task LoadUser(string login_user)
        {
            // Строка запроса
            string
                sql = "SELECT * FROM пользователи where login=@login_user AND role_id=2";

            // Объявление переменной на основе класс подключения:
            // >    Connector conn
            // Инициализация переменной:
            // >    = new Connector()

            Connector
                conn = new Connector();

            // Объявление объекта команды:
            // >    MySqlCommand cmd
            // Инициализация объекта команды:
            // >    new MySqlCommand(sql, conn.GetConn());
            MySqlCommand
                cmd = new MySqlCommand(sql, conn.GetConn());
            cmd.Parameters.Add(new MySqlParameter("@login_user", login_user));
            // Асинхронное подключение к БД
            await conn.GetOpen();

            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                // Список товаров опусташается
                zakazlist.Clear();
                // Асинхронное отключение от БД
                await conn.GetClose();
                // Возращение false
                return;
            }
            var date1 = DateTime.Now.ToString();
            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {

                TextBlockName.Text = string.Format("Логин: "+ reader["login"].ToString());
                TextBlockLogin.Text = string.Format("Имя: " + reader["name"].ToString());
                TextBlockNumaber_tel.Text = string.Format("Номер телефона: " + reader["numaber_tel"].ToString());


            }

            // Асинхронное отключение от БД
            await conn.GetClose();
            zakazlistData.ItemsSource = zakazlist;
            // Возращение true
            return;
        }
        private async Task LoadZakaz(string login_user)
        {
            // Строка запроса
            string
                sql = "SELECT * FROM заказы where login_user=@login_user";

            // Объявление переменной на основе класс подключения:
            // >    Connector conn
            // Инициализация переменной:
            // >    = new Connector()

            Connector
                conn = new Connector();

            // Объявление объекта команды:
            // >    MySqlCommand cmd
            // Инициализация объекта команды:
            // >    new MySqlCommand(sql, conn.GetConn());
            MySqlCommand
                cmd = new MySqlCommand(sql, conn.GetConn());
            cmd.Parameters.Add(new MySqlParameter("@login_user", login_user));
            // Асинхронное подключение к БД
            await conn.GetOpen();

            // Объявление и инициалзиация метода асинрхонного чтения данных из бд
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();

            // Проверка, что строк нет
            if (!reader.HasRows)
            {
                // Список товаров опусташается
                zakazlist.Clear();
                // Асинхронное отключение от БД
                await conn.GetClose();
                // Возращение false
                return;
            }
            var date1 = DateTime.Now.ToString();
            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {
                // Добавление элемента в коллекцию списка товаров на основе класса (Экземпляр класс создается - объект)
                zakazlist.Add(new ZakazUser()
                {
                    id_zakaza = Convert.ToInt32(reader["id_zakaza"]),
                    loginuser = Convert.ToString(reader["login_user"]),
                    name_tovara = Convert.ToString(reader["name_tovara"]),
                    img_tavara = Convert.ToString(reader["img_tavara"]),
                    price_tovara = Convert.ToSingle(reader["price_tovara"]),
                    date = Convert.ToDateTime(reader["date_zakaza"]),
                    count = Convert.ToInt32(reader["count"]),
                    status = Convert.ToString(reader["status"]),
                    itogovaya_summa = Convert.ToSingle(reader["itogovaya_summa"]),
                    adress_dostavki = Convert.ToString(reader["adress_dostavki"]),
                });


                // Обновление данных колекции
                //zakazlistData.Items.Refresh();
                // Время ожидания (Время "Сна")

            }

            // Асинхронное отключение от БД
            await conn.GetClose();
            zakazlistData.ItemsSource = zakazlist;
            // Возращение true
            return;
        }

        private void MenuGoToPageExit(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
