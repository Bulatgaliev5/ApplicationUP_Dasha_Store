using ApplicationUP.Core;
using MySqlConnector;
using System;
using System.Collections.Generic;
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

namespace ApplicationUP.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageUpdateGoods.xaml
    /// </summary>
    public partial class PageUpdateGoods : Page
    {
        private int 
            ID;
        MainWindow
            window;

        public PageUpdateGoods(MainWindow main, int ID_Item)
        {
            InitializeComponent();
            ID = ID_Item;
            window = main;
            Load();
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
                
                InputName.Text = reader["name"].ToString();
                InputDesc.Text = reader["Desc"].ToString();
                InputCost.Text = reader["Cost"].ToString();
                InputDiscount.Text = reader["Discount"].ToString();
                InputImg.Text = reader["Img"].ToString();
                InputCount.Text = reader["V_nalichii"].ToString();
                
                break;
            }

            await conn.GetClose();
            return;
        }

        private async Task<bool> UpdateGoods()
        {

            if (IsValidText(InputName))
            {
                InputName.Focusable = true;
                return false;
            }
            else if (IsValidText(InputDesc))
            {
                InputDesc.Focusable = true;
                return false;
            }
            else if (IsValidText(InputCost))
            {
                InputCost.Focusable = true;
                return false;
            }
            else if (IsValidText(InputDiscount))
            {
                InputDiscount.Focusable = true;
                return false;
            }
            else if (IsValidText(InputCount))
            {
                InputCount.Focusable = true;
                return false;
            }

            string
                sql = 
                "UPDATE `товары` " +
                "SET `name`=@1, `Desc`=@2, `Cost`=@3, `Discount`=@4, `Img`=@5, `V_nalichii`=@6 " +
                "WHERE `id_tovar`=@id;";

            Connector
                conn = new Connector();

            MySqlCommand
                cmd = new MySqlCommand(sql, conn.GetConn());

            cmd.Parameters.Add(new MySqlParameter("@1", InputName.Text));
            cmd.Parameters.Add(new MySqlParameter("@2", InputDesc.Text));
            cmd.Parameters.Add(new MySqlParameter("@3", InputCost.Text));
            cmd.Parameters.Add(new MySqlParameter("@4", InputDiscount.Text));
            cmd.Parameters.Add(new MySqlParameter("@5", InputImg.Text));
            cmd.Parameters.Add(new MySqlParameter("@6", InputCount.Text));
            cmd.Parameters.Add(new MySqlParameter("@id", ID));

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

        private async void UpdateGoods(object sender, MouseButtonEventArgs e)
        {
            bool 
                status = await UpdateGoods();

            if (status)
                MessageBox.Show("Данный товар обновлен", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
            else 
                MessageBox.Show("Данный товар не обновлен", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);

            window.GoToPageGoodsList();
        }

        private void CanselPage(object sender, MouseButtonEventArgs e)
        {
            window.GoToPageGoodsList();
        }

        private void IsValueNumberChech(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, @"[^0-9]$");
        }
    }
}
