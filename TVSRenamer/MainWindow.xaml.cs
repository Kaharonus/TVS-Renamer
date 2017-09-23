using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using TVSRenamer;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;
using Panel = System.Windows.Controls.Panel;
using System.Windows.Media.Animation;
using Application = System.Windows.Application;
using System.Threading.Tasks;

namespace TVSRenamer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public void SetFrame(Page page) {
            MainFrame.Content = page;
        }
        public void CloseTempFrame() {
            BaseGrid.Children.RemoveAt(BaseGrid.Children.Count - 1);
        }

        public static void SwitchPage(Page p, bool animate = true) {
            Window main = Application.Current.MainWindow;
            if (animate) {
                ((MainWindow)main).SwitchFrame(p);
            } else {
                ((MainWindow)main).SwitchFrameNoAnim(p);
            }
        }

        private void SwitchFrameNoAnim(Page p) {
            MainFrame.Content = p;
        }

        private void SwitchFrame(Page p) {
            Storyboard FadeOut = FindResource("OpacityDown") as Storyboard;
            FadeOut.Completed += (s,e) => {
                Storyboard fadein = FindResource("OpacityUp") as Storyboard;
                MainFrame.Content = p;
                fadein.Begin(MainFrame);

            };
            FadeOut.Begin(MainFrame);
        }

        Task<List<TVShow>> task;
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            string name = TextBox.Text;
            if (task != null) { 
                if (task.Status == TaskStatus.Running) {
                    task.ContinueWith((t) => { });
                }
            }
            task = new Task<List<TVShow>>(() => API.getShows(name));
            task.ContinueWith((t) => {
                SetUI(task.Result);
            });
            task.Start();
        }
        private void SetUI(List<TVShow> showList) {
            Dispatcher.Invoke(new Action(() => {
            Page p = (Page)MainFrame.Content;
                if (p.Name == "LocationsSettings") {               
                    SwitchPage(new SelectShow(showList), false);             
                } else {
                    SwitchPage(new SelectShow(showList));
                }
            }), DispatcherPriority.Send);
        }

        private void MainFrame_Loaded(object sender, RoutedEventArgs e) {
            MainFrame.Content = new Page();
        }

        private void GitHubIcon_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Process.Start("https://github.com/Kaharonus/TVS-Renamer");
        }


        #region AppInitiationAnimations

        private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
            Storyboard MoveUp = FindResource("MoveUp") as Storyboard;
            Storyboard MoveUpLogo = FindResource("MoveUpLogo") as Storyboard;
            MoveUpLogo.Begin(Logo);
            Storyboard Opacity = FindResource("OpacityDown") as Storyboard;
            Storyboard OpacityUp = FindResource("OpacityUpSlow") as Storyboard;
            OpacityUp.Begin(MenuIcon);
            OpacityUp.Begin(GitHubIcon);
            Opacity.Begin(Logo);
            MoveUp.Completed += (s, ev) => AddEvent();
            MoveUp.Begin(TextBox);

            AnimateMargins();
            TextBox.Text = "";
            TextBox.GotFocus -= TextBox_GotFocus;
        }

        private void AddEvent() {
            TextBox.TextChanged += TextBox_TextChanged;
            MainFrame.Visibility = Visibility.Visible;
        }


        private void AnimateMargins() {
            Thread t = new Thread(new ThreadStart(AnimateLeft));
            t.Start();
            Thread t2 = new Thread(new ThreadStart(AnimateRight));
            t2.Start();
        }
        private void AnimateLeft() {
            for (double i = 1; i >= 0; i -= .05) {
                double width = 100;
                Dispatcher.Invoke(new Action(() => {
                    width = LeftColumn.Width.Value;
                }), DispatcherPriority.Send);
                if (width <= 0.1) {
                    Dispatcher.Invoke(new Action(() => {
                        LeftColumn.Width = new GridLength(10, GridUnitType.Pixel);
                    }), DispatcherPriority.Send);
                    break;
                }
                Application.Current.Dispatcher.BeginInvoke(
                 new Action(() => LeftColumn.Width = new GridLength(i, GridUnitType.Star)));
                Thread.Sleep(15);
            }
        }
        private void AnimateRight() {
            for (double i = 1; i >= 0; i -= .05) {
                double width = 100;
                Dispatcher.Invoke(new Action(() => {
                    width = RightColumn.ActualWidth;
                }), DispatcherPriority.Send);
                if (width <= 75) {
                    Dispatcher.Invoke(new Action(() => {
                        RightColumn.Width = new GridLength(75, GridUnitType.Pixel);
                    }), DispatcherPriority.Send);
                    break;
                }
          
                Application.Current.Dispatcher.BeginInvoke(
                 new Action(() => RightColumn.Width = new GridLength(i, GridUnitType.Star)));
                Thread.Sleep(15);
            }
        }
        private void Set80() {

        }

        #endregion
    }
}