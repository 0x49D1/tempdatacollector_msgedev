using System;
using System.IO;
using System.Net;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Newtonsoft.Json;
using WPTermoClient.Common;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace WPTermoClient
{
    public sealed partial class Dashboard : Page
    {
        StatusBar statusBar = StatusBar.GetForCurrentView();

        public Dashboard()
        {
            this.InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                GetCurrentMeasurement();
            };
        }

        private void GetCurrentMeasurement()
        {

            statusBar.ProgressIndicator.ShowAsync();
            HttpWebRequest request =
                (HttpWebRequest)HttpWebRequest.Create(new Uri(ApplicationData.Current.LocalSettings.Values["weatherstation"].ToString()));

            request.BeginGetResponse(GetWeatherDataCallback, request);

        }

        private void GetWeatherDataCallback(IAsyncResult ar)
        {
            HttpWebRequest request = ar.AsyncState as HttpWebRequest;
            if (request != null)
            {
                try
                {
                    WebResponse response = request.EndGetResponse(ar);
                    var r = response.GetResponseStream();

                    using (StreamReader sr = new StreamReader(r))
                    {
                        var content = sr.ReadToEnd(); // Read the stream till the end as string

                        var tempData = new TempData();
                        tempData.TempDataAdapter(JsonConvert.DeserializeObject<Rootobject>(content));

                        double tem = tempData.Temperature; // To celsium tempData.c;
                        double h = tempData.Humidity;

                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => txtTemp.Text = (tem).ToString("F2") + "C");
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => txtHum.Text = (h).ToString("F1") + "%");
                    
                        Color c = Colors.White;
                        // Change the color based on "comfort"
                        string message = string.Empty;
                        
                        if (tem < 5)
                        {
                            c = Colors.DodgerBlue;
                            message = "Brrr... Its cold!";
                        }
                        else if (tem < 20)
                        {
                            c = Colors.Orange;
                            message = "You can wear the spring things!";
                        }
                        else if (tem < 35)
                        {
                            c = Colors.OrangeRed;
                            message = "Weather is comfy!";
                        }
                        else
                        {
                            c = Colors.Red;
                            message = "You will DIE. The sun is angry!";
                        }
                        if (h > 95)
                            message += " Hope you know how to swim!";
                        if (h > 80)
                            message += " You probably need an umbrella!";

                            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => txtMessage.Text += message);

                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => txtTemp.Foreground = new SolidColorBrush(c));
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => statusBar.HideAsync());

                    }
                }
                catch (WebException e)
                {
                    return;
                }
            }
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            GetCurrentMeasurement();
        }
    }

    public class TempData
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }

        public void TempDataAdapter(object ob)
        {
            if (ob.GetType() == typeof (Rootobject))
            {
                var a = (Rootobject) ob;
                Temperature = a.main.temp - 273.15;
                Humidity = a.main.humidity;
            }
            else if (ob.GetType() == typeof (RootobjectLocal))
            {
                var a = (RootobjectLocal)ob;
                Temperature = a.c;
                Humidity = a.h;
            }
        }
    }
}

