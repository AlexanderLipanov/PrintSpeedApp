using AnalizeData;
using LiveCharts;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace AnalizerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpListener tcpListener = new TcpListener(IPAddress.Any, 55785);

        private readonly IAnalizerManager _analizer;


        private string _responseData = string.Empty;

        private double _totalSeconds = 0;
        private double _instantSeconds = 0;

        public MainWindow(IAnalizerManager analizerManager)
        {
            _analizer = analizerManager;

            InitializeComponent();

            DataContext = this;

            InstantSpeedSeries = new();
            AverageSpeedSeries = new();

            try
            {
                ListenAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public ChartValues<double> InstantSpeedSeries { get; set; }
        public ChartValues<double> AverageSpeedSeries { get; set; }

        private async Task ListenAsync()
        {
            try
            {
                tcpListener.Start();

                Stopwatch instantSopWatch = new();

                while (true)
                {
                    TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();                   

                    var stream = tcpClient.GetStream();
                    
                    var reader = new StreamReader(stream);
     
                    instantSopWatch.Start();

                    var data = await reader.ReadLineAsync();

                    instantSopWatch.Stop();

                    _totalSeconds += instantSopWatch.Elapsed.TotalSeconds;
                    _instantSeconds += instantSopWatch.Elapsed.TotalSeconds;

                    if (data is not null)
                    {                        
                        _responseData = data;
                        var currentCount = data.Count() - _responseData.Count();

                        var instantSpeed = _analizer.PrintSpeed(_instantSeconds, data.Count());
                        var averageSpeed = _analizer.PrintSpeed(_totalSeconds, _responseData.Count());

                        if (instantSpeed is not double.PositiveInfinity or double.NegativeInfinity
                            || averageSpeed is not double.PositiveInfinity or double.NegativeInfinity)
                        {
                            UpdateChart(instantSopWatch.Elapsed.TotalSeconds, averageSpeed);
                        }
                    }
                    else
                    {
                        instantSopWatch = new();
                        _instantSeconds = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                tcpListener.Stop();
            }
        }

        private void UpdateChart(double instantSpeed, double averageSpeed)
        {
            InstantSpeedSeries.Add(instantSpeed);
            AverageSpeedSeries.Add(averageSpeed);
        }
    }
}
