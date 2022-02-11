using Binance.Net;
using Binance.Net.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CryptoInfo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        public BinanceClient client;
        public double lastPrice = -1;

        public MainWindow()
        {

            client = new BinanceClient(new BinanceClientOptions()
            {

            });

            InitializeComponent();

        }

        private async void GetPricesAsync(Object source, System.Timers.ElapsedEventArgs e)
        {

            var callResult = await client.Spot.Market.GetCurrentAvgPriceAsync("HOTUSDT");

            if (!callResult.Success)
            {
                Trace.WriteLine("Call result not successful!");
            } else
            {

                // Data is storet into callResult.Data

                Trace.WriteLine(callResult.Data.Price);
                
                if (lastPrice == -1)
                {
                    lastPrice = (double) callResult.Data.Price;
                    this.Dispatcher.Invoke(() =>
                    {
                        price.Content = callResult.Data.Price.ToString();
                        percent_diff.Content = "0.00";
                    });

                } else
                {
                    // Calculate diff
                    if (lastPrice != (double) callResult.Data.Price)
                    {
                        double diff = Math.Round((((double)callResult.Data.Price / lastPrice) - 1) * 100, 6);
                        Trace.WriteLine("DIFF: " + diff);
                        lastPrice = (double)callResult.Data.Price;
                        this.Dispatcher.Invoke(() =>
                        {
                            price.Content = callResult.Data.Price.ToString();
                            percent_diff.Content = diff.ToString();
                        });
                        
                    }
                    
                }

            }


        }

        private void get_data(object sender, RoutedEventArgs e)
        {

            var aTimer = new System.Timers.Timer();
            aTimer.Interval = 1000;

            aTimer.Elapsed += GetPricesAsync;

            aTimer.Enabled = true;

            //GetPricesAsync();

        }
    }
}
