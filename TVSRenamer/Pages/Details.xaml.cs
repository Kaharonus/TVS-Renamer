using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace TVSRenamer {
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : Page {
        public Details(TVShow s) {
            InitializeComponent();
            show = s;
        }
        TVShow show;

        private void Grid_Loaded(object sender, RoutedEventArgs e) {
        /*    ShowName.Text = show.name;
            rating.Text = show.rating + "/10";
            date.Text = show.releaseDate;
            station.Text = show.station;
            imdb.MouseLeftButtonUp += (se, ea) => OpenIMDb(show.imdb);
            overview.Text = RemoveStyle(show.overview);
            if (show.image != null) { 
                Img.Source = new BitmapImage(new Uri(show.image, UriKind.RelativeOrAbsolute));
            }
            foreach (string genre in show.genres) {
                genres.Text += genre + ", ";
            }
        }
        private void OpenIMDb(string url) {
            Process.Start(url);*/
        }

        private string RemoveStyle(string line) {
            string[] separator = { "</?p>", "</?em>", "</?strong>", "</?b>", "</?i>", "</?span>" };
            string text = line;
            for (int i = 0; i < separator.Length; i++) {
                Regex reg = new Regex(separator[i]);
                Match m = reg.Match(text);
                while (m.Success) {
                    m = reg.Match(text);
                    if (m.Success) {
                        text = text.Remove(m.Index, m.Length);
                    }
                }
            }
            return text;
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            Window main = Window.GetWindow(this);
            ((MainWindow)main).CloseTempFrame();
        }
    }
}
