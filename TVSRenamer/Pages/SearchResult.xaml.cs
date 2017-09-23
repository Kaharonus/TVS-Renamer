using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TVSRenamer {
    /// <summary>
    /// Interakční logika pro SearchResult.xaml
    /// </summary>
    public partial class SearchResult : UserControl {
        public SearchResult(TVShow show) {
            InitializeComponent();
            this.show = show;
        }
        TVShow show;

        private void Details_MouseEnter(object sender, MouseEventArgs e) {
            Thread t = new Thread(new ThreadStart(FillInInfo));
            t.IsBackground = true;
            t.Start();
            Storyboard open = FindResource("OpenDetails") as Storyboard;
            open.Completed += (se, ev) => { DetailsGrid.Visibility = Visibility.Visible; };
            open.Begin(MainPart);
        }
        private void FillInInfo() {
            Dispatcher.Invoke(new Action(() => {
                showName.Text = show.name;
                showName.MouseLeftButtonUp -= (s, e) => { Process.Start("http://www.imdb.com/title/" + show.externals.imdb + "/?ref_=nv_sr_1"); };
                showName.MouseLeftButtonUp += (s, e) => { Process.Start("http://www.imdb.com/title/"+show.externals.imdb+"/?ref_=nv_sr_1"); };
                network.Text = show.network.name;
                timezone.Text = show.network.country.timezone.Replace('_', ' ');
                stat.Text = show.status;
                len.Text = show.runtime + " minutes";
                lang.Text = show.language;
                type.Text = show.type;
                offsite.MouseLeftButtonUp -= (s, e) => { Process.Start(show.officialSite); };
                offsite.MouseLeftButtonUp += (s, e) => { Process.Start(show.officialSite); };
                rating.Text = show.rating.average + "/10";
                if (show.genres != null) {
                    string genre = null;
                    for (int i = 0; i < show.genres.Count;i++) {
                        if (i == show.genres.Count - 1) {
                            genre += show.genres[i];

                        } else {
                            genre += show.genres[i] + ", ";
                        }
                    }
                    genres.Text = genre;
                }
                if (show.schedule.days != null) {
                    string schedules = null;
                    for (int i = 0; i < show.schedule.days.Count; i++) {
                        if (i == show.schedule.days.Count - 1) {
                            schedules += show.schedule.days[i] + " at ";
                        } else {
                            schedules += show.schedule.days[i] + ", ";
                        }
                    }
                    schedule.Text = schedules;
                }
                schedule.Text += show.schedule.time;
                if (show.premiered != null) {
                    prem.Text = DateTime.ParseExact(show.premiered, "yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.None).ToString("dd.MM.yyyy");
                }
                summary.Text = RemoveStyle(show.summary);
                BitmapImage image = new BitmapImage(new Uri(show.image.original));
                Poster.Source = image;
            }), DispatcherPriority.Send);
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
        private void MainPart_MouseLeave(object sender, MouseEventArgs e) {
            DetailsGrid.Visibility = Visibility.Hidden;
            Storyboard close = FindResource("CloseDetails") as Storyboard;           
            close.Begin(MainPart);
        }

        private void showName_MouseEnter(object sender, MouseEventArgs e) {
            showName.Cursor = Cursors.Hand;
        }

        private void showName_MouseLeave(object sender, MouseEventArgs e) {
            showName.Cursor = Cursors.Arrow;
        }
    }
}
