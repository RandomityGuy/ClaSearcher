using ClaSearcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

namespace ClaSearcherGUI
{
    /// <summary>
    /// Interaction logic for MissionInfo.xaml
    /// </summary>
    public partial class MissionInfo : UserControl
    {
        Timer fadeInTimer;
        Timer fadeOutTimer = null;

        Mission mission;

        public MissionInfo(Mission mission)
        {
            InitializeComponent();
            this.mission = mission;

            MissionName.Content = mission.Name;
            AuthorName.Content = mission.Artist;
            Desc.Text = mission.Desc;
            EE.Visibility = (Visibility)Convert.ToInt32(!mission.Egg);

            Opacity = 0;
            fadeInTimer = new Timer((e) => { Dispatcher.BeginInvoke(new Action(delegate { if (Opacity < 1) Opacity += 0.1; else { GridThing.MouseLeftButtonDown += loseFocus; fadeInTimer.Dispose(); } }));  },null,TimeSpan.Zero,TimeSpan.FromMilliseconds(0.6));
        }

        private void loseFocus(object sender, MouseButtonEventArgs e)
        {           
                if (fadeOutTimer==null)fadeOutTimer = new Timer((e2) => { Dispatcher.BeginInvoke(new Action(delegate {
                        if (Opacity > 0) Opacity -= 0.1;
                        else
                        {
                            fadeOutTimer.Dispose();
                        GridThing.MouseLeftButtonDown -= loseFocus;
                        ((Panel)this.Parent).Children.Remove(this);
                        }
                })); }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(0.6));            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((string)DownloadButton.Content == "Add To Downloads") //Terrible method
            {
                ((MainWindow)Application.Current.MainWindow).addMissionForDownload(mission);
                DownloadButton.Content = "Remove From Downloads";
                //webclient.DownloadFileAsync(new Uri("https://cla.higuy.me/api/getMissionZip.php?id=" + mission.Id.ToString()), mission.BaseName +".zip");
            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).removeMissionForDownload(mission);
                DownloadButton.Content = "Add To Downloads";
            }
        }

        private void onMouseEnter(object sender, MouseEventArgs e)
        {
            var that = sender as Button;
            that.Background = new SolidColorBrush(Color.FromRgb(97, 194, 86));

        }

        private void onMouseLeave(object sender, MouseEventArgs e)
        {
            var that = sender as Button;
            that.Background = new SolidColorBrush(Color.FromRgb(65, 156, 56));
        }
    }
}
