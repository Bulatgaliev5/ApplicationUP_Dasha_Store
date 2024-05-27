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
using static ApplicationUP.Core.CoreSys;

namespace ApplicationUP.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageListKorzina.xaml
    /// </summary>
    /// 
    public partial class PageListZakazAdmin : Page
    {
        IList<Zakaz>
              zakazlist = new List<Zakaz>();
        MainWindow
            window;
        RoleGroup
            role;

        public PageListZakazAdmin(MainWindow main, string name, int role_id, string role_name)
        {
            InitializeComponent();

            window = main;
            role = (RoleGroup)role_id;
            // Коллекция товаров используется как источник данных для ItemsControl
            zakazlistData.ItemsSource = zakazlist;
            // Вызов метода загрузки данных
            Load();
        }

        public async void Load()
        {
            // Список товаров опусташается
            zakazlist.Clear();
            // Обновление данных колекции
            zakazlistData.Items.Refresh();

            // Вызов метода загрузки данных
            await LoadGoods();

            // Обновление данных колекции
            zakazlistData.Items.Refresh();
        }
        public void ReloadData()
        {
            Load();
        }

        private async Task LoadGoods()
        {
            // Строка запроса
            string
                sql = "SELECT * FROM заказы";

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
                zakazlist.Add(new Zakaz()
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
                    number_documenta = Convert.ToString(reader["number_documenta"]),

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

        private void UpdateStatus(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.DataContext is Zakaz g)
            {
                PageUpdateStatus pageUpdateStatus = new PageUpdateStatus(g.id_zakaza);
                this.Opacity = 0.5;
                pageUpdateStatus.ShowDialog();
                Load();
                pageUpdateStatus.Focus();
                this.Opacity = 1;
               
            }
        }
    }
}
