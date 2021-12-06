using NotificationsExtensions.Tiles;
using NotificationsVisualizerLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NotificationsVisualizerLibraryTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SampleSettingsPage : Page
    {
        public SampleSettingsPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            foreach (var tile in AllTiles())
            {
                await UpdateTilePropertiesAsync(tile);
            }

            UpdateTileNotifications();

            ToggleShowExams.Toggled += ToggleShowExams_Toggled;
            ToggleShowHomework.Toggled += ToggleShowHomework_Toggled;
        }

        private async Task UpdateTilePropertiesAsync(PreviewTile tile)
        {
            tile.DisplayName = "CSC 252";
            tile.VisualElements.BackgroundColor = Colors.Red;
            tile.VisualElements.ShowNameOnSquare150x150Logo = true;
            tile.VisualElements.Square150x150Logo = new Uri("ms-appx:///Assets/PlannerAssets/Square150x150Logo.png");
            tile.VisualElements.Wide310x150Logo = new Uri("ms-appx:///Assets/PlannerAssets/Wide310x150Logo.png");
            tile.VisualElements.Square44x44Logo = new Uri("ms-appx:///Assets/PlannerAssets/Square44x44Logo.png");

            // Commit the tile properties we changed
            await tile.UpdateAsync();
        }

        private IEnumerable<PreviewTile> AllTiles()
        {
            return new PreviewTile[]
            {
                MediumPreviewTile,
                WidePreviewTile
            };
        }

        private void UpdateTileNotifications()
        {
            // If both are disabled, there's nothing to show on the tile
            if (!ToggleShowHomework.IsOn && !ToggleShowExams.IsOn)
            {
                // Clear current content
                foreach (var tile in AllTiles())
                    tile.CreateTileUpdater().Clear();

                return;
            }

            TileBindingContentAdaptive bindingContent = new TileBindingContentAdaptive();

            // NOTE: In a real app, this data would probably be dynamically generated from actual user data

            // Add the date header
            bindingContent.Children.Add(new TileText()
            {
                Text = "In two days"
            });

            // Add exams
            if (ToggleShowExams.IsOn)
            {
                bindingContent.Children.Add(new TileText()
                {
                    Text = "Exam 2",
                    Style = TileTextStyle.CaptionSubtle
                });
            }

            // Add homework
            if (ToggleShowHomework.IsOn)
            {
                bindingContent.Children.Add(new TileText()
                {
                    Text = "Bookwork Pg 37-39",
                    Style = TileTextStyle.CaptionSubtle
                });

                bindingContent.Children.Add(new TileText()
                {
                    Text = "Lab report 4",
                    Style = TileTextStyle.CaptionSubtle
                });
            }

            TileBinding binding = new TileBinding()
            {
                Content = bindingContent
            };

            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = binding,
                    TileWide = binding,

                    Branding = TileBranding.NameAndLogo
                }
            };

            // And send the notification
            foreach (var tile in AllTiles())
                tile.CreateTileUpdater().Update(new TileNotification(content.GetXml()));
        }

        private void ToggleShowHomework_Toggled(object sender, RoutedEventArgs e)
        {
            UpdateTileNotifications();
        }

        private void ToggleShowExams_Toggled(object sender, RoutedEventArgs e)
        {
            UpdateTileNotifications();
        }
    }
}
