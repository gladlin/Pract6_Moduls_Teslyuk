using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using Практическая_3.Models;
using Практическая_3.Services;
namespace Практическая_3.Pages
{
    public partial class AddUser : Page
    {
        string PhotoPath;

        public Producers Null { get; private set; }

        public AddUser(ObservableCollection<userStruct> userList)
        {

            InitializeComponent();
            tblPassport.Visibility = Visibility.Collapsed;
            tbPassport.Visibility = Visibility.Collapsed;
            tbPhoneNumber.Visibility = Visibility.Collapsed;
            tblPhoneNumber.Visibility = Visibility.Collapsed;
            tblProducer.Visibility = Visibility.Collapsed;
            cbProducer.Visibility = Visibility.Collapsed;
            foreach(var user in userList)
            {
                if (user.Role == "Продюсер")

                    cbProducer.Items.Add(user.Name.Split()[1]);
            }
       
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbSurname.Text) || string.IsNullOrEmpty(tbFirstname.Text) ||
                string.IsNullOrEmpty(tbMiddleName.Text) || string.IsNullOrEmpty(tbEmail.Text) ||
                string.IsNullOrEmpty(cbRole.Text) || string.IsNullOrEmpty(tbLogin.Text) ||
                string.IsNullOrEmpty(tbPassword.Text))
            {
                MessageBox.Show("Заполните все обязательные поля!");
                return;
            }
            var db = Helper.GetContext();
            int maxId = db.UserAccounts.OrderByDescending(u => u.account_id).First().account_id + 1;
            var newUser = new UserAccounts
            {
                username = tbLogin.Text,
                password = Hash.HashPassword(tbPassword.Text),
                email = tbEmail.Text,
                role_id = GetRoleId(cbRole.Text),
                account_id = maxId,
            };

            var context = new ValidationContext(newUser);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if (!Validator.TryValidateObject(newUser, context, (ICollection<System.ComponentModel.DataAnnotations.ValidationResult>)results, true))
            {
                MessageBox.Show("Не получилось создать пользователя, введите корретные данные");
                return
            }
            db.UserAccounts.Add(newUser);
            db.SaveChanges();
            int IdInRole;
            int isCreated = 0;
            switch (cbRole.Text)
            {
                case "Админ":
                    IdInRole = db.Admins.OrderByDescending(u => u.admin_id).First().admin_id + 1;
                    var admin = AddAdmin(maxId, IdInRole);
                    if (admin != null)
                    {
                        db.Admins.Add(admin);
                        isCreated = 1;
                    }
                    break;
                case "Продюсер":
                    IdInRole = db.Producers.OrderByDescending(u => u.producer_id).First().producer_id + 1;
                    var producer = AddProducer(maxId, IdInRole);
                    if (producer != null)
                    { 
                        db.Producers.Add(producer);
                        isCreated = 1;
                    }
                    break;
                case "Артист":
                    IdInRole = db.Artists.OrderByDescending(u => u.artist_id).First().artist_id + 1;
                    var artist = AddArtist(maxId, IdInRole);
                    if (artist != null)
                    { 
                        db.Artists.Add(artist);
                        isCreated = 1;
                     }
                     break;
                default:
                    MessageBox.Show("Неверная роль!");
                    return;
            }
            try
            {
                db.SaveChanges();
                if (isCreated == 1)
                MessageBox.Show("Пользователь успешно создан!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении пользователя: {ex.Message}");
            }
        }

        private Admins AddAdmin(int maxId, int IdInRole)
        {
            if (string.IsNullOrEmpty(tbPassport.Text) || string.IsNullOrEmpty(tbPhoneNumber.Text))
            {
                MessageBox.Show("Заполните все обязательные поля!");
                return null;
            }
            var admin = new Admins
            {
                first_name = tbFirstname.Text,
                last_name = tbSurname.Text,
                middle_name = tbMiddleName.Text,
                photo_path = PhotoPath,
                admin_id = IdInRole,
                account_id = maxId,
            };
            var context = new ValidationContext(admin);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if (Validator.TryValidateObject(admin, context, results, true))
                return admin;
            else
            {
                MessageBox.Show($"Ошибка при сохранении пользователя: {results}");
                return null;
            }
        }

        private Artists AddArtist(int maxId, int IdInRole)
        {
            if (string.IsNullOrEmpty(tbPassport.Text) || string.IsNullOrEmpty(tbPhoneNumber.Text))
            {
                MessageBox.Show("Заполните все обязательные поля!");
                return null;
            }
            var artist = new Artists
            {
                first_name = tbFirstname.Text,
                last_name = tbSurname.Text,
                middle_name = tbMiddleName.Text,
                photo_path = PhotoPath,
                artist_id = IdInRole,
                account_id = maxId,
                producer_id = cbProducer.SelectedIndex + 1,
                phone_number = tbPhoneNumber.Text,
                passport_number = tbPassport.Text
            };
            var context = new ValidationContext(artist);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if (Validator.TryValidateObject(artist, context, results, true))
                return artist;
            else
            {
                MessageBox.Show($"Ошибка при сохранении пользователя: {results}");
                return null;
            }
        }

        private Producers AddProducer(int maxId, int IdInRole)
        {
            if (string.IsNullOrEmpty(tbPassport.Text) || string.IsNullOrEmpty(tbPhoneNumber.Text))
            {
                MessageBox.Show("Заполните все обязательные поля!");
                return null;
            }
            var producer = new Producers
            {
                first_name = tbFirstname.Text,
                last_name = tbSurname.Text,
                middle_name = tbMiddleName.Text,
                photo_path = PhotoPath,
                account_id = maxId,
                producer_id= IdInRole,
                phone_number = tbPhoneNumber.Text,
                passport_number = tbPassport.Text,
            };

            var context = new ValidationContext(producer);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if (Validator.TryValidateObject(producer, context, results, true))
                return producer;
            else
            {
                MessageBox.Show($"Ошибка при сохранении пользователя: {results}");
                return null;
            }
        }

        private int GetRoleId(string roleName)
        {
            switch (roleName)
            {
                case "Админ":
                    return 1;
                case "Продюсер":
                    return 2;
                case "Артист":
                    return 3;
                default:
                    return -1;
            }
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbSurname.Text = "";
            tbFirstname.Text = "";
            tbMiddleName.Text = "";
            tbEmail.Text = "";
            cbRole.Text = "";
            tbLogin.Text = "";
            tbPassword.Text = "";
            imgUser.Source = null;
            PhotoPath = null;
            tbPassport.Text = "";
            tbPhoneNumber.Text = "";
            cbProducer.Text = "";
        }
        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.png; *.jpg; *.jpeg)|*.png;*.jpg;*.jpeg";
            dialog.DefaultExt = ".png";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string filePath = dialog.FileName;
                if (File.Exists(filePath))
                {
                    try
                    {
                        filePath.Replace("\\", "\\\\");
                        imgUser.Source = new BitmapImage(new Uri(filePath));
                        PhotoPath = filePath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Файл не найден!");
                }
            }
        }

        private void cbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedRole = cbRole.SelectedIndex;

            switch (selectedRole)
            {
                case 0:
                    tblPassport.Visibility = Visibility.Collapsed;
                    tbPassport.Visibility = Visibility.Collapsed;
                    tbPhoneNumber.Visibility = Visibility.Collapsed;
                    tblPhoneNumber.Visibility = Visibility.Collapsed;
                    tblProducer.Visibility = Visibility.Collapsed;
                    cbProducer.Visibility = Visibility.Collapsed;
                    break;

                case 1:
                    tblPassport.Visibility = Visibility.Visible;
                    tbPassport.Visibility = Visibility.Visible;
                    tbPhoneNumber.Visibility = Visibility.Visible;
                    tblPhoneNumber.Visibility = Visibility.Visible;
                    tblProducer.Visibility = Visibility.Collapsed;
                    cbProducer.Visibility = Visibility.Collapsed;
                    break;

                case 2:
                    tblPassport.Visibility = Visibility.Visible;
                    tbPassport.Visibility = Visibility.Visible;
                    tbPhoneNumber.Visibility = Visibility.Visible;
                    tblPhoneNumber.Visibility = Visibility.Visible;
                    tblProducer.Visibility = Visibility.Visible;
                    cbProducer.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}