using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
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
            tbTimer.Visibility = Visibility.Hidden;
            tbLeftTime.Visibility = Visibility.Hidden;
        }

        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null));
        }

        private void GenerateCaptcha()
        {
            click++;
            tblCaptcha.Text = CaptchaGenerator.GenerateCaptchaText(6);
            tblCaptcha.TextDecorations = TextDecorations.Strikethrough;
            tbCaptcha.Visibility = Visibility.Visible;
            tblCaptcha.Visibility = Visibility.Visible;
        }
        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text.Trim();
            string password = Hash.HashPassword(tbPassword.Password.Trim());

            MusicRecordEntities db = Helper.GetContext();
            var user = db.UserAccounts .FirstOrDefault(x => x.username == login && x.password == password);


            if (click <= 1)
            {
                if (user == null)
                {
                    MessageBox.Show("Вы ввели логин или пароль неверно!");
                    tbPassword.Clear();
                    tbLogin.Clear();
                    GenerateCaptcha();
                }
                else
                {
                    string roleName = user.Roles.role_name;
                    MessageBox.Show($"Вы вошли под: {roleName}");
                    tbLogin.Clear();
                    tbPassword.Clear();
                    tbCaptcha.Clear();
                    LoadPage(user);

                }
            }
            else if (click == 2)
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
            else if(click == 3)
            {
                MessageBox.Show("Вы три раза неверно ввели данные!");
                click = 0;
                btnEnter.IsEnabled = false;
                btnEnterGuests.IsEnabled = false;
                StartTimer();
            }
        }

        private int remainingSeconds = 10;
        private DispatcherTimer timer;
        private void StartTimer()
        {
            remainingSeconds = 10;
            tbCaptcha.Clear();
            tbCaptcha.Visibility = Visibility.Hidden;
            tblCaptcha.Visibility = Visibility.Hidden;
            tbPassword.Clear();
            tbLogin.Clear();
            tbPassword.IsEnabled = false;
            tbLogin.IsEnabled = false;
            tbTimer.Visibility = Visibility.Visible;
            tbLeftTime.Visibility = Visibility.Visible;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tbTimer.Text = remainingSeconds.ToString();
            remainingSeconds--;
            if (remainingSeconds < 0)
            {
                timer.Stop();
                tbTimer.Text = "";
                tbPassword.IsEnabled = true;
                tbLogin.IsEnabled = true;
                tbTimer.Visibility = Visibility.Hidden;
                tbLeftTime.Visibility = Visibility.Hidden;
                btnEnter.IsEnabled = true;
                btnEnterGuests.IsEnabled = true;
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
