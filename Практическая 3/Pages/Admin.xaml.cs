using System;
using System.Collections.ObjectModel;
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
        public string PicturePath { get; set; }
    }
    
    public partial class Admin : Page
    {
        private ObservableCollection<userStruct> userList;
        private ObservableCollection<userStruct> userListSearch;

        public Admin(UserAccounts user, string userName, string userSurname)
        {
            InitializeComponent();
            printGreeting(userName, userSurname);

            userList = new ObservableCollection<userStruct>();
            userListSearch = new ObservableCollection<userStruct>();
            GetAllUsers();

            LViewProduct.ItemsSource = userListSearch;
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
                string photo_path;

                if (user.role_id == 1)
                {
                    var admin = db.Admins.FirstOrDefault(x => x.account_id == user.account_id);
                    if (admin == null) continue;
                    userName = admin.first_name;
                    userSurname = admin.last_name;
                    userMiddlename = admin.middle_name;
                    photo_path = admin.photo_path;
                    role = "Админ";
                }
                else if (user.role_id == 2)
                {
                    var producer = db.Producers.FirstOrDefault(x => x.account_id == user.account_id);
                    if (producer == null) continue;
                    userName = producer.first_name;
                    userSurname = producer.last_name;
                    userMiddlename = producer.middle_name;
                    photo_path = producer.photo_path;
                    role = "Продюсер";
                }
                else
                {
                    var artist = db.Artists.FirstOrDefault(x => x.account_id == user.account_id);
                    if (artist == null) continue;
                    userName = artist.first_name;
                    userSurname = artist.last_name;
                    userMiddlename = artist.middle_name;
                    photo_path = artist.photo_path;
                    role = "Артист";
                }

                userStruct userStruct = new userStruct
                {
                    Name = $"{userName} {userSurname} {userMiddlename}",
                    Role = role,
                    Email = user.email,
                    PicturePath = photo_path,
                };

                userList.Add(userStruct);
            }

            userListSearch = new ObservableCollection<userStruct>(userList);
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

        private void btnSeeUser_Click(object sender, RoutedEventArgs e)
        {
            if (LViewProduct.SelectedItem is userStruct selectedUser)
            {
                NavigationService.Navigate(new SeeUserDetail(selectedUser));
            }
        }

        private void tbSearchSurname_Changed(object sender, EventArgs e)
        {
            ChangeListUsers();
        }

        private void ChangeListUsers()
        {
            userListSearch.Clear();

            foreach (var user in userList)
            {
                bool matchesSearch = string.IsNullOrEmpty(tbSearchSurname.Text) || user.Name.Split(' ')[1].Contains(tbSearchSurname.Text);
                bool matchesRole = string.IsNullOrEmpty(cbRole.Text) || cbRole.Text == user.Role;

                if (matchesSearch && matchesRole)
                {
                    userListSearch.Add(user);
                }
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddUser());
        }

        private void cbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeListUsers();
        }
    }
}