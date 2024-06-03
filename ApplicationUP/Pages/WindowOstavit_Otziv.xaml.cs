using ApplicationUP.Core;
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
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.Primitives;

namespace ApplicationUP.Pages
{
    /// <summary>
    /// Логика взаимодействия для WindowOstavit_Otziv.xaml
    /// </summary>
    public partial class WindowOstavit_Otziv : Window
    {
        string LoginUser;
        int ozenka;
        int ID_zakaza;
        public WindowOstavit_Otziv(string login, int id_zakaza)
        {
            InitializeComponent();
            InputOzenka.Items.Add(1);
            InputOzenka.Items.Add(2);
            InputOzenka.Items.Add(3);
            InputOzenka.Items.Add(4);
            InputOzenka.Items.Add(5);
            LoginUser = login;
            ID_zakaza = id_zakaza;

        }


        private async Task<bool> AddReviews(string loginuser, int id_zakaza)
        {

            if (IsValidText(InputComment))
            {
                InputComment.Focusable = true;
                return false;
            }
            else if(ozenka==0)
            {
                InputOzenka.Focusable = true;
                return false;
            }

            var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string
                sql = "INSERT INTO `отзывы` (`comment`, `ozenka`, `id_zakaza`, `login_user`, `Date_insert`) " +
                "VALUES (@comment, @ozenka, @id_zakaza, @loginuser, @date);";

            Connector
                conn = new Connector();

            MySqlCommand
                cmd = new MySqlCommand(sql, conn.GetConn());

            cmd.Parameters.Add(new MySqlParameter("@comment", InputComment.Text));
            cmd.Parameters.Add(new MySqlParameter("@ozenka", ozenka));
            cmd.Parameters.Add(new MySqlParameter("@id_zakaza", id_zakaza));
            cmd.Parameters.Add(new MySqlParameter("@loginuser", loginuser));
            cmd.Parameters.Add(new MySqlParameter("@date", date));

            await conn.GetOpen();

            if (await cmd.ExecuteNonQueryAsync() == 1)
            {

                MessageBox.Show("Данный отзыв добавлен", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                await conn.GetClose();
                this.Close();
                return true;
            }

            MessageBox.Show("Данный отзыв не добавлен", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            await conn.GetClose();
            return false;
        }


        private bool IsValidText(TextBox box)
        {
            return string.IsNullOrEmpty(box.Text) || box.Text.Length < 1;
        }

        private void SelectOzenka(object sender, SelectionChangedEventArgs e)
        {
            if (InputOzenka.SelectedValue!=null) // если не равно null
            {
                ozenka = Convert.ToInt32( InputOzenka.SelectedValue);
            }
        }

        private async void BtnOstavitOtziv(object sender, MouseButtonEventArgs e)
        {
            bool res = await AddReviews(LoginUser, ID_zakaza);
            if (!res)
            {
                MessageBox.Show("Заполните все поля", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
