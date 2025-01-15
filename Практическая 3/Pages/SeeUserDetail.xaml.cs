using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Практическая_3.Models;
using Практическая_3.Services;

namespace Практическая_3.Pages
{
    public partial class SeeUserDetail : Page
    {
        private userStruct currentUser;
        private string newPhotoPath;

        public SeeUserDetail(userStruct user)
        {
            InitializeComponent();
            currentUser = user;
            fillAllData(user);
        }

        private void fillAllData(userStruct user)
        {
            tbSurname.Text = user.Name.Split(' ')[1];
            tbFirstname.Text = user.Name.Split(' ')[0];
            tbMiddleName.Text = user.Name.Split(' ')[2];
            tbEmail.Text = user.Email;
            cbRole.Text = user.Role.ToString();

            var db = Helper.GetContext();
            var userdb = db.UserAccounts.FirstOrDefault(x => x.email == user.Email);
            tbLogin.Text = userdb.username;
            tbPassword.Text = userdb.password;
            imgUser.Source = new BitmapImage(new Uri(user.PicturePath));
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
            newPhotoPath = null;
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
            var userdb = db.UserAccounts.FirstOrDefault(x => x.email == currentUser.Email);
            if (userdb != null)
            {
                userdb.username = tbLogin.Text;
                userdb.password = tbPassword.Text;
                userdb.email = tbEmail.Text;

                if (cbRole.Text == "Админ")
                {
                    var admin = db.Admins.FirstOrDefault(x => x.account_id == userdb.account_id);
                    if (admin != null)
                    {
                        admin.first_name = tbFirstname.Text;
                        admin.last_name = tbSurname.Text;
                        admin.middle_name = tbMiddleName.Text;
                        admin.photo_path = newPhotoPath ?? admin.photo_path;
                    }
                }
                else if (cbRole.Text == "Продюсер")
                {
                    var producer = db.Producers.FirstOrDefault(x => x.account_id == userdb.account_id);
                    if (producer != null)
                    {
                        producer.first_name = tbFirstname.Text;
                        producer.last_name = tbSurname.Text;
                        producer.middle_name = tbMiddleName.Text;
                        producer.photo_path = newPhotoPath ?? producer.photo_path;
                    }
                }
                else if (cbRole.Text == "Артист")
                {
                    var artist = db.Artists.FirstOrDefault(x => x.account_id == userdb.account_id);
                    if (artist != null)
                    {
                        artist.first_name = tbFirstname.Text;
                        artist.last_name = tbSurname.Text;
                        artist.middle_name = tbMiddleName.Text;
                        artist.photo_path = newPhotoPath ?? artist.photo_path;
                    }
                }

                try
                {
                    db.SaveChanges();
                    MessageBox.Show("Данные успешно сохранены!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}");
                }
            }
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
                        newPhotoPath = filePath;
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