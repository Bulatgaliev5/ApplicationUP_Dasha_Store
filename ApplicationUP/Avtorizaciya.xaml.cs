using ApplicationUP;
using ApplicationUP.Core;
using MySqlConnector;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace AplicationUP.Windows
{
    /// <summary>
    /// Логика взаимодействия для Avtorizaciya.xaml
    /// </summary>
    public partial class Avtorizaciya : Window
    {

        int Role_id;
        string
            name,
            Role_name,
            UserLogin;
        public Avtorizaciya()
        {
            InitializeComponent();
        }

        private async void CheckData()
        {



            ////  MessageBox.Show(string.Format("{0} - {1} - {2}", name, role_id, role_name));
            //MainWindow main = new MainWindow(this, name, Role_id, Role_name, UserLogin);

            //loginTBox.Text = "";
            //passTBox.Password = "";
            //this.Hide();
            //main.Show();
        }

        private void Zaregestrirovatsa(object sender, RoutedEventArgs e)
        {

        }

        private void Vhod(object sender, RoutedEventArgs e)
        {
        
        }


    }
}
