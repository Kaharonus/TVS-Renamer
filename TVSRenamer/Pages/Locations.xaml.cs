using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Ookii.Dialogs.Wpf;
using System.Windows.Forms;
using System.IO;
using System.Windows.Threading;
using System.Text.RegularExpressions;

namespace TVSRenamer {
    /// <summary>
    /// Interaction logic for Locations.xaml
    /// </summary>
    public partial class Locations : Page {
        public Locations(TVShow show) {
            InitializeComponent();
            this.show = show;
        }
        TVShow show;

        /*private void Grid_Loaded(object sender, RoutedEventArgs e) {
            //ShowName.Text = show.name;
            Loc1.Text = Properties.Settings.Default.Lokace1;
            Loc2.Text = Properties.Settings.Default.Lokace2;
            Loc3.Text = Properties.Settings.Default.Lokace3;
            FolderSizeBox.Text = Properties.Settings.Default.maxSize.ToString();
            checkDelete.IsChecked = Properties.Settings.Default.Delete;
        }

        private void StartButton_MouseUp(object sender, MouseButtonEventArgs e) {
            ShowWaiting();
            string text = TextBox.Text;
            if (CheckFolders()) {
                List<string> l = new List<string>();
                l.Add(Loc1.Text);
                l.Add(Loc2.Text);
                l.Add(Loc3.Text);
                Action a = null;
                //a = () => Renamer.RenameBatch(l, text, show);                            
                IAsyncResult ar = a.BeginInvoke(Callback, null);
            } else {
                System.Windows.MessageBox.Show("One of the paths you entered doesn't exist");
                HideWaiting();
            }
        }
        public void Callback(IAsyncResult result) {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => HideWaiting()));
        }
        private bool CheckFolders() {
            if (Directory.Exists(TextBox.Text) && (Directory.Exists(Loc1.Text) || Loc1.Text == "") && (Directory.Exists(Loc2.Text) || Loc2.Text == "") && (Directory.Exists(Loc3.Text) || Loc3.Text == "")) {
                Properties.Settings.Default.Lokace1 = Loc1.Text;
                Properties.Settings.Default.Lokace2 = Loc2.Text;
                Properties.Settings.Default.Lokace3 = Loc3.Text;
                Properties.Settings.Default.Save();
                return true;
            } else { return false; }
        }

        private void SelectLocation_MouseUp(object sender, MouseButtonEventArgs e) {
            VistaFolderBrowserDialog vfbd = new VistaFolderBrowserDialog();
            vfbd.ShowDialog();
            if (vfbd.SelectedPath != "") {
                TextBox.Text = vfbd.SelectedPath;
            }
        }

        private void Loc1Select_MouseUp(object sender, MouseButtonEventArgs e) {
            VistaFolderBrowserDialog vfbd = new VistaFolderBrowserDialog();
            vfbd.ShowDialog();
            if (vfbd.SelectedPath != "") {
                Loc1.Text = vfbd.SelectedPath;
            }
        }
        private void Loc2Select_MouseUp(object sender, MouseButtonEventArgs e) {
            VistaFolderBrowserDialog vfbd = new VistaFolderBrowserDialog();
            vfbd.ShowDialog();
            if (vfbd.SelectedPath != "") {
                Loc2.Text = vfbd.SelectedPath;
            }
        }
        private void Loc3Select_MouseUp(object sender, MouseButtonEventArgs e) {
            VistaFolderBrowserDialog vfbd = new VistaFolderBrowserDialog();
            vfbd.ShowDialog();
            if (vfbd.SelectedPath != "") {
                Loc3.Text = vfbd.SelectedPath;
            }
        }
        private void ShowWaiting() {
            StartButton.MouseUp -= StartButton_MouseUp;
            ShowHideMenu("ShowBottom", Waiting);
        }

        private void HideWaiting() {
            StartButton.MouseUp += StartButton_MouseUp;
            ShowHideMenu("HideBottom", Waiting);
        }

        private void ShowHideMenu(string Storyboard, Grid pnl) {
            System.Windows.Media.Animation.Storyboard sb = Resources[Storyboard] as System.Windows.Media.Animation.Storyboard;
            sb.Begin(pnl);
        }

        private void checkDelete_Click(object sender, RoutedEventArgs e) {
            if (checkDelete.IsChecked == true) {
                FolderSizeBox.IsEnabled = true;
                Properties.Settings.Default.Delete = true;
                Properties.Settings.Default.Save();
            } else {
                FolderSizeBox.IsEnabled = false;
                Properties.Settings.Default.Delete = false;
                Properties.Settings.Default.Save();
            }
        }

        private void FolderSizeBox_TextChanged(object sender, TextChangedEventArgs e) {
            string pattern = @"^([1-9][0-9]{0,1}(\.[\d]{1,2})?|100)$";
            if (Regex.IsMatch(FolderSizeBox.Text, pattern)) {
                Properties.Settings.Default.maxSize = Int32.Parse(FolderSizeBox.Text);
                Properties.Settings.Default.Save();
            } else if (FolderSizeBox.Text != "") {
                Properties.Settings.Default.maxSize = 1;
                Properties.Settings.Default.Save();
            } else {
                System.Windows.MessageBox.Show("Enter numbers between 1-100");
            }

        }


    */


    }
}
