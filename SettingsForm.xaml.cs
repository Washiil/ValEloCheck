using Logichandler;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ValorantEloTracker
{
    /// <summary>
    /// Interaction logic for SettingsForm.xaml
    /// </summary>
    public partial class SettingsForm : Window
    {
        public SettingsForm()
        {
            InitializeComponent();
            Console.WriteLine(colourselectionbox.SelectedIndex);
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if (colourselectionbox.SelectedIndex != -1)
            {
                Properties.Settings1.Default.colour = superparser.colorselectionparse(colourselectionbox.SelectedIndex);
            }
            if (regionselect.SelectedIndex != -1)
            {
                Properties.Settings1.Default.region = regionselect.SelectedIndex;
            }

            if (usernamebox.Text.Length > 0)
            {
                Properties.Settings1.Default.username = usernamebox.Text;
                Properties.Settings1.Default.password = passwordbox.Password;
                LogicHandle.AccessToken = "";
                LogicHandle.EntitlementToken = "";
                LogicHandle.username = "";
                LogicHandle.password = "";
                LogicHandle.region = "";
                LogicHandle.rankname = "";
                LogicHandle.EloAfter.Clear();
                LogicHandle.EloBefore.Clear();
                LogicHandle.CompetitiveMovement.Clear();
                LogicHandle.compranknum.Clear();
                LogicHandle.elolist.Clear();
                LogicHandle.CompetiveMovement = null;
                LogicHandle.maps.Clear();
                LogicHandle.matchstarts.Clear();
            }
            Properties.Settings1.Default.Save();

            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void closebtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
