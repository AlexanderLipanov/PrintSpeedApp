using AnalizeData;
using LiveCharts;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace AnalizerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        TcpListener tcpListener = new TcpListener(IPAddress.Any, 55785);

        private readonly IAnalizerManager _analizer;

        private long _start = DateTimeOffset.Now.ToUnixTimeSeconds();

        private double _totalSeconds = 0;
        private int _totalCount = 0;

        private double _instantSeconds = 0;
        private int _instantCount = 0;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private string _currentSymbol = string.Empty;
        public string CurrentSymbol
        {
            get
            {
                return _currentSymbol;
            }
            set
            {
                _currentSymbol = value;
                OnPropertyChanged(nameof(CurrentSymbol));
            }
        }


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

                while (true)
                {
                    TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                    var stream = tcpClient.GetStream();

                    var reader = new StreamReader(stream);

                    var data = await reader.ReadLineAsync();

                    if (!string.IsNullOrEmpty(data))
                    {
                        _instantCount++;
                        _totalCount++;

                        _totalSeconds += DateTimeOffset.Now.ToUnixTimeSeconds() - _start;

                        CurrentSymbol = data;

                        var instantSpeed = _analizer.PrintSpeed(_instantSeconds, _instantCount);
                        var averageSpeed = _analizer.PrintSpeed(_totalSeconds, _totalCount);

                        UpdateChart(instantSpeed, averageSpeed);
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
            if (instantSpeed != double.NegativeInfinity
                && instantSpeed != double.PositiveInfinity)
                InstantSpeedSeries.Add(instantSpeed);

            if (averageSpeed != double.NegativeInfinity
                && averageSpeed != double.PositiveInfinity)
                AverageSpeedSeries.Add(averageSpeed);
        }
    }
}
