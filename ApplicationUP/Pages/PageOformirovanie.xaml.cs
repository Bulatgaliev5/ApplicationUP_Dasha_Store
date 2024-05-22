using ApplicationUP.Core;
using ApplicationUP.Template;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
using Xceed.Wpf.AvalonDock.Themes;

namespace ApplicationUP.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageUpdateGoods.xaml
    /// </summary>
    public partial class PageOformirovanie : Page
    {
        private int 
            ID;
        MainWindow
            window;
        decimal price_tovara, itogovaya_summa;
        int id_tovara;
        string image;
        string user_login;
        int Count;
        public PageOformirovanie(MainWindow main, int ID_Item, string UserLogin, int count)
        {
            InitializeComponent();
            ID = ID_Item;
            window = main;
            user_login = UserLogin;
            Load();
            Count = count;
        }

        private async void Load()
        {
            await LoadGoods();
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

                InputName.Content = reader["name"].ToString();
                countlabel.Content = Count;
                id_tovara = Convert.ToInt32(reader["id_tovar"]);
                InputCost.Content = reader["Cost"].ToString();
                image = reader["Img"].ToString();
                price_tovara = Convert.ToDecimal(reader["Cost"]);



                string imagePath = image; // Замените на путь к вашему изображению
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmapImage.EndInit();
                ImageImg.Source = bitmapImage;

                Itog_Summa();

                break;
            }

            await conn.GetClose();
            return;
        }



        public void Itog_Summa()
        {
            itogovaya_summa = Count * price_tovara;
        }

        private async Task<bool> zakazatSQL()
        {
            if (IsValidText(TextBoxAdressDostavki))
            {
                TextBoxAdressDostavki.Focusable = true;
                return false;
            }
            else if (!CardNumberTextBox.IsMaskFull)
            {
                CardNumberTextBox.Focusable = true;
                return false;
            }
            else if (!CardNumberTextBox.IsMaskFull)
            {
                DateTextBox.Focusable = true;
                return false;
            }
            else if (!CardNumberTextBox.IsMaskFull)
            {
                CVVTextBox.Focusable = true;
                return false;
            }
            DateTime date_zakaza = DateTime.Now;
            string
            sql =
            "INSERT INTO zakazi " +
            "(login_user, name_tovara, img_tavara, price_tovara, date_zakaza, id_tovara, adress_dostavki, count, number_card,itogovaya_summa,status) " +
            "VALUES (@login_user, @name_tovara, @img_tavara, @price_tovara, @date_zakaza, @id_tovara, @adress_dostavki, @count, @number_card,@itogovaya_summa,@status)";
            Connector
            conn = new Connector();
            MySqlCommand
            cmd = new MySqlCommand(sql, conn.GetConn());
            string status = "Заказ в обработке";

            cmd.Parameters.Add(new MySqlParameter("@id_tovara", id_tovara));
            cmd.Parameters.Add(new MySqlParameter("@name_tovara", InputName.Content));
            cmd.Parameters.Add(new MySqlParameter("@img_tavara", image));
            cmd.Parameters.Add(new MySqlParameter("@adress_dostavki", TextBoxAdressDostavki.Text));
            cmd.Parameters.Add(new MySqlParameter("@price_tovara", price_tovara));
            cmd.Parameters.Add(new MySqlParameter("@date_zakaza", date_zakaza));
            cmd.Parameters.Add(new MySqlParameter("@login_user", user_login));
            cmd.Parameters.Add(new MySqlParameter("@count", Count));
            cmd.Parameters.Add(new MySqlParameter("@number_card", CardNumberTextBox.Text));
            cmd.Parameters.Add(new MySqlParameter("@itogovaya_summa", itogovaya_summa));
            cmd.Parameters.Add(new MySqlParameter("@status", status));


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

      


        private void IsValueNumberChech(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, @"[^0-9]$");
        }

        private void Maskavvoda(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
                MessageBox.Show("Введите число!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

 

        private async void btnzakazat(object sender, MouseButtonEventArgs e)
        {
            bool
               status = await zakazatSQL(); 

            if (status)
            {

                MessageBox.Show("заказ принят", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                window.GoToPageGoodsList();
            }

            else
                MessageBox.Show("заказ не принят. Заполните все поля", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Information);

            window.GoToPageOformlenie(ID, user_login, Count);
            
        }
    }
}
