using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SignalRClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection _hubConnection;
        private ObservableCollection<string> _messages = new ObservableCollection<string>();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        public IEnumerable<string> Messages => _messages;
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MainWindow), new PropertyMetadata(null));


        public string UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register("UserName", typeof(string), typeof(MainWindow), new PropertyMetadata(null));



        private async void OnConnect(object sender, RoutedEventArgs e)
        {
            _hubConnection = new HubConnectionBuilder()
                .ConfigureLogging(logging =>
              {

              })
                .WithUrl("https://localhost:44321/mycomm")
                .Build();
            _hubConnection.On<string,string>("MyBroadcast", (name, message) =>
              {
                  _messages.Add($"{name}:{message}");
              });
           await  _hubConnection.StartAsync();
        }

        private async void OnSend(object sender, RoutedEventArgs e)
        {
           await _hubConnection.SendAsync("SomeMessage", UserName, Message);
        }
    }
}
