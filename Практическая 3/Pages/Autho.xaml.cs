using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Практическая_3.Models;
using Практическая_3.Services;

namespace Практическая_3.Pages
{
    public partial class Autho : Page
    {
        int click;

        public Autho()
        {
            InitializeComponent();
            click = 0;
            tbCaptcha.Visibility = Visibility.Hidden;
            tblCaptcha.Visibility = Visibility.Hidden;
        }

        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null));
        }

        private void GenerateCaptcha()
        {
            tblCaptcha.Text = CaptchaGenerator.GenerateCaptchaText(6);
            tblCaptcha.TextDecorations = TextDecorations.Strikethrough;
            tbCaptcha.Visibility = Visibility.Visible;
            tblCaptcha.Visibility = Visibility.Visible;
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            click++;
            string login = tbLogin.Text.Trim();
            string password = Hash.HashPassword(tbPassword.Password.Trim());

            MusicRecordEntities db = Helper.GetContext();
            var user = db.UserAccounts .FirstOrDefault(x => x.username == login && x.password == password);

            if (click == 1)
            {
                if (user == null)
                {
                    MessageBox.Show("Вы ввели логин или пароль неверно!");
                    tbPassword.Clear();
                    tbLogin.Clear();
                }
                else
                {
                    GenerateCaptcha();
                }
            }
            else if (click > 1)
            {
                if (user != null && tbCaptcha.Text.Trim() == tblCaptcha.Text)
                {
                    string roleName = user.Roles.role_name;
                    MessageBox.Show($"Вы вошли под: {roleName}");
                    tbLogin.Clear();
                    tbPassword.Clear();
                    tbCaptcha.Clear();
                    tblCaptcha.Visibility = Visibility.Hidden;
                    tbCaptcha.Visibility = Visibility.Hidden;
                    LoadPage(user);
                }
                else
                {
                    MessageBox.Show("Пройдите капчу заново!");
                    tbCaptcha.Clear();
                    GenerateCaptcha();
                }
            }
        }

        private void LoadPage(UserAccounts user)
        {
            click = 0;
            switch (user.role_id)
            {
                case 1:
                    NavigationService.Navigate(new Admin());
                    break;
                case 2:
                case 3:
                    NavigationService.Navigate(new Client(user));
                    break;
                default:
                    MessageBox.Show("Неизвестная роль пользователя.");
                    break;
            }
        }
    }
}
