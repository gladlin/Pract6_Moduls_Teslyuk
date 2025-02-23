using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Практическая_3.Services;

namespace Практическая_3.Pages
{
    public partial class ConfirmEmail : Page
    {
        private int SendCode;
        private string Email;
        private static int trys = 3;
        public ConfirmEmail(string mail, int sendCode)
        {
            InitializeComponent();
            SendCode = sendCode;
            Email = mail;
            btnAccept.Visibility = Visibility.Collapsed;
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (trys == 0)
            {
                trys = 3;
                MessageBox.Show("Вы израсходовали все попытки, попробуйте заново");
                NavigationService.GoBack();
            }
            int userCode = 0;
            bool result = Int32.TryParse(tbUserCode.Text, out userCode);
            if (result && userCode == SendCode)
            {
                trys = 3;
                lbMessage.Content = "Введите новый пароль в поле ниже:";
                btnCheck.Visibility = Visibility.Collapsed;
                btnAccept.Visibility = Visibility.Visible;
                tblLeftTry.Visibility = Visibility.Collapsed;
                tbUserCode.Clear();
                MessageBox.Show("Подтверждение почты прошло успешно!");
            }
            else
            {
                trys--;
                if (trys == 0)
                {
                    trys = 3;
                    MessageBox.Show("Вы израсходовали все попытки, попробуйте заново");
                    NavigationService.GoBack();

                }
                else
                {
                    MessageBox.Show($"Неверный код подтверждения, осталось {trys} попыток");
                    tblLeftTry.Text = $"Осталось попыток: {trys}";
                }

            }
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            var db = Helper.GetContext();
            var user = db.UserAccounts.FirstOrDefault(x => x.email == Email);
            if (user != null) 
            {
                user.password = Hash.HashPassword(tbUserCode.Text);
                try
                {
                    db.SaveChanges();
                    MessageBox.Show("Успешное изменение данных!");
                    NavigationService.Navigate(new Autho());
                }
                catch (Exception ex) 
                { 
                    MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}");
                    tbUserCode.Clear();
                }
            }
        }
    }
}
