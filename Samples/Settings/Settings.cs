using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Settings
{
    public class Settings
    {
        // https://docs.microsoft.com/en-us/uwp/api/Windows.Storage.ApplicationData
        private static readonly Lazy<Settings> instance = new Lazy<Settings>(() => new Settings());

        public static Settings Instance { get { return instance.Value; } }

        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        private Settings()
        {
        }

        private string testString;

        public string TestString
        {
            get => testString;
            set
            {
                localSettings.Values["TestString"] = value;
                testString = value;
            }
        }

        public enum Mode
        {
            [AttributeValue("Test1")]
            Test1,

            [AttributeValue("Test2")]
            Test2
        }

        private Mode testEnum;

        public Mode TestEnum
        {
            get => testEnum;
            set
            {
                localSettings.Values["TestEnum"] = value.GetAttributeValue<AttributeValue>();
                testEnum = value;
            }
        }

        private string serializedList;

        public string SerializedList
        {
            get => serializedList;
            set
            {
                serializedList = value;
                localSettings.Values["TestList"] = value;
            }
        }

        private TimeSpan testStruct;

        public TimeSpan TestStruct
        {
            get => testStruct;
            set
            {
                localSettings.Values["TestStruct"] = value;
                testStruct = value;
            }
        }

        private StorageFolder testLocalFolder;

        public StorageFolder TestLocalFolder
        {
            get { return testLocalFolder; }
            set
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("TestLocalFolder", value);
                testLocalFolder = value;
            }
        }

        public async Task initialize()
        {
            testString = localSettings.Values["testString"] as string;
            testEnum = localSettings.Values["TestEnum"] != null ? EnumExtension.GetEnumFromAttribute<Mode>(localSettings.Values["TestEnum"] as string) : Mode.Test1;
            serializedList = localSettings.Values["TestList"] as string;
            testStruct = localSettings.Values["TestStruct"] != null ? (TimeSpan)localSettings.Values["TestStruct"] : new TimeSpan(0, 0, 0);
            try
            {
                testLocalFolder = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFolderAsync("TestFolder");
            }
            catch
            {
                testLocalFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("TestFolder", CreationCollisionOption.OpenIfExists);
            }
        }
    }
}