using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace ZaruPass.Program.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _greeting = "Welcome to Avalonia!";
        [ObservableProperty]
        private ObservableCollection<string> _list;

        public MainViewModel()
        {
            List = new ObservableCollection<string>()
            {
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1",
                "1"
            };
        }
    }
}
