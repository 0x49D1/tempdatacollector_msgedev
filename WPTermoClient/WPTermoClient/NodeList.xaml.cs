using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WPTermoClient.Common;

namespace WPTermoClient
{
    public sealed partial class NodeList : Page
    {
        private NavigationHelper navigationHelper;

        public NodeList()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.Loaded += (sender, args) =>
            {
                txtWeatherStationName.Text = "http://api.openweathermap.org/data/2.5/weather?id=611717";

            };

        }

        /// <summary>
        /// Adds an item to the list when the app bar button is clicked.
        /// </summary>
        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            settings.Values.Clear();
            settings.Values.Add("weatherstation",txtWeatherStationName.Text);

            Frame.Navigate(typeof (Dashboard));


        }
    }
}
