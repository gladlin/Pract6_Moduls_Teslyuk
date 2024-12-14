using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Практическая_3.Models;
using Практическая_3.Services;

namespace Практическая_3.Pages
{
    public struct userStruct
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }

    public partial class Admin : Page
    {
        private List<userStruct> userList;

        public Admin(UserAccounts user, string userName, string userSurname)
        {
            InitializeComponent();
            printGreeting(userName, userSurname);

            userList = new List<userStruct>();
            GetAllUsers();

            LViewProduct.ItemsSource = userList;
        }

        private void GetAllUsers()
        {
            var db = Helper.GetContext();
            var users = db.UserAccounts.ToList();

            foreach (var user in users)
            {
                string userName;
                string userSurname;
                string userMiddlename;
                string role;

                if (user.role_id == 1)
                {
                    var admin = db.Admins.FirstOrDefault(x => x.account_id == user.account_id);
                    if (admin == null) continue;
                    userName = admin.first_name;
                    userSurname = admin.last_name;
                    userMiddlename = admin.middle_name;
                    role = "Admin";
                }
                else if (user.role_id == 2)
                {
                    var producer = db.Producers.FirstOrDefault(x => x.account_id == user.account_id);
                    if (producer == null) continue;
                    userName = producer.first_name;
                    userSurname = producer.last_name;
                    userMiddlename = producer.middle_name;
                    role = "Producer";
                }
                else
                {
                    var artist = db.Artists.FirstOrDefault(x => x.account_id == user.account_id);
                    if (artist == null) continue;
                    userName = artist.first_name;
                    userSurname = artist.last_name;
                    userMiddlename = artist.middle_name;
                    role = "Artist";
                }

                userStruct userStruct = new userStruct
                {
                    Name = $"{userName} {userSurname} {userMiddlename}",
                    Role = role,
                    Email = user.email
                };

                userList.Add(userStruct);
            }
        }

        private void printGreeting(string userName, string userSurname)
        {
            TimeSpan userTime = DateTime.Now.TimeOfDay;
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
            tbUserName.Text = $"{userSurname} {userName}";
        }

        private void LViewProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnSeeUser_Click(object sender,RoutedEventArgs e)
        {
            object user = LViewProduct.SelectedItem.ToString();
            //NavigationService.Navigate(new SeeUserDetail(selectedItem));
            //надо как-то найти способ узнать выбранный элемент
        }
    }
}
