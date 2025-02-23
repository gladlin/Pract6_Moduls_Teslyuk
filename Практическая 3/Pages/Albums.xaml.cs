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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Практическая_3.Services;

namespace Практическая_3.Pages
{
    /// <summary>
    /// Логика взаимодействия для Albums.xaml
    /// </summary>
    /// 

    public struct albumStruct
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Cover { get; set; }
    }

    public partial class Albums : Page
    {
        int artistId;
        public Albums(int artist_id)
        {
            InitializeComponent();
            artistId = artist_id;
            GetAllAlbums(artist_id);
        }

        private void GetAllAlbums(int artist_id)
        {
            var db = Helper.GetContext();
            var albums = db.Albums.ToList();
            List<albumStruct> albumsResult = new List<albumStruct> { };

            foreach (var album in albums)
            {
                if (album.artist_id == artist_id)
                {
                    albumStruct newAlbum = new albumStruct();
                    newAlbum.Title = album.title;
                    newAlbum.Genre = album.genre;
                    newAlbum.Cover = album.cover;
                    albumsResult.Add(newAlbum);
                }
            }
            LViewProductAlbum.ItemsSource = albumsResult;
        }

        private void btnPrintDocs_Click(object sender, RoutedEventArgs e)
        {
            FlowDocument doc = flowDocumentReader.Document;

            if (doc == null)
            {
                MessageBox.Show("Документ не найден");
                return;
            }
            else
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    IDocumentPaginatorSource idpSource = doc;
                    printDialog.PrintDocument(idpSource.DocumentPaginator, "Список сотрудников");
                }
            }
        }
    }
}
