using System;
using System.Collections.Generic;
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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Net.WebSockets;
namespace Server
{
    public partial class MainWindow : Window
    {
        private Socket? listenSocket;
        private bool isServerStarted = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartServer_Click(object sender, RoutedEventArgs e)
        {
            IPEndPoint endpoint;
            try
            {
                IPAddress ip = IPAddress.Parse(serverIp.Text);
                int port = Convert.ToInt32(serverPort.Text);
               endpoint = 
                    new(ip, port);
            }
            catch
            {
                MessageBox.Show("Chack start network parameters");
                return;
            }
            listenSocket = new(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            new Thread(StartServerMethod).Start(endpoint);
            StartServer.IsEnabled = false;
            StopServer.IsEnabled = true;
            listenSocket = new(
        AddressFamily.InterNetwork,
        SocketType.Stream,
        ProtocolType.Tcp);

            new Thread(() => StartServerMethod(endpoint)).Start();
            isServerStarted = true;
            serverStatus.Background = new SolidColorBrush(Colors.Green);
            serverStatus.Content = "ON";
        }
        private void StartServerMethod(object? param)
        {
            if (listenSocket is null) return;
            IPEndPoint? endpoint = param as IPEndPoint;
            try
            {
                listenSocket.Bind(endpoint);
                listenSocket.Listen(100);
                Dispatcher.Invoke(() => 
                serverLogs.Text += "ServerStarted\n");
                byte[] buf = new byte[1024];
                while (true)
                {
                    Socket socket = 
                        listenSocket.Accept();

                    StringBuilder sb = new StringBuilder();
                    do
                    {
                        int n =
                            socket.Receive(buf);
                        sb.Append(System.Text.Encoding.UTF8
                            .GetString(buf, 0, n));
                    } while (socket.Available > 0);

                    String str = sb.ToString();
                    Dispatcher.Invoke(() =>
                    serverLogs.Text +=
                    str + "\n");

                    str = "Received at " + DateTime.Now;
                    socket.Send(
                        Encoding.UTF8.
                        GetBytes(str));
                    socket.Shutdown(
                        SocketShutdown.Both);
                    socket.Close();

                    
                }
            }
            catch(Exception ex) 
            {
                Dispatcher.Invoke(() =>
                serverLogs.Text +=
                "Server stopped" 
                + ex.Message + "\n");
            }
        }
        private void StopServer_Click(object sender, RoutedEventArgs e)
        {
            listenSocket?.Close();
            StartServer.IsEnabled = true;
            StopServer.IsEnabled = false;
            isServerStarted = false;
            serverStatus.Background = new SolidColorBrush(Colors.Red);
            serverStatus.Content = "OFF";
        }
    }
}
