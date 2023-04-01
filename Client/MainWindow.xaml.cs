using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
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
            Socket clientSocket = new(
             AddressFamily.InterNetwork,
             SocketType.Stream,
             ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(endpoint);
                clientSocket.Send(
                  Encoding.UTF8.GetBytes(
                      messageTextBox.Text));

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Dispose();

                StringBuilder sb = new StringBuilder();
                String str = sb.ToString();
                Dispatcher.Invoke(() =>
                ChatLogs.Text +=
                str + "\n");
                //str = "Received at " + DateTime.Now;
                //socket.Send(
                //    Encoding.UTF8.
                //    GetBytes(str));
                //socket.Shutdown(
                //    SocketShutdown.Both);
                //socket.Close();

            }
            catch (Exception ex)
            {
                ChatLogs.Text += ex.Message + "\n";
            }

        }
    }
}
