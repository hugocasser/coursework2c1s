using System.Windows;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public RegisterWindow RegisterWindow { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectToServer_OnClick(object sender, RoutedEventArgs e)
        {
            RegisterWindow = new RegisterWindow();
            RegisterWindow.Show();
        }

        private async void CreateServer_Click(object sender, RoutedEventArgs e)
        {
            var message = new MessegBox("\n " +
                                        "     Server created");
            var server = new ServerObject();
            await server.ListenAsync();  
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}