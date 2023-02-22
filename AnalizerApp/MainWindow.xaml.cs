using AnalizeData;
using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
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

        private long _averageStart = 0;
        private long _instantStart = 0;

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

        private string _responseData = string.Empty;

        private string _currentSymbol = string.Empty;
        public string CurrentSymbol
        {
            get=> "Нажатая клавиша: " + _currentSymbol;
            set
            {
                _currentSymbol = value;
                OnPropertyChanged(nameof(CurrentSymbol));
            }
        }

        private string _averageSymbol = string.Empty;
        public string AverageSymbol
        {
            get => "Среднее значение: " + _averageSymbol;
            set
            {
                _averageSymbol = value;
                OnPropertyChanged(nameof(AverageSymbol));
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

        public ChartValues<DateTimePoint> InstantSpeedSeries { get; set; }
        public ChartValues<DateTimePoint> AverageSpeedSeries { get; set; }


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
                        _responseData += data;

                        CurrentSymbol = data.LastOrDefault().ToString();

                        if (_instantCount == 0
                            && _totalCount == 0
                            && _averageStart == 0)
                            _averageStart = DateTimeOffset.Now.ToUnixTimeSeconds();

                        if (_instantCount == 0)
                            _instantStart = DateTimeOffset.Now.ToUnixTimeSeconds();

                        _instantCount++;
                        _totalCount++;

                        _totalSeconds = DateTimeOffset.Now.ToUnixTimeSeconds() - _averageStart;
                        _instantSeconds = DateTimeOffset.Now.ToUnixTimeSeconds() - _instantStart;

                        CurrentSymbol = data;

                        var instantSpeed = _analizer.PrintSpeed(_instantSeconds, _instantCount);
                        var averageSpeed = _analizer.PrintSpeed(_totalSeconds, _totalCount);

                        UpdateChart(instantSpeed, averageSpeed);
                        SetAverageSymbol();
                    }
                    else
                    {
                        CurrentSymbol = string.Empty;
                        _instantCount = 0;
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
                InstantSpeedSeries.Add(new()
                {
                    DateTime = DateTime.Now,
                    Value = instantSpeed,
                });

            if (averageSpeed != double.NegativeInfinity
                && averageSpeed != double.PositiveInfinity)
                AverageSpeedSeries.Add(new()
                {
                    DateTime = DateTime.Now,
                    Value = averageSpeed,
                });

            Debug.WriteLine(instantSpeed);
        }

        private void SetAverageSymbol()
        {
            if (string.IsNullOrEmpty(_responseData)) return;

            KeyValuePair<string, int> dataSymbol = new("", 0);
          
            for (var i = 0; i < _responseData.Count(); i++)
            {
                var p = _responseData.Substring(i, 1);

                if (p == null) continue;

                var t = _responseData.Where(e => e.ToString() == p).Count();

                if (t > dataSymbol.Value)
                    dataSymbol = new(p, t);

#if DEBUG
                Debug.WriteLine($"AverageIndexOf: {_responseData.Substring(i, 1)} / Count: {t}");
#endif
            }

            AverageSymbol = dataSymbol.Key;
        }
    }
}
