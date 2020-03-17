namespace BindableBase
{
    public class MainPageViewModel : BindableBase
    {
        private string testText;

        public string TestText
        {
            get => testText;
            set => SetProperty(ref testText, value);
        }
    }
}