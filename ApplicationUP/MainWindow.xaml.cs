using ApplicationUP.Pages;
using ApplicationUP.Template;
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

namespace ApplicationUP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PageAddGoods
            pageGoods;
        PageListGoods
            pageListGoods;
        PageUpdateGoods
            pageUpdateGoods;
        PageListZakaz
            pageListZakaz;
        PageListZakazAdmin
            pageListZakazAdmin;
        PageOformirovanie
            pageOformirovanie;
        PageUser
             pageUser;
        PageListUser
             pageListUser;


        public MainWindow(string name, int role_id, string role_name, string UserLogin)
        {
            InitializeComponent();

            pageListUser = new PageListUser();
            pageUser = new PageUser(this, UserLogin);
            pageGoods = new PageAddGoods(name, role_id, role_name);
            pageListGoods = new PageListGoods(this, name, role_id, role_name, UserLogin);
            pageListZakaz = new PageListZakaz(this, name, role_id, role_name, UserLogin);
            pageListZakazAdmin = new PageListZakazAdmin(this, name, role_id, role_name);

            switch ((RoleGroup)role_id)
            {
                case RoleGroup.Admin:
                    ButtonCreate.Visibility = Visibility.Visible;
                    BtnKorzina.Visibility = Visibility.Collapsed;
                    BtnZakazAdmin.Visibility = Visibility.Visible;
                    BtnListUser.Visibility = Visibility.Visible;
                    break;
                default:
                    ButtonCreate.Visibility = Visibility.Collapsed;
                    BtnKorzina.Visibility = Visibility.Visible;
                    BtnZakazAdmin.Visibility = Visibility.Collapsed;
                    BtnListUser.Visibility = Visibility.Collapsed;
                    break;
            }

            LabelName.Text = name;
            LabelRole.Text = role_name;



        }

        /// <summary>
        /// Метод обработки события клика перехода на стрраницу добавления товара
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuGoToPageAddGoods(object sender, MouseButtonEventArgs e)
        {
            Frame1.NavigationService.Navigate(pageGoods);
            pageGoods.ReloadData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuGoToPageListGoods(object sender, MouseButtonEventArgs e)
        {
            Frame1.NavigationService.Navigate(pageListGoods);
            pageListGoods.ReloadData();
        }

        public void GoToPageGoodsList()
        {
            Frame1.NavigationService.Navigate(pageListGoods);
            pageListGoods.ReloadData();
        }

        public void GoToPageGoodsUpdate(int ID)
        {
            pageUpdateGoods = new PageUpdateGoods(this, ID);
            Frame1.NavigationService.Navigate(pageUpdateGoods);
            pageListGoods.ReloadData();
        }


        public void GoToPageOformlenie(int ID, string UserLogin, int count)
        {
            pageOformirovanie = new PageOformirovanie(this, ID, UserLogin, count);
            Frame1.NavigationService.Navigate(pageOformirovanie);
            Frame1.NavigationService.RemoveBackEntry();
            // pageOformirovanie.ReloadData();
        }
        public void PageOformlenieClose()
        {
            Frame1.NavigationService.RemoveBackEntry();


            // pageOformirovanie.ReloadData();
        }

        public void UpdateUser(string username)
        {
            LabelName.Text= username;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitToLogin(object sender, MouseButtonEventArgs e)
        {
           
            Close();
        }

        private void MenuGoToPageListKorzina(object sender, MouseButtonEventArgs e)
        {

          
            Frame1.NavigationService.Navigate(pageListZakaz);
            pageListZakaz.ReloadData();
        }

        private void MenuGoToPageListZakazAdmin(object sender, MouseButtonEventArgs e)
        {
            Frame1.NavigationService.Navigate(pageListZakazAdmin);
            pageListZakazAdmin.ReloadData();
        }

        private void MenuGoToPageOpenUser(object sender, MouseButtonEventArgs e)
        {
            Frame1.NavigationService.Navigate(pageUser);
            pageUser.ReloadData();


        }

        private void MenuGoToPageListUser(object sender, MouseButtonEventArgs e)
        {
            Frame1.NavigationService.Navigate(pageListUser);
            pageListUser.ReloadData();
        }
    }
}
