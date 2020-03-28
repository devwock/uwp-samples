using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.UI.Core;

namespace LanguageChange
{
    public class LanguageManager
    {
        public enum SupportedLanguage
        {
            [AttributeValue("ko")]
            Korean,

            [AttributeValue("en-US")]
            English
        }

        private SupportedLanguage currentLanguage;

        public SupportedLanguage CurrentLanguage
        {
            get => currentLanguage;
            set
            {
                if (currentLanguage == value)
                {
                    return;
                }
                currentLanguage = value;
                ChangeLanguage(value);
            }
        }

        public event EventHandler<LanguageEventArgs> LanguageChanged;

        private static Lazy<LanguageManager> instance = new Lazy<LanguageManager>(() => new LanguageManager());
        public static LanguageManager Instance { get { return instance.Value; } }

        private LanguageManager()
        {
            ChangeLanguage(SupportedLanguage.Korean);
            var qualifierValues = ResourceContext.GetForCurrentView().QualifierValues;
            qualifierValues.MapChanged += new MapChangedEventHandler<string, string>(QualifierValues_MapChanged);
        }

        public string GetStringFromView(string key)
        {
            return GetString(ResourceLoader.GetForCurrentView(), key);
        }

        public string GetString(string key)
        {
            return GetString(ResourceLoader.GetForViewIndependentUse("Strings"), key);
        }

        private string GetString(ResourceLoader stringContext, string key)
        {
            return stringContext.GetString(key);
        }

        private void ChangeLanguage(SupportedLanguage targetLanguage)
        {
            ApplicationLanguages.PrimaryLanguageOverride = targetLanguage.GetAttributeValue<AttributeValue>();
            //ResourceContext.GetForViewIndependentUse().Reset();
            //ResourceContext.GetForCurrentView().Reset();
            //WaitUntilResourceContextRefreshed(targetLanguage);
        }

        private async void QualifierValues_MapChanged(IObservableMap<string, string> sender, IMapChangedEventArgs<string> @event)
        {
            var resourceLanguage = ResourceContext.GetForViewIndependentUse().QualifierValues["Language"].Split(';');
            if (resourceLanguage[0].Equals(currentLanguage.GetAttributeValue<AttributeValue>()))
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    while (true)
                    {
                        var viewLanguage = ResourceContext.GetForCurrentView().QualifierValues["Language"].Split(';');
                        if (viewLanguage[0].Equals(currentLanguage.GetAttributeValue<AttributeValue>()))
                        {
                            LanguageChanged?.Invoke(this, new LanguageEventArgs(currentLanguage));
                            break;
                        }
                        await Task.Delay(100);
                    }
                });
            }
        }

        //public void WaitUntilResourceContextRefreshed(SupportedLanguage language)
        //{
        //    Task.Run(async () =>
        //    {
        //        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
        //        {
        //            bool isChanged = false;
        //            while (!isChanged)
        //            {
        //                var resourceLanguage = ResourceContext.GetForViewIndependentUse().QualifierValues["Language"].Split(';');
        //                if (resourceLanguage[0].Equals(currentLanguage.GetAttributeValue<AttributeValue>()))
        //                {
        //                    var viewLanguage = ResourceContext.GetForCurrentView().QualifierValues["Language"].Split(';');
        //                    if (viewLanguage[0].Equals(currentLanguage.GetAttributeValue<AttributeValue>()))
        //                    {
        //                        isChanged = true;
        //                    }
        //                }
        //                await Task.Delay(100);
        //            }
        //            LanguageChanged?.Invoke(this, new LanguageEventArgs(language));
        //        });
        //    });
        //}
    }

    public class LanguageEventArgs : EventArgs
    {
        public LanguageManager.SupportedLanguage language { get; set; }

        public LanguageEventArgs(LanguageManager.SupportedLanguage language)
        {
            this.language = language;
        }
    }
}