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

using Практическая_3.Models;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Практическая_3.Pages
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Page
    {
        public Client(UserAccounts user, string userName, string userSurname)
        {
            InitializeComponent();
            printGreeting(userName, userSurname);
        }
        private void printGreeting(string userName, string userSurname)
        {
            TimeSpan userTime = (DateTime.Now.TimeOfDay);
            TimeSpan morning = new TimeSpan(10, 0, 0);
            TimeSpan day = new TimeSpan(12, 0, 0);
            TimeSpan evening = new TimeSpan(17, 0, 0);
            TimeSpan deepEvening = new TimeSpan(19, 0, 0);

            if (userTime >= morning && userTime <= day)
            {
                tbGreeting.Text = "Доброе утро!";

            }
            else if (userTime >= day && userTime <= evening)
            {
                tbGreeting.Text = "Добрый день!";
            }
            else if (userTime >= evening && userTime <= deepEvening)
            {
                tbGreeting.Text = "Добрый вечер!";
            }
            tbUserName.Text = userSurname + " " + userName;
        }
    }
}
