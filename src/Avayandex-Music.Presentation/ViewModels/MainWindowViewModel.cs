using System;
using System.Collections.Generic;
using System.Text;
using Yandex.Music.Api;
using Yandex.Music.Api.Common;

namespace Avayandex_Music.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            var yanMusicApi = new YandexMusicApi();
            var authStorage = new AuthStorage();
        }
        public string Greeting => "Welcome to Avalonia!";
    }
}
