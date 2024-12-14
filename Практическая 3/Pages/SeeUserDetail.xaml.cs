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
using Практическая_3.Services;

namespace Практическая_3.Pages
{
    /// <summary>
    /// Логика взаимодействия для SeeUserDetail.xaml
    /// </summary>
    /// 


    public partial class SeeUserDetail : Page
    {
        public SeeUserDetail(userStruct user)
        {
            InitializeComponent();
            fillAllData(user);
        }

        private void fillAllData(userStruct user)
        {
            tbSurname.Text = user.Name.Split(' ')[1];
            tbFirstname.Text = user.Name.Split(' ')[0];
            tblMiddleName.Text = user.Name.Split(' ')[2];
            tblEmail.Text = user.Email;
            tblRole.Text = user.Role;
            var db = Helper.GetContext();
            var userdb = db.UserAccounts.FirstOrDefault(x => x.email == user.Email);
            tbLogin.Text = userdb.username;
            tbPassword.Text = userdb.password;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            tbSurname.Text = "";
            tbFirstname.Text = "";
            tblMiddleName.Text = "";
            tblEmail.Text = "";
            tblRole.Text = "";
            tbLogin.Text = "";
            tbPassword.Text = "";
        }

        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
