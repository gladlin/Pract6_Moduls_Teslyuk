using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Xceed.Words.NET;
using Практическая_3.Models;
using Практическая_3.Services;
using static System.Net.Mime.MediaTypeNames;
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
            var db = Helper.GetContext();
            int maxId = db.UserAccounts.OrderByDescending(u => u.account_id).First().account_id + 1;
            /*
             Получение id последней записи в таблице, после прибавляем 1, 
             чтобы получить account_id для новой записи, создаваемой далее
             */
            var newUser = new UserAccounts
            {
                username = tbLogin.Text,
                password = Hash.HashPassword(tbPassword.Text),
                email = tbEmail.Text,
                role_id = GetRoleId(cbRole.Text),
                account_id = maxId,
            };


            // проверка того, что все поля в созданной структуре соответсвуют всем необходимым ограничениям
            // нужно для корректного сохранения пользователя в БД.
            var context = new ValidationContext(newUser);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if (!Validator.TryValidateObject(newUser, context, (ICollection<System.ComponentModel.DataAnnotations.ValidationResult>)results, true))
            {
                StringBuilder sb = new StringBuilder();
                foreach (var reason in results)
                {
                    sb.AppendLine(reason.ToString());
                }
                MessageBox.Show(sb.ToString());
                return;
            }
            db.UserAccounts.Add(newUser);
            db.SaveChanges();
            int IdInRole;
            int isCreated = 0;

            switch (cbRole.Text)
            {
                case "Админ":
                    IdInRole = db.Admins.OrderByDescending(u => u.admin_id).First().admin_id + 1;
                    var admin = AddAdmin(maxId, IdInRole, tbFirstname.Text, tbSurname.Text, tbMiddleName.Text, PhotoPath);
                    if (admin != null)
                    {
                        db.Admins.Add(admin);
                        isCreated = 1;
                    }
                   
                    break;
                case "Продюсер":
                    IdInRole = db.Producers.OrderByDescending(u => u.producer_id).First().producer_id + 1;
                    var producer = AddProducer(maxId, IdInRole, tbFirstname.Text, tbSurname.Text, tbMiddleName.Text, PhotoPath, tbPhoneNumber.Text, tbPassport.Text);
                    if (producer != null)
                    { 
                        db.Producers.Add(producer);
                        isCreated = 1;
                    }
              
                    break;
                case "Артист":
                    IdInRole = db.Artists.OrderByDescending(u => u.artist_id).First().artist_id + 1;
                    var artist = AddArtist(maxId, IdInRole, tbFirstname.Text, tbSurname.Text, tbMiddleName.Text, PhotoPath, cbProducer.SelectedIndex + 1, tbPhoneNumber.Text, tbPassport.Text);
                   
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
                createFile(newUser);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении пользователя: {ex.Message}");
            }
        }

        public static Admins AddAdmin(int maxId, int IdInRole, string firstName, string lastName, string middleName, string photoPath)
        {
            var admin = new Admins
            {
                first_name = firstName,
                last_name = lastName,
                middle_name = middleName,
                photo_path = photoPath,
                admin_id = IdInRole,
                account_id = maxId,
            };
            var context = new ValidationContext(admin);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if (Validator.TryValidateObject(admin, context, results, true))
                return admin;
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var reason in results)
                {
                    /*добавление новой строки в конец строки sb*/
                    sb.AppendLine(reason.ToString());
                }
                MessageBox.Show(sb.ToString());
                return null;
            }
        }

        public static Artists AddArtist(int maxId, int IdInRole, string firstName, string lastName, string middleName, string photoPath, int producerId, string phoneNumber, string passportNumber)
        {
            var artist = new Artists
            {
                first_name = firstName,
                last_name = lastName,
                middle_name = middleName,
                photo_path = photoPath,
                artist_id = IdInRole,
                account_id = maxId,
                producer_id = producerId,
                phone_number = phoneNumber,
                passport_number = passportNumber
            };
            var context = new ValidationContext(artist);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if (Validator.TryValidateObject(artist, context, results, true))
                return artist;
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var reason in results)
                {
                    sb.AppendLine(reason.ToString());
                }
                MessageBox.Show(sb.ToString());
                return null;
            }
        }

        public static Producers AddProducer(int maxId, int IdInRole, string firstName, string lastName, string middleName, string photoPath, string phoneNumber, string passportNumber)
        {
            var producer = new Producers
            {
                first_name = firstName,
                last_name = lastName,
                middle_name = middleName,
                photo_path = photoPath,
                account_id = maxId,
                producer_id = IdInRole,
                phone_number = phoneNumber,
                passport_number = passportNumber,
            };

            var context = new ValidationContext(producer);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if (Validator.TryValidateObject(producer, context, results, true))
                return producer;
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var reason in results)
                {
                    sb.AppendLine(reason.ToString());
                }
                MessageBox.Show(sb.ToString());
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

        // Показ определенных полей страницы в зависимости от роли пользователя
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
        private void createFile(UserAccounts newUser)
        {
            try
            {
                MusicRecordEntities db = Helper.GetContext();
                string fullNameStaff = string.Join(" ", tbSurname.Text, tbFirstname.Text, tblMiddleName.Text);
                 
               
                string date = DateTime.Now.ToLongDateString();

                string workDate = DateTime.Now.AddDays(1).ToString();
                string workDateS = DateTime.Now.AddDays(1).ToLongDateString();
                string dateOfSigning = DateTime.Now.ToShortDateString();

                var staffSpeciality = (db.Roles.FirstOrDefault(x => x.id_role == newUser.role_id)).role_name;

                var items = new Dictionary<string, string>()
                {
                    {"<Number>", Convert.ToString(newUser.account_id)},
                    {"<City>", "Новосибирск" },
                    {"<DateOfCreate>", date },
                    {"<MusicRecord>", "Студия звукозаписи"},
                    {"<MusicRecordName>", "Для четких реперов" },
                    {"<Director>", "Тимура Ильдаровича Юнусова" },
                    {"<FullNameStaff>", fullNameStaff},
                    {"<MusicRecordItem>", "студию звукозаписи \"Для четких реперов\"" },
                    {"<Role>", staffSpeciality },
                    {"<AddresRecord>", "Светлая 86" },
                    {"<WorkDate>", workDate},
                    {"<Probation>",  "3(три)"},
                    {"<Salary>", "20000" },
                    {"<SalaryLetters>", "двадцать тысяч" },
                    {"<NormDock>", "Отпускные выплаты" },
                    {"<DescripNormalDock>", "(выплаты с размере 0.5% от оклада)" },
                    {"<DirectorINN>", "638469378" },
                    {"<DirectorNameINN>", "BlackStar Mafia" },
                    {"<GeneralDirector>", "Егор Николаевич Булаткин" },
                    {"<GeneralDirectorSocr>", "Булаткин Е. Н." },
                    {"<GeneralDirectorSign>", "<Подпись>" },
                    {"<Passport>", tbPassport.Text },
                    {"<Issued>", "ГУ МВД по Новосибирской Области"},
                    {"<DateOfSigning>",  dateOfSigning}
                };

                using (var doc = DocX.Load("C:\\Users\\Alina\\source\\repos\\-34\\Практическая 3\\Contracts\\defaultContract.docx"))
                {
                    foreach (var item in items)
                    {
                        foreach (var paragraph in doc.Paragraphs)
                        {
                            if (paragraph.Text.Contains(item.Key))
                            {
                                paragraph.ReplaceText(item.Key, item.Value);
                            }
                        }
                    }
                    doc.SaveAs($@"C:\Users\Alina\source\repos\-34\Практическая 3\Contracts\{tbSurname.Text}Contract.docx");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}