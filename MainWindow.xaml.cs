using GetRankedPoints;
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
using Logichandler;
using System.IO;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using AutoUpdateTester;

namespace ValorantEloTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string[] userinfo { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Valorant Elo";
            new Updator();
            //Grabbing username and password
            LogicHandle.username = Properties.Settings1.Default.username;
            LogicHandle.password = Properties.Settings1.Default.password;
            //Logic Startup
            userinfo = LogicHandle.logicstartup();
            if (LogicHandle.ValorantLogin())
            {
                LogicHandle.CheckRankedUpdates();
                Console.WriteLine("RankName:" + LogicHandle.rankname);
                displayuserbox.Text = $"User: {userinfo[0]} Region: {userinfo[1]} Rank: {LogicHandle.rankname}";
                selectionslider.Value = 1;
                selectionslider.Value = 0;
                selectionslider.IsEnabled = true;
                //Line Series Stuff

                SolidColorBrush redbrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings1.Default.colour));
                SolidColorBrush lightredbrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10C1C1C1"));
                ChartValues<double> chartvalue = new ChartValues<double>();
                SeriesCollection = new SeriesCollection
                {
                new LineSeries
                {
                    Fill = lightredbrush,
                    Stroke = redbrush,
                    Title = "Valorant Elo",
                    Values = new ChartValues<double> { LogicHandle.elolist[19], LogicHandle.elolist[18], LogicHandle.elolist[17], LogicHandle.elolist[16], LogicHandle.elolist[15], LogicHandle.elolist[14], LogicHandle.elolist[13], LogicHandle.elolist[12], LogicHandle.elolist[11], LogicHandle.elolist[10], LogicHandle.elolist[9], LogicHandle.elolist[8], LogicHandle.elolist[7], LogicHandle.elolist[6], LogicHandle.elolist[5], LogicHandle.elolist[4], LogicHandle.elolist[3], LogicHandle.elolist[2], LogicHandle.elolist[1], LogicHandle.elolist[0] }
                }
            };
                Labels = new[] { "20", "19", "18", "17", "16", "15", "14", "13", "12", "11", "10", "9", "8", "7", "6", "5", "4", "3", "2", "1" };
                YFormatter = value => value.ToString("0");
                DataContext = this;
                //End Region
            }
        }

        private void dragbox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void closebtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SettingsForm newindow = new SettingsForm();
            newindow.Show();
            this.Close();
            //Settings Option ||Login||Other Stuff||
        }

        private void refreshbtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
        }

        private void selectionslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderbox.Text = Convert.ToString(selectionslider.Value + 1);
            string after = LogicHandle.EloAfter[Convert.ToInt32(selectionslider.Value)];
            string before = LogicHandle.EloBefore[Convert.ToInt32(selectionslider.Value)];
            string movement = LogicHandle.CompetitiveMovement[Convert.ToInt32(selectionslider.Value)];
            string map = superparser.mapnameparse(LogicHandle.maps[Convert.ToInt32(selectionslider.Value)]);
            string dynamicrankname = Convert.ToString(LogicHandle.compranknum[Convert.ToInt32(selectionslider.Value)]);
            int finalelo = ((Convert.ToInt32(dynamicrankname) * 100) -300 + Convert.ToInt32(LogicHandle.EloAfter[Convert.ToInt32(selectionslider.Value)]));
            DateTime timeofmatch = LogicHandle.matchstarts[Convert.ToInt32(selectionslider.Value)];
            mapbox.Text = map;
            datebox.Text = Convert.ToString(timeofmatch);
            eloafter.Text = finalelo.ToString();
            pointsafter.Text = after;
            pointsbefore.Text = before;
            rankmovementimg.Source = new BitmapImage(new Uri($"/{movement}.png", UriKind.Relative));
            
            //rankmovement.Text = movement;

            if (movement == "PROMOTED")
            {
                //totalincrease.Foreground = new SolidColorBrush(Colors.LightGreen);
                totalincrease.Text = $"+{(Convert.ToInt32(before) - 100 - Convert.ToInt32(after)) * -1}";
            }
            else if (movement == "DEMOTED")
            {
                //totalincrease.Foreground = new SolidColorBrush(Color.FromRgb(250, 68, 84));
                totalincrease.Text = Convert.ToString(Convert.ToInt32(after) - 100 - Convert.ToInt32(before));
            }
            else if (movement != "MOVEMENT_UNKNOWN")
            {
                if (Convert.ToInt32(before) < Convert.ToInt32(after))
                {
                    
                    //totalincrease.Foreground = new SolidColorBrush(Colors.LightGreen);
                    totalincrease.Text = $"+{Convert.ToInt32(after) - Convert.ToInt32(before)}";
                }
                else
                {
                    //totalincrease.Foreground = new SolidColorBrush(Color.FromRgb(250, 68, 84));
                    totalincrease.Text = $"{(Convert.ToInt32(before) - Convert.ToInt32(after)) * -1}";
                }
            }
            imagebanner.Source = new BitmapImage(new Uri($"/TX_CompetitiveTier_Large_{dynamicrankname}.png", UriKind.Relative));
        }
    }
}
