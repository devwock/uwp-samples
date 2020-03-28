using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageChange
{
    public class MainPageViewModel : BindableBase
    {
        private string testText;

        public string TestText
        {
            get => testText;
            set => SetProperty(ref testText, value);
        }

        private LanguageManager languageManager { get; set; }

        public MainPageViewModel()
        {
            languageManager = LanguageManager.Instance;
            selectedLanguage = languageManager.CurrentLanguage;
            languageManager.LanguageChanged += LanguageManager_LanguageChanged;
            TestText = languageManager.GetString("TestText");
        }

        private void LanguageManager_LanguageChanged(object sender, LanguageEventArgs e)
        {
            TestText = languageManager.GetString("TestText");
            RaisePropertyChanged(nameof(TestEnumList));
        }

        private LanguageManager.SupportedLanguage selectedLanguage;

        public LanguageManager.SupportedLanguage SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                if (value != languageManager.CurrentLanguage)
                {
                    SetProperty(ref selectedLanguage, value);
                    languageManager.CurrentLanguage = SelectedLanguage;
                }
            }
        }

        public List<LanguageManager.SupportedLanguage> LanguageList
        {
            get => Enum.GetValues(typeof(LanguageManager.SupportedLanguage)).Cast<LanguageManager.SupportedLanguage>().ToList();
        }

        public enum TestEnum
        {
            [Localized("EnumText1")]
            Test1,

            [Localized("EnumText2")]
            Test2
        }

        public List<string> TestEnumList
        {
            get => Enum.GetValues(typeof(TestEnum))
                .Cast<TestEnum>()
                .Select(value => value.GetLocalizedAttributeValue())
                .ToList();
        }
    }
}