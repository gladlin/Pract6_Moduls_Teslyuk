using System;
using System.Data;
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
        MusicRecordEntities db = Helper.GetContext();
        private string userName = null;
        private string userSurname = null;
        private int specialID = 0;

        public Autho()
        {
            InitializeComponent();
            click = 0;
            ClearAll();
            tbCaptcha.Visibility = Visibility.Hidden;
            tblCaptcha.Visibility = Visibility.Hidden;
            tbTimer.Visibility = Visibility.Hidden;
            tbLeftTime.Visibility = Visibility.Hidden;
        }

        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null, "", ""));
            ClearAll();
        }

        private void GenerateCaptcha()
        {
            tblCaptcha.Text = CaptchaGenerator.GenerateCaptchaText(6);
            tblCaptcha.TextDecorations = TextDecorations.Strikethrough;
            tbCaptcha.Visibility = Visibility.Visible;
            tblCaptcha.Visibility = Visibility.Visible;
        }

        private void GetName(UserAccounts user, int role)
        {

             if (role == 1)
             {
                 var admin = db.Admins.FirstOrDefault(x => x.account_id == user.account_id);
                 specialID = admin.admin_id;
                 userName = admin.first_name;
                 userSurname = admin.last_name;
             }
             else if (role == 2)
             {
                 var producer = db.Producers.FirstOrDefault(x => x.account_id == user.account_id);
                 specialID = producer.producer_id;
                 userName = producer.first_name;
                 userSurname = producer.last_name;
             }
            else if (role == 3)
            {
                var artist = db.Artists.FirstOrDefault(x => x.account_id == user.account_id);
                specialID = artist.artist_id;
                userName = artist.first_name;
                userSurname = artist.last_name;
            }
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            click++;
            string login = tbLogin.Text.Trim();
            string password = Hash.HashPassword(tbPassword.Password.Trim());
            var user = db.UserAccounts.FirstOrDefault(x => x.username == login && x.password == password); 
            /*  поиск первой попавшейся записи в таблице "UserAccounts", у которой в поле "username" значение равно в переменной login,
                а также значение в поле "password" у записи равно значению в переменной password
                Если нет записи, подходящей под условие, то возвращается значение null
             */

            if (click <= 1)
            {
                if (user == null)
                {
                    MessageBox.Show("Вы ввели логин или пароль неверно!");
                    tbPassword.Clear();
                    tbLogin.Clear();
                    tbCaptcha.Clear();
                    GenerateCaptcha();
                }
                else
                {
                    string roleName = user.Roles.role_name;
                    LoadPage(user);

                }
            }
            else if (click <= 3)
            {
                if (user != null && tbCaptcha.Text.Trim() == tblCaptcha.Text)
                {
                    string roleName = user.Roles.role_name;
                    LoadPage(user);
                }
                else
                {
                    MessageBox.Show("Пройдите капчу заново!");
                    tbCaptcha.Clear();
                    GenerateCaptcha();
                }
            }
            else if(click == 4)
            {
                if (user != null && tbCaptcha.Text.Trim() == tblCaptcha.Text)
                {
                    string roleName = user.Roles.role_name;
                    LoadPage(user);
                }
                else
                {
                    MessageBox.Show("Вы три раза неверно ввели данные!");
                    click = 0;
                    btnEnter.IsEnabled = false;
                    btnEnterGuests.IsEnabled = false;
                    StartTimer();
                }
            }
        }

        private int remainingSeconds = 10;
        private DispatcherTimer timer;

        // Таймер ожидания, из-за неверной введенной капчи
        // Не дает пользователю действовать с системой, дод тех пор пока не закончится таймер
        private void StartTimer()
        {
            remainingSeconds = 10;
            ClearAll();
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

        private void ClearAll()
        {

            tbLogin.Clear();
            tbPassword.Clear();
            tbCaptcha.Visibility = Visibility.Hidden;
            tblCaptcha.Visibility = Visibility.Hidden;
            tbCaptcha.Clear();
        }


        private void LoadPage(UserAccounts user)
        {
            ClearAll();
            click = 0;
            GetName(user, user.role_id);
            TimeSpan userTime = (DateTime.Now.TimeOfDay);
            TimeSpan morning = new TimeSpan(10, 0, 0);
            TimeSpan deepEvening = new TimeSpan(19, 0, 0);
            if (userTime < morning || userTime > deepEvening)
            {
                MessageBox.Show("Неподходящее время для работы");
            }

            else if (userName != null && userSurname != null)
            {
                switch (user.role_id)
                {
                    case 1:
                        NavigationService.Navigate(new Admin(user, userName, userSurname));
                        break;
                    case 2:
                        NavigationService.Navigate(new Client(user, userName, userSurname));
                        break;
                    case 3:
                        NavigationService.Navigate(new Albums(specialID));
                        break;
                    default:
                        MessageBox.Show("Неизвестная роль пользователя.");
                        break;
                }
            }

        }

        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ForgotPassword());
        }
    }
}
