using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Settings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Settings settings { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            settings = Settings.Instance;
        }

        private async void FolderSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                Settings.Instance.TestLocalFolder = folder;
            }
        }

        private void ListAddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ListText.Text))
            {
                Settings.Instance.TestList.Add(ListText.Text);
            }
        }

        private void ListDelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TestList.SelectedIndex >= 0)
            {
                Settings.Instance.TestList.RemoveAt(TestList.SelectedIndex);
            }
        }
    }
}