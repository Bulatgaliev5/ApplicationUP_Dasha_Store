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
        string
            Loginuser;
      


        // Конструктор
        public PageListGoods(MainWindow main, string name, int role_id, string role_name,string loginuser)
        {
            InitializeComponent();
            window = main;
            role = (RoleGroup)role_id;
            Loginuser = loginuser;
            // Коллекция товаров используется как источник данных для ItemsControl
            GoodsData.ItemsSource = goods;
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

            GoodsData.Items.Refresh();
          
        }

        public void ReloadData()
        {
            goods.Clear();
            Load();
        }

        private async Task LoadGoods()
        {

            Connector
                conn = new Connector();
            // Строка запроса
            string
                sql = "ПолучитьТоварыВНаличии";

            // Объявление переменной на основе класс подключения:
            // >    Connector conn
            // Инициализация переменной:
            // >    = new Connector()


            // Объявление объекта команды:
            // >    MySqlCommand cmd
            // Инициализация объекта команды:
            // >    new MySqlCommand(sql, conn.GetConn());
            MySqlCommand
                cmd = new MySqlCommand(sql, conn.GetConn());
            cmd.CommandType = CommandType.StoredProcedure;

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
                    CanVisible = TypeRoleGet(role) ? Visibility.Visible : Visibility.Collapsed,
                    CanZakaz = TypeRoleGet(role),
                    CanZakazVisible = TypeRoleGet(role) ? Visibility.Collapsed : Visibility.Visible,
                    count = 1
                }) ;

                // Обновление данных колекции
               // GoodsData.Items.Refresh();
                // Время ожидания (Время "Сна")
                
            }

            // Асинхронное отключение от БД
            await conn.GetClose();
                  GoodsData.ItemsSource = goods;
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

      


 

        private void addzakazat(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border b && b.DataContext is Goods g)
                window.GoToPageOformlenie(g.ID, Loginuser, g.count);

         
        }

        public void plus(object sender, MouseButtonEventArgs e)
        {

            if (sender is Border b && b.DataContext is Goods g)
            {
                Label countLabel = b.FindName("CountLab") as Label;
                if (g.count < g.CountInContainer) // проверка, чтобы count не стал отрицательным
                {
                    g.count++;
                    countLabel.Content = g.count;
                }
            }
        }

        public void minus(object sender, MouseButtonEventArgs e)
        {

            if (sender is Border b && b.DataContext is Goods g)
            {
                Label countLabel = b.FindName("CountLab") as Label;
                if (g.count > 0) // проверка, чтобы count не стал отрицательным
                {
                    g.count--;
                    countLabel.Content = g.count;
                }

            }
        }

        private void btnSearch(object sender, RoutedEventArgs e)
        {
           
        }

        private bool IsValidText(TextBox box)
        {
            return string.IsNullOrEmpty(box.Text) || box.Text.Length < 1;
        }

        private void pricepoubiv(object sender, RoutedEventArgs e)
        {
            
            GoodsData.ItemsSource = goods.OrderByDescending(a => a.Cost).ToList();
        }

        private void pricedefault(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void pricepovos(object sender, RoutedEventArgs e)
        {
            GoodsData.ItemsSource = goods.OrderBy(a => a.Cost).ToList();
        }

        private void tc(object sender, TextChangedEventArgs e)
        {
            var result = goods.Where(a => a.Name.ToLower().StartsWith(tb1.Text.ToLower()));
            GoodsData.ItemsSource = null;
            GoodsData.ItemsSource = result;
        }
    }
}
