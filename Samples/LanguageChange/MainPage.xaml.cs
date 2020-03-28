using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LanguageChange
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel viewModel { get; set; }

        public MainPage()
        {
            viewModel = new MainPageViewModel();
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReloadView();
        }

        public void ReloadView()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null)
            {
                rootFrame.Navigate(typeof(MainPage));
            }
        }
    }
}