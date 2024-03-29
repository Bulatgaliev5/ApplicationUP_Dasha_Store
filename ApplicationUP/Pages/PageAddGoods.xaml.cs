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
    /// Логика взаимодействия для PageAddGoods.xaml
    /// </summary>
    public partial class PageAddGoods : Page, PageLoadUpdate
    {
        public PageAddGoods(string name, int role_id, string role_name)
        {
            InitializeComponent();
        }

        public void ReloadData()
        {

        }

        private async void CreateNewGoods(object sender, MouseButtonEventArgs e)
        {
            await CreateElement();
        }

        private async Task<bool> CreateElement()
        {

            if (IsValidText(InputName))
            {
                InputName.Focusable = true;
                return false;
            }
            else if (IsValidText(InputName))
            {
                InputDesc.Focusable = true;
                return false;
            }
            else if (IsValidText(InputName))
            {
                InputCost.Focusable = true;
                return false;
            }
            else if (IsValidText(InputName))
            {
                InputDiscount.Focusable = true;
                return false;
            }
            else if (IsValidText(InputName))
            {
                InputCount.Focusable = true;
                return false;
            }

            string
                sql = "INSERT INTO `товары` (`name`, `Desc`, `Cost`, `Discount`, `Img`, `V_nalichii`) VALUES (@1, @2, @3, @4, @5, @6);";

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

            await conn.GetOpen();

            if (await cmd.ExecuteNonQueryAsync() == 1)
            {

                MessageBox.Show("Данный товар добавлен", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                await conn.GetClose();
                return true;
            }

            MessageBox.Show("Данный товар не добавлен", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
