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
using ClaSearcher;
using Newtonsoft.Json;
using System.Net;
namespace ClaSearcherGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Mission> missionList;
        List<Mission> filteredMissionList;
        List<Mission> shownMissionList;

        List<Mission> downloadList;

        bool isDownloadsPage = false;
        int page = 0;
        int maxpages;
        public MainWindow()
        {
            InitializeComponent();
            var client = new WebClient();
            var missionlistdata = client.DownloadString("https://marbleland.vani.ga/api/level/list");
            missionList = JsonConvert.DeserializeObject<List<Mission>>(missionlistdata);
            maxpages = (int)Math.Ceiling((double)missionList.Count / 15);
            shownMissionList = missionList;
            filteredMissionList = missionList;
            downloadList = new List<Mission>();
            UpdateMissionList();
        }
        public void removeMissionForDownload(Mission mission)
        {
            downloadList.Remove(mission);
        }
        public void addMissionForDownload(Mission mission)
        {
            downloadList.Add(mission);
        }
        private void UpdateMissionList()
        {
            MissionBrowser.Children.Clear();
            List<Mission> collection;
            if (isDownloadsPage)
                collection = downloadList;
            else
                collection = shownMissionList;
            try
            {
                for (int i = 0 + (page * 15); i < (page + 1) * 15; i++)
                {

                    var box = new MissionBox(collection[i]);
                    box.parentCollection = MissionBrowser.Children;
                    box.parentList = collection;
                    MissionBrowser.Children.Add(box);
                    //box.MissionPreview.Source = await missionList[i].getDownloadImageAsync();
                    if (isDownloadsPage)
                        box.getDownloadStatus();
                    else
                        box.getMissionImage();
                }
            }
            catch (Exception)
            {

            }
            maxpages = (int)Math.Ceiling((double)collection.Count / 15);
            UpdatePages();
        }

        private void UpdatePages()
        {
            page = Clamp(page, 0, maxpages - 1);

            PageThing.Content = (page + 1).ToString() + "/" + maxpages.ToString();
        }

        private void SearchMissionListByName(string text)
        {
            shownMissionList = filteredMissionList.Where(mission => mission.Name.ToLower().Contains(text.ToLower())).ToList();
        }
        private void SearchMissionListByAuthor(string text)
        {
            shownMissionList = filteredMissionList.Where(mission => mission.Artist.ToLower().Contains(text.ToLower())).ToList();
        }
        private void SearchMissionListByFileName(string text)
        {
            shownMissionList = filteredMissionList.Where(mission => mission.BaseName.ToLower().Contains(text.ToLower())).ToList();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (page + 1 >= maxpages) return;
            page++;
            UpdatePages();
            UpdateMissionList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (page - 1 < 0) return;
            page--;
            UpdatePages();
            UpdateMissionList();
        }

        private void onEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                switch(SearchFilter.SelectedIndex)
                {
                    case 0:
                        SearchMissionListByName(SearchBox.Text);
                        break;
                    case 1:
                        SearchMissionListByAuthor(SearchBox.Text);
                        break;
                    case 2:
                        SearchMissionListByFileName(SearchBox.Text);
                        break;
                }               
                UpdateMissionList();
            }
        }
        private void onGameFilterChanged(object sender, SelectionChangedEventArgs e)
        {
            var senderas = sender as ComboBox;
            if (senderas.IsLoaded) FilterMissionList();
        }
        private void onMissionTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            var senderas = sender as ComboBox;
            if (senderas.IsLoaded) FilterMissionList();
        }
        void FilterMissionList()
        {
            IEnumerable<Mission> outlist = missionList;

            switch(MissionTypeFilter.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    outlist = missionList.Where(a => a.GameType == "single");
                    break;

                case 2:
                    outlist = missionList.Where(a => a.GameType == "multi");
                    break;
            }

            switch (GameFilter.SelectedIndex)
            {
                case 0:
                    break;

                case 1:
                    outlist = outlist.Where(a => a.Modification == "gold");
                    break;

                case 2:
                    outlist = outlist.Where(a => a.Modification == "platinum");
                    break;

                case 3:
                    outlist = outlist.Where(a => a.Modification == "ultra"); 
                    break;

                case 4:
                    outlist = outlist.Where(a => a.Modification == "platinumquest");
                    break;
            }

            shownMissionList = outlist.ToList();
            UpdateMissionList();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            isDownloadsPage = !isDownloadsPage;
            UpdateMissionList();
        }

        private void enableSelect(object sender, RoutedEventArgs e)
        {           
        }

        int Clamp(int n,int a,int b)
        {
            return Math.Max(a, Math.Min(n, b));
        }
    }
}
