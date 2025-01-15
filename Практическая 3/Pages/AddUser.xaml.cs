using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Практическая_3.Models;
using Практическая_3.Services;

namespace Практическая_3.Pages
{
    public partial class AddUser : Page
    {
        string PhotoPath;
        public AddUser()
        {
            InitializeComponent();
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
            db.UserAccounts.Add(newUser);
            db.SaveChanges();
            int IdInRole;
            switch (cbRole.Text)
            {
                case "Админ":
                    IdInRole = db.Admins.OrderByDescending(u => u.admin_id).First().admin_id + 1;
                    var admin = new Admins
                    {
                        first_name = tbFirstname.Text,
                        last_name = tbSurname.Text,
                        middle_name = tbMiddleName.Text,
                        photo_path = PhotoPath,
                        admin_id = IdInRole,
                        account_id = maxId,
                    };
                    db.Admins.Add(admin);
                    break;

                case "Продюсер":
                    IdInRole = db.Producers.OrderByDescending(u => u.producer_id).First().producer_id + 1;
                    var producer = new Producers
                    {
                        first_name = tbFirstname.Text,
                        last_name = tbSurname.Text,
                        middle_name = tbMiddleName.Text,
                        photo_path = PhotoPath,
                        producer_id = IdInRole,
                        account_id = maxId,

                    };
                    db.Producers.Add(producer);
                    break;

                case "Артист":
                    IdInRole = db.Artists.OrderByDescending(u => u.artist_id).First().artist_id + 1;
                    var artist = new Artists
                    {
                        first_name = tbFirstname.Text,
                        last_name = tbSurname.Text,
                        middle_name = tbMiddleName.Text,
                        photo_path = PhotoPath,
                        artist_id = IdInRole,
                        account_id = maxId,
                    };
                    db.Artists.Add(artist);
                    break;

                default:
                    MessageBox.Show("Неверная роль!");
                    return;
            }
            try
            {
                db.SaveChanges();
                MessageBox.Show("Пользователь успешно создан!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении пользователя: {ex.Message}");
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
    }
}