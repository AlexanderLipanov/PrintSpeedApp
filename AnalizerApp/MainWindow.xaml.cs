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

        private Stopwatch _stopwatch = new();

        private string? _responseData = string.Empty;
        private long _totalCount = 0;
        private long _totalSeconds = 0;

        public MainWindow(IAnalizerManager analizerManager)
        {
            _analizer = analizerManager;
            InitializeComponent();
            DataContext = this;
            Values1 = new();
            Values2 = new();
            try
            {
                _stopwatch.Start();
                ListenAsync();
            }
            catch (Exception ex)
            {
                _stopwatch.Stop();
                Console.WriteLine(ex.Message);
            }
        }

        public ChartValues<double> Values1 { get; set; }
        public ChartValues<double> Values2 { get; set; }

        private async Task ListenAsync()
        {
            try
            {
                tcpListener.Start();

                while (true)
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    _responseData = string.Empty;

                    TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                    var stream = tcpClient.GetStream();

                    var reader = new StreamReader(stream);

                    _responseData = await reader.ReadLineAsync();
                    sw.Stop();
                    UpdateChart(sw.Elapsed.TotalSeconds);
                    //   Thread.Sleep(100);
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

        private string _dataLengeth = string.Empty;

        private void UpdateChart(double s)
        {
            double momentSpeed = 0;
            if (_responseData != null)
            {

                _totalSeconds = _stopwatch.Elapsed.Seconds;
                _totalCount = _responseData.Count();
                momentSpeed = _responseData.Count() / s;
            }
            Values1.Add(momentSpeed);
            Values2.Add(_totalCount / _totalSeconds);
        }
    }
}
