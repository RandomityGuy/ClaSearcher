using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Net;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Threading;
namespace ClaSearcher
{
    public class Mission
    {
        BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        public int id;
        public string name;
        public string desc;
        public string artist;
        public string modification;
        public string gameType;
        public string baseName;
        public int gems;
        public int? difficulty;
        public float? rating;
        public int? weight;
        public bool egg;
        public string bitmap;
        bool isDownloading = false;
        BitmapImage downloadedImage = null;
        public int Id
        {
            get => id;
        }
        public string Name
        {
            get => name;
        }
        public string Desc { get => desc; }
        public string Artist { get => artist; } 
        public string Modification { get => modification;  }
        public string GameType { get => gameType;  }
        public string BaseName { get => baseName;  }
        public int Gems { get => gems;  }
        public bool Egg { get => egg;  }
        public float? Rating { get => rating; }
        public int? Difficulty { get => difficulty; }
        public int? RatingWeight { get => weight; }
        public async Task<BitmapImage> getDownloadImageAsync(Image progress,Dispatcher dispatcher)
        {
            if (downloadedImage != null) return downloadedImage;
            else
            {
                var client = new WebClient();
                client.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) =>
                {
                    dispatcher.BeginInvoke(new Action(delegate () { progress.Source = ProgressToImage(e.ProgressPercentage); }));
                };
                client.DownloadDataCompleted += (object sender, DownloadDataCompletedEventArgs e) =>
                {
                    var bitmapdownloaded = e.Result;
                    downloadedImage = ToImage(bitmapdownloaded);
                    dispatcher.BeginInvoke(new Action(delegate () { progress.Source = downloadedImage; }));
                };
                var bitmapdata = await client.DownloadDataTaskAsync("https://cla.higuy.me/api/v1/missions/" + id.ToString() + "/bitmap");
                downloadedImage = ToImage(bitmapdata);

                return downloadedImage;
            }
        }
        public void downloadMission(Image progress, Dispatcher dispatcher,UIElementCollection boxcollection,List<Mission> list,UIElement box)
        {
            if (!isDownloading)
            {
                var client = new WebClient();
                client.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) =>
                {
                    dispatcher.BeginInvoke(new Action(delegate () { progress.Source = ProgressToImage(e.ProgressPercentage); }));
                };
                client.DownloadFileCompleted += (object sender, System.ComponentModel.AsyncCompletedEventArgs e) =>
                {
                    dispatcher.BeginInvoke(new Action(delegate () { progress.Source = downloadedImage; }));
                    boxcollection.Remove(box);
                    list.Remove(this);
                    isDownloading = false;
                };
                isDownloading = true;
                client.DownloadFileAsync(new Uri("https://cla.higuy.me/api/v1/missions/" + Id.ToString() + "/zip"), BaseName + ".zip");
            }
        }
        private string ProgressToImageURI(int progress)
        {
            float prg = (progress * 16) / 100;
            int nearestprg =  (int)Math.Round((double)prg);
            return ".\\Progress\\loader_" + nearestprg.ToString() + ".png";
        }

        BitmapImage ProgressToImage(int progress)
        {
            float prg = (progress * 16) / 100;
            int nearestprg = (int)Math.Round((double)prg);
            return ToBitmapImage((System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject($"loader_{nearestprg}"));
            //return new BitmapImage(new Uri($"Progress/loader_{nearestprg}.png"));
        }

        BitmapImage ToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}
