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
    public partial class ForgotPassword : Page
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void btnSendCode_Click(object sender, RoutedEventArgs e)
        {
            var db = Helper.GetContext();
            var user = db.UserAccounts.FirstOrDefault(x => x.email == tbPassLogin.Text || x.username == tbPassLogin.Text);
            if (user != null)
            {
                MailService sendEmail = new MailService();
                Random rnd = new Random();
                int sendcode = rnd.Next(1000, 9999);
                bool res = sendEmail.SendPasswordResetEmail(user.email, sendcode);
                if (res)
                {
                    tbPassLogin.Clear();
                    NavigationService.Navigate(new ConfirmEmail(user.email, sendcode));
                }
                else
                {
                    MessageBox.Show("Ошибка при отправке письма, проверьте валидность почты");
                    tbPassLogin.Clear();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Пользователя с данным логином или почтой не существует");
                tbPassLogin.Clear();
                return;
            }

        }
    }
}
