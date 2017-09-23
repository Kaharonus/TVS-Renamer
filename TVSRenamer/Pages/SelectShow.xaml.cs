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
        public SelectShow(List<TVShow> list) {
            InitializeComponent();
            this.list = list;
        }
        List<TVShow> list;

        private void MainScrollViewer_Loaded(object sender, RoutedEventArgs e) {
            FillUI();
        }

        private void FillUI() {
            Task t = new Task(() => {
                foreach (TVShow show in list) {
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
            });
            t.Start();
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
            MainWindow.SwitchPage(new Locations(s));          
        }
    }
}
