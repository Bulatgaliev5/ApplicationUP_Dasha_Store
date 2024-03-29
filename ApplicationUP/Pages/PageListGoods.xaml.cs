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

namespace ApplicationUP.Pages
{
    public partial class PageListGoods : Page, PageLoadUpdate
    {
        // Объявление интерфейса коллекции на основе класса Товаров
        // Ининциализация коллекции на основе класса
        IList<Goods>
            goods = new List<Goods>();
        MainWindow
            window;
        RoleGroup
            role;

        // Конструктор
        public PageListGoods(MainWindow main, string name, int role_id, string role_name)
        {
            InitializeComponent();
            window = main;
            role = (RoleGroup)role_id;

            // Коллекция товаров используется как источник данных для ItemsControl
            GoodsData.ItemsSource = goods;
            // Вызов метода загрузки данных
            Load();
        }

        public async void Load()
        {
            // Список товаров опусташается
            goods.Clear();
            // Обновление данных колекции
            GoodsData.Items.Refresh();

            // Вызов метода загрузки данных
            await LoadGoods();

            // Обновление данных колекции
            GoodsData.Items.Refresh();
        }

        public void ReloadData()
        {
            Load();
        }

        private async Task LoadGoods()
        {
            // Строка запроса
            string
                sql = "SELECT * FROM товары";

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
                goods.Clear();
                // Асинхронное отключение от БД
                await conn.GetClose();
                // Возращение false
                return;
            }

            // Цикл while выполняется, пока есть строки для чтения из БД
            while (await reader.ReadAsync())
            {
                // Добавление элемента в коллекцию списка товаров на основе класса (Экземпляр класс создается - объект)
                goods.Add(new Goods()
                {
                    ID = Convert.ToInt32(reader["id_tovar"]),
                    Name = reader["name"].ToString(),
                    Desc = reader["Desc"].ToString(),
                    Discount = Convert.ToInt32(reader["Discount"]),
                    Cost = Convert.ToSingle(reader["Cost"]),
                    CountInContainer = Convert.ToInt32(reader["V_nalichii"]),
                    IMG = reader["Img"].ToString(),
                    CanEdit = TypeRoleGet(role),
                    CanVisible = TypeRoleGet(role) ? Visibility.Visible : Visibility.Collapsed
                });

                // Обновление данных колекции
                GoodsData.Items.Refresh();
                // Время ожидания (Время "Сна")
                await Task.Delay(250);
            }

            // Асинхронное отключение от БД
            await conn.GetClose();
            // Возращение true
            return;
        }

        private bool TypeRoleGet(RoleGroup group)
        {
            switch (group)
            {
                case RoleGroup.Admin:
                    return true;
                default:
                    return false;
            }
        }


        private async void GoodsDelete(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.DataContext is Goods g)
            {
                if (MessageBox.Show(
                    string.Format("Вы действительно собираетесь удалить товар - {0}", g.Name),
                    "Внимание!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                    ) != MessageBoxResult.Yes)
                    return;

                if (!await GoodsDeleteSQL(g.ID))
                {
                    MessageBox.Show("Данный товар не удален", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("Данный товар удален", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                CleanListData(g.ID);
            }
        }

        private async Task<bool> GoodsDeleteSQL(int id)
        
        {
            string
                sql = "DELETE FROM товары WHERE id_tovar=@id";

            Connector
                conn = new Connector();

            MySqlCommand
                cmd = new MySqlCommand(sql, conn.GetConn());

            cmd.Parameters.Add(new MySqlParameter("@id", id));

            await conn.GetOpen();

            if (await cmd.ExecuteNonQueryAsync() == 1)
            {
                await conn.GetClose();
                return true;
            }

            await conn.GetClose();
            return false;
        }

        private void CleanListData(int id)
        {
            foreach (Goods good in goods)
            {
                if (good.ID != id)
                    continue;

                goods.Remove(good);
                GoodsData.Items.Refresh();
                return;
            }
        }

        private void GoodsEdit(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.DataContext is Goods g)
                window.GoToPageGoodsUpdate(g.ID);
        }
    }
}
