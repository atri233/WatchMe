using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WatchMeWpfTestApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private bool _isDataDirty;
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object send, RoutedEventArgs s)
    {
        MessageBox.Show("点击触发");
    }
    
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        // If data is dirty, prompt user and ask for a response
        if (!_isDataDirty) return;
        var result = MessageBox.Show("Document has changed. Close without saving?",
            "Question",
            MessageBoxButton.YesNo);

        // User doesn't want to close, cancel closure
        if (result == MessageBoxResult.No)
            e.Cancel = true;
    }
}