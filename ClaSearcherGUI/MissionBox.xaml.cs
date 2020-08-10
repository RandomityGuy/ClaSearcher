using ClaSearcher;
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

namespace ClaSearcherGUI
{
    /// <summary>
    /// Interaction logic for MissionBox.xaml
    /// </summary>
    public partial class MissionBox : UserControl
    {
        public Color GameColor = Colors.Black;
        public UIElementCollection parentCollection;
        public List<Mission> parentList;

        public Mission mission;

        public MissionBox(Mission mission)
        {
            InitializeComponent();
            this.mission = mission;

            MissionName.Content = mission.Name;
            AuthorName.Content = mission.Artist;
            if (mission.Modification == "gold") GameColor = Color.FromRgb(244, 237, 52);
            if (mission.Modification == "platinum") GameColor = Color.FromRgb(189, 199, 221);
            if (mission.Modification == "Ultra") GameColor = Color.FromRgb(83, 169, 255);
            if (mission.Modification == "platinumquest") GameColor = Color.FromRgb(250, 250, 250);

            if (mission.Difficulty.HasValue)
            {
                switch (mission.difficulty.Value)
                {
                    case 1:
                        DifficultyLevel.Content = "VERY EASY";
                        DifficultyLevel.Foreground = new SolidColorBrush(Colors.Black);
                        DifficultyBorder.Background = new SolidColorBrush(Color.FromRgb(85, 255, 0));
                        break;

                    case 2:
                        DifficultyLevel.Content = "EASY";
                        DifficultyLevel.Foreground = new SolidColorBrush(Colors.Black);
                        DifficultyBorder.Background = new SolidColorBrush(Color.FromRgb(149, 255, 0));
                        break;

                    case 3:
                        DifficultyLevel.Content = "BEGINNER";
                        DifficultyLevel.Foreground = new SolidColorBrush(Colors.Black);
                        DifficultyBorder.Background = new SolidColorBrush(Color.FromRgb(212, 255, 0));
                        break;

                    case 4:
                        DifficultyLevel.Content = "INTERMEDIATE";
                        DifficultyLevel.Foreground = new SolidColorBrush(Colors.Black);
                        DifficultyBorder.Background = new SolidColorBrush(Color.FromRgb(255, 234, 0));
                        break;

                    case 5:
                        DifficultyLevel.Content = "ADVANCED";
                        DifficultyLevel.Foreground = new SolidColorBrush(Colors.White);
                        DifficultyBorder.Background = new SolidColorBrush(Color.FromRgb(255, 170, 0));
                        break;

                    case 6:
                        DifficultyLevel.Content = "EXPERT";
                        DifficultyLevel.Foreground = new SolidColorBrush(Colors.White);
                        DifficultyBorder.Background = new SolidColorBrush(Color.FromRgb(255, 106, 0));
                        break;

                    case 7:
                        DifficultyLevel.Content = "NIGHTMARE";
                        DifficultyLevel.Foreground = new SolidColorBrush(Colors.White);
                        DifficultyBorder.Background = new SolidColorBrush(Color.FromRgb(255, 43, 0));
                        break;

                    case 8:
                        DifficultyLevel.Content = "IMPOSSIBLE";
                        DifficultyLevel.Foreground = new SolidColorBrush(Colors.White);
                        DifficultyBorder.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        break;
               }
            }
            else
            {
                DifficultyBorder.Visibility = Visibility.Collapsed;
            }

            BG.Stroke = new SolidColorBrush(GameColor);
            /*var mission = Tag as Mission;
            if (mission.Modification == "gold") GameColor = Color.FromRgb(244, 237, 52);
            if (mission.Modification == "platinum") GameColor = Color.FromRgb(189, 199, 221);
            if (mission.Modification == "Ultra") GameColor = Color.FromRgb(83, 169, 255);*/
        }

        private void onHover(object sender, MouseEventArgs e)
        {
            this.RenderTransform = new ScaleTransform(1.1, 1.1) { CenterX = 76, CenterY = 76 };
            BG.Fill = new SolidColorBrush(Color.FromRgb(22,119,193));
            BG.Stroke = new SolidColorBrush(Color.FromRgb(17, 91, 147));
        }

        async public void getMissionImage()
        {
            MissionPreview.Source = await mission.getDownloadImageAsync(MissionPreview,Dispatcher);
        }

        public void getDownloadStatus()
        {
           mission.downloadMission(MissionPreview, Dispatcher,parentCollection,parentList,this);
        }

        private void onExitHover(object sender, MouseEventArgs e)
        {
            this.RenderTransform = new ScaleTransform(1, 1) { CenterX = 76, CenterY = 76 };
            BG.Fill = new SolidColorBrush(Color.FromRgb(52,52,52));
            BG.Stroke = new SolidColorBrush(GameColor);            
        }

        async private void ClickMission(object sender, MouseButtonEventArgs e)
        {
            if (((MainWindow)Application.Current.MainWindow).SelectCheck.IsChecked == true) { ((MainWindow)Application.Current.MainWindow).addMissionForDownload(mission); return; }
            var missioninfo = new MissionInfo(mission);

            ((MainWindow)Application.Current.MainWindow).WindowGrid.Children.Add(missioninfo);
            missioninfo.MissionPreview.Source = await mission.getDownloadImageAsync(missioninfo.MissionPreview, Dispatcher);
        }
    }
}
