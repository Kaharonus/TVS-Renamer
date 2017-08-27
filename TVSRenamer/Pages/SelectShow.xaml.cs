using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace TVSRenamer
{
    /// <summary>
    /// Interaction logic for SelectShow.xaml
    /// </summary>
    public partial class SelectShow : Page
    {
        public SelectShow()
        {
            InitializeComponent();
        }

        private void GitHub_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            Process.Start("https://github.com/Kaharonus/TVS-Renamer");   
        }

        private void Info_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            Page p = new Info();
            Window main = Window.GetWindow(this);
            ((MainWindow)main).AddTempFrame(p);
        }
        Thread searchThread;
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            string text = TextBox.Text;
            Action a = () => FillUI(text);
            searchThread = new Thread(a.Invoke);
            searchThread.IsBackground = true;
            searchThread.Name = "SearchAPI";
            searchThread.Start();             
        }
        private void FillUI(string name) {           
            List<TVShow> s = API.getShows(name);
            Dispatcher.Invoke(new Action(() => {
                ClearList();
            }), DispatcherPriority.Send);
            foreach (TVShow show in s) {
                Dispatcher.Invoke(new Action(() => {                   
                    SearchResult sr = new SearchResult(show);
                    sr.Opacity = 0;
                    Storyboard MoveUp = FindResource("OpacityUp") as Storyboard;
                    MoveUp.Begin(sr);
                    sr.MainText.Text = show.name;
                    sr.Selected.MouseLeftButtonUp += (se, e) => NextPage(show);
                    panel.Children.Add(sr);
                }), DispatcherPriority.Send);
                Thread.Sleep(7);
            }
        }
        private void ClearList() {
            UIElementCollection children = panel.Children;
            foreach (SearchResult child in children) {
                Storyboard OpacityDown = FindResource("OpacityDown") as Storyboard;
                OpacityDown.Begin(child);
                Thread.Sleep(1);
            }
            panel.Children.Clear();
        }


        private void NextPage(TVShow s) {
            Page p = new Locations(s);
            Window main = Window.GetWindow(this);
            ((MainWindow)main).SetFrame(p);
        }

#region Animations
        private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
            Storyboard MoveUp = FindResource("MoveUp") as Storyboard;
            Storyboard MoveUpLogo = FindResource("MoveUpLogo") as Storyboard;
            MoveUpLogo.Begin(Logo);
            Storyboard Opacity = FindResource("OpacityDown") as Storyboard;
            Opacity.Begin(Logo);
            MoveUp.Completed += (s,ev) => AddEvent();
            MoveUp.Begin(TextBox);
            
            AnimateMargins();          
            TextBox.Text = "";
            TextBox.GotFocus -= TextBox_GotFocus;
        }
        private void AddEvent() {
            TextBox.TextChanged += TextBox_TextChanged;
            MainScrollViewer.Visibility = Visibility.Visible;
        }
        private void AnimateMargins() {
            Thread t = new Thread( new ThreadStart(AnimateLeft));
            t.Start();
            Thread t2 = new Thread(new ThreadStart(AnimateRight));
            t2.Start();
        }
        private void AnimateLeft() {
            for (double i = 1; i >= 0; i-=.05) {
                Application.Current.Dispatcher.BeginInvoke(
                 new Action(() => LeftColumn.Width = new GridLength(i,GridUnitType.Star)));
                Thread.Sleep(15);
            }
        }
        private void AnimateRight() {
            for (double i = 1; i >= 0; i -= .05) {
                Application.Current.Dispatcher.BeginInvoke(
                 new Action(() => RightColumn.Width = new GridLength(i, GridUnitType.Star)));
                Thread.Sleep(15);
            }
        }
#endregion
    }
}
