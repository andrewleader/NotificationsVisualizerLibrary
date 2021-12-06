using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NotificationsVisualizerLibraryTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UpdateImageInUsePage : Page
    {
        public UpdateImageInUsePage()
        {
            this.InitializeComponent();

            var dontWait = Initialize();
        }

        private async Task Initialize()
        {
            await UpdateImage();
        }

        private async Task UpdateImage()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/PlannerAssets/Square150x150Logo.scale-100.png"));

            using (Stream s = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync("MyImage.png", CreationCollisionOption.ReplaceExisting))
            {
                using (Stream source = await file.OpenStreamForReadAsync())
                {
                    source.CopyTo(s);
                }
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<tile><visual><binding template='TileMedium'><image src='ms-appdata:///local/MyImage.png'/></binding></visual></tile>");

            MediumPreviewTile.CreateTileUpdater().Update(new TileNotification(doc));
        }

        private async void ButtonUpdateImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MediumPreviewTile.CreateTileUpdater().Clear();

                await UpdateImage();
            }

            catch (Exception ex)
            {
                await new MessageDialog(ex.ToString()).ShowAsync();
            }
        }
    }
}
