using ApplicationUP.Core;
using ApplicationUP.Template;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class PageKartochaGoods : Page
    {
        IList<Reviews>
              reviews = new List<Reviews>();
        private int
                 ID, Count;
        private string
                 UserLogin;
        private string imageSourcelink;
        PageOformirovanie pageOformirovanie;
        MainWindow Win;
        RoleGroup role;
        public PageKartochaGoods(MainWindow win, RoleGroup Role_id, int ID_Item, string userLogin, int count, float rating, int reviews_count)
        {
            InitializeComponent();
            TextBlockRating.Text = rating.ToString();
            TextBlockReviews_count.Text = string.Format(reviews_count.ToString() + " отзыва");
            ID = ID_Item;
            UserLogin = userLogin;
            Win = win;
            Count = count;
            InputQantity.Text = string.Format(count.ToString() + " шт.");
            ReviewsData.ItemsSource = reviews;
            role = Role_id;
            switch (Role_id)
            {
                case RoleGroup.Admin:
                    btnzakazat.Visibility = Visibility.Collapsed;
                    break;
                case RoleGroup.User:
                    btnzakazat.Visibility = Visibility.Visible;
                    break;

            }

            Load();
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
        private async void Load()
        {
            NoReviews.Visibility = Visibility.Collapsed;
            await LoadGoods();
            bool res = await LoadReviews();
            if (!res)
            {
                NoReviews.Visibility = Visibility.Visible;
                ReviewsData.Visibility = Visibility.Collapsed;
            }
            ReviewsData.Items.Refresh();
        }


        private async Task LoadGoods()
        {
            string
                sql = "SELECT * FROM товары where id_tovar = @id";

            Connector
                conn = new Connector();

            MySqlCommand
                cmd = new MySqlCommand(sql, conn.GetConn());

            cmd.Parameters.Add(new MySqlParameter("@id", ID));
            await conn.GetOpen();

            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows)
            {
                await conn.GetClose();
                return;
            }

            while (await reader.ReadAsync())
            {

                InputName.Text = reader["name"].ToString();
                InputDesc.Text = reader["Desc"].ToString();
                InputCost.Text = string.Format(reader["Cost"].ToString() + " рублей");
                InputDiscount.Text = string.Format(reader["Discount"].ToString() + " рублей");
                imageSourcelink = reader["Img"].ToString();
                InputCount.Text = string.Format(reader["V_nalichii"].ToString()+" шт.");
                InputMaterial.Text = reader["Material"].ToString();
                InputKachestvo.Text = reader["Kachestvo"].ToString();
                break;
            }
            
            ImageSource imageSource = new BitmapImage(new Uri(imageSourcelink));
            InputImg.Source = imageSource;
            await conn.GetClose();
            return;
        }
        private async Task<bool> LoadReviews()
        {
            string
                sql = "SELECT * FROM отзывы о " +
                "JOIN пользователи п ON п.login = о.login_user " +
                "JOIN заказы з ON о.id_zakaza = з.id_zakaza " +
                "WHERE з.id_tovara = @id";

            Connector
                conn = new Connector();

            MySqlCommand
                cmd = new MySqlCommand(sql, conn.GetConn());

            cmd.Parameters.Add(new MySqlParameter("@id", ID));
            await conn.GetOpen();
             
            MySqlDataReader
                 reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows)
            {
                await conn.GetClose();
                return false;
            }

            while (await reader.ReadAsync())
            {

                reviews.Add(new Reviews()
                {
                    NameUser = reader["name"].ToString(),
                    Comment = reader["comment"].ToString(),
                    Date_insert = Convert.ToDateTime(reader["Date_insert"]),
                    ozenka = Convert.ToInt32(reader["ozenka"]),

                });
            }

            await conn.GetClose();
            ReviewsData.ItemsSource = reviews;
            return true;
        }


        private bool IsValidText(TextBox box)
        {
            return string.IsNullOrEmpty(box.Text) || box.Text.Length < 1;
        }





        private void IsValueNumberChech(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, @"[^0-9]$");
        }

        private void MenuGoToPageExit(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void addzakazat(object sender, MouseButtonEventArgs e)
        {

                pageOformirovanie = new PageOformirovanie(Win, ID, UserLogin, Count);
                NavigationService.Navigate(pageOformirovanie);
            
           
        }

        private void plus(object sender, MouseButtonEventArgs e)
        {

        }

        private void minus(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
