using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using static Settings.Settings;

namespace Settings
{
    public class MainPageViewModel : BindableBase
    {
        public MainPageViewModel()
        {
            settings = Settings.Instance;
            if (!string.IsNullOrWhiteSpace(serializedList))
            {
                TestList = JsonConvert.DeserializeObject<ObservableCollection<string>>(serializedList);
            }
            else
            {
                TestList = new ObservableCollection<string>();
            }
            TestList.CollectionChanged += TestList_CollectionChanged;
        }

        public Settings settings { get; set; }

        private string testString;

        public string TestString
        {
            get => testString;
            set
            {
                SetProperty(ref testString, value);
                settings.TestString = value;
            }
        }

        private Mode testEnum;

        public Mode TestEnum
        {
            get => testEnum;
            set
            {
                SetProperty(ref testEnum, value);
                settings.TestEnum = value;
            }
        }

        private TimeSpan testStruct;

        public TimeSpan TestStruct
        {
            get => testStruct;
            set
            {
                SetProperty(ref testStruct, value);
                settings.TestStruct = value;
            }
        }

        private StorageFolder testLocalFolder;

        public StorageFolder TestLocalFolder
        {
            get { return testLocalFolder; }
            set
            {
                SetProperty(ref testLocalFolder, value);
                settings.TestLocalFolder = value;
            }
        }

        public ObservableCollection<string> TestList { get; set; }

        private string serializedList;

        public string SerializedList
        {
            get => serializedList;
            set
            {
                SetProperty(ref serializedList, value);
                settings.SerializedList = value;
            }
        }

        private void TestList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SerializedList = JsonConvert.SerializeObject(TestList);
        }

        private string listText;

        public string ListText
        {
            get => listText;
            set { SetProperty(ref listText, value); }
        }

        private int selectedIdx = -1;

        public int SelectedIdx
        {
            get => selectedIdx;
            set { SetProperty(ref selectedIdx, value); }
        }

        public ICommand FolderSelectCmd
        {
            get => new CommandHandler(async () => await SelectFolder());
        }

        public ICommand ListAddCmd
        {
            get => new CommandHandler(() => AddToList());
        }

        public ICommand ListDelCmd
        {
            get => new CommandHandler(() => DelList());
        }

        public List<Mode> TestEnumList
        {
            get => Enum.GetValues(typeof(Mode)).Cast<Mode>().ToList();
        }

        private async Task SelectFolder()
        {
            var folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                TestLocalFolder = folder;
            }
        }

        private void AddToList()
        {
            if (!string.IsNullOrWhiteSpace(ListText))
            {
                TestList.Add(ListText);
            }
        }

        private void DelList()
        {
            if (SelectedIdx >= 0)
            {
                TestList.RemoveAt(SelectedIdx);
            }
        }
    }
}