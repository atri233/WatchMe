using System.Windows;


namespace WpfUser;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void button1_Click(object sender, RoutedEventArgs e)
    {
        var connectWindow = new WpfConnect.MainWindow();
        connectWindow.ShowDialog();
    }
    
}