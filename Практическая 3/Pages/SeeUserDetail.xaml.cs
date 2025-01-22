using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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

        public SeeUserDetail(userStruct user, ObservableCollection<userStruct> userList)
        {
            InitializeComponent();
            currentUser = user;
            fillAllData(user, userList);
        }

        private void fillAllData(userStruct user, ObservableCollection<userStruct> userList)
        {
            tbSurname.Text = user.Name.Split(' ')[1];
            tbFirstname.Text = user.Name.Split(' ')[0];
            tbMiddleName.Text = user.Name.Split(' ')[2];
            tbEmail.Text = user.Email;
            tbRole.Text = user.Role.ToString();

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
            tbRole.Text = "";
            tbLogin.Text = "";
            tbPassword.Text = "";
            imgUser.Source = null;
            newPhotoPath = null;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        var db = Helper.GetContext();
        var userdb = db.UserAccounts.FirstOrDefault(x => x.email == currentUser.Email);
           
        if (userdb != null)
        {
            userdb.username = tbLogin.Text;
            userdb.password = tbPassword.Text;
            userdb.email = tbEmail.Text;
            var context = new ValidationContext(userdb);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if(!Validator.TryValidateObject(userdb, context, results, true))
            {
                foreach (var reason in results)
                    MessageBox.Show($"{reason}");
                    return;
             }

            if (userdb.role_id == 1)
            {
                var admin = db.Admins.FirstOrDefault(x => x.account_id == userdb.account_id);
                    
                if (admin != null)
                {
                    admin.first_name = tbFirstname.Text;
                    admin.last_name = tbSurname.Text;
                    admin.middle_name = tbMiddleName.Text;
                    admin.photo_path = newPhotoPath ?? admin.photo_path;
                    context = new ValidationContext(admin);
                    var results1 = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                    if (Validator.TryValidateObject(admin, context, results1, true))
                    {
                        db.SaveChanges();
                        MessageBox.Show("Пользователь сохранен");
                        return;
                    }
                    else
                    {
                        foreach (var reason in results1)
                            MessageBox.Show($"{reason}");
                    }
                }
                else
                    MessageBox.Show("Не получилось обновить пользователя");

            }
            else if (userdb.role_id == 2)
            {
                var producer = db.Producers.FirstOrDefault(x => x.account_id == userdb.account_id);
                    

                if (producer != null)
                {
                    producer.first_name = tbFirstname.Text;
                    producer.last_name = tbSurname.Text;
                    producer.middle_name = tbMiddleName.Text;
                    producer.photo_path = newPhotoPath ?? producer.photo_path;
                    context = new ValidationContext(producer);
                    var results1 = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                    if (Validator.TryValidateObject(producer, context, results1, true))
                    {
                        db.SaveChanges();
                        MessageBox.Show("Пользователь сохранен");
                        return;
                    }
                    else
                    {
                        foreach (var reason in results1)
                            MessageBox.Show($"{reason}");
                    }
                }
                else
                        MessageBox.Show("Не получилось обновить пользователя");
                }
                
            else if (userdb.role_id == 3)
            {
                var artist = db.Artists.FirstOrDefault(x => x.account_id == userdb.account_id);
                if (artist != null)
                {
                    artist.first_name = tbFirstname.Text;
                    artist.last_name = tbSurname.Text;
                    artist.middle_name = tbMiddleName.Text;
                    artist.photo_path = newPhotoPath ?? artist.photo_path;
                    context = new ValidationContext(artist);
                    var results1 = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                    if (Validator.TryValidateObject(artist, context, results1, true))
                    {
                        db.SaveChanges();
                        MessageBox.Show("Пользователь сохранен");
                        return;
                    }
                    else
                    {
                        foreach (var reason in results1)
                            MessageBox.Show($"{reason}");
                    }
                }
                else
                    MessageBox.Show("Не получилось создать пользователя");
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}