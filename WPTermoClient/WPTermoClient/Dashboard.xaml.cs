using WPTermoClient.Common;
using WPTermoClient.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media.Devices;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;

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
                (HttpWebRequest)HttpWebRequest.Create(new Uri(Windows.Storage.ApplicationData.Current.LocalSettings.Values["weatherstation"].ToString()));

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
                        var content = sr.ReadToEnd();

                        var tempData = JsonConvert.DeserializeObject<Rootobject>(content);

                        double tem = tempData.main.temp - 273.15;
                        Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => txtTemp.Text = (tem).ToString("F2") + "C");
                        Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => txtHum.Text = (tempData.main.humidity).ToString("F1") + "%");
                    
                        Color c = Colors.White;

                        if (tem < 5)
                        {
                            c = Colors.DodgerBlue;
                        }
                        else if (tem < 20)
                        {
                            c = Colors.Orange;

                        }
                        else if (tem < 35)
                        {
                            c = Colors.OrangeRed;
                        }
                        else
                        {
                            c = Colors.Red;
                        }
                        Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => txtTemp.Foreground = new SolidColorBrush(c));
                        Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => statusBar.HideAsync());

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
}

