using ApplicationUP.Core;
using ApplicationUP.Template;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ApplicationUP.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageUpdateStatus.xaml
    /// </summary>
    public partial class PageUpdateStatus : Window
    {
        MainWindow
                window;
        public PageUpdateStatus(int selectorder)
        {

            InitializeComponent();
            TextBoxstatuslist.Items.Add("Заказ принят");
            TextBoxstatuslist.Items.Add("Заказ оформлен");
            TextBoxstatuslist.Items.Add("Заказ собирается");
            TextBoxstatuslist.Items.Add("Заказ отправлен");
            TextBoxstatuslist.Items.Add("Заказ в пути");
            TextBoxstatuslist.Items.Add("Заказ доставлен");
            Load(selectorder);

        }
        private async Task Load(int selectorder)
        {
          await LoadZakazStatus(selectorder);
        }
        public int _id_zakaza;
        public int id_zakaza
        {
            get => _id_zakaza;
            set => _id_zakaza = value;
        }
        private string _status;


        public string status
        {
            get => _status;
            set => _status = value;
        }



        private async Task<bool> LoadZakazStatus(int selectorder)
        {
            id_zakaza = selectorder;
            Connector con = new Connector();
            string
                  sql = "SELECT * FROM zakazi WHERE id_zakaza = @id_zakaza";



            MySqlCommand
                cmd = new MySqlCommand(sql, con.GetConn());
            cmd.Parameters.Add(new MySqlParameter("@id_zakaza", selectorder));


            await con.GetOpen();
            MySqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows)
            {

                await con.GetClose();

                return false;

            }


            while (await reader.ReadAsync())
            {

                status = Convert.ToString(reader["status"]);
            }



            await con.GetClose();

            return true;


        }
        private async Task<bool> UpdateStatusZakazaSQL()
        {

            Connector con = new Connector();
            string
                  sql = "UPDATE заказы SET `status`=@status WHERE `id_zakaza`=@id_zakaza";



            MySqlCommand
                cmd = new MySqlCommand(sql, con.GetConn());
            cmd.Parameters.Add(new MySqlParameter("@id_zakaza", id_zakaza));
            cmd.Parameters.Add(new MySqlParameter("@status", TextBoxstatuslist.SelectedValue));

            await con.GetOpen();
           await cmd.ExecuteNonQueryAsync();
            await con.GetClose();

            return true;


        }
        private async void SaveStatus(object sender, MouseButtonEventArgs e)
        {
            if (TextBoxstatuslist.SelectedValue!=null)
            {
                bool res = await UpdateStatusZakazaSQL();
                if (res)
                    MessageBox.Show("Статус заказа обновлен", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Статус заказа не обновлен", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();

            }
            else
            {
                MessageBox.Show("Выберите статус заказа", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void TextBoxstatuslist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
