using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Core.Playbacks.Audio;
using Avayandex_Music.Core.Players.Audio.Track;
using Avayandex_Music.Core.Services;
using Avayandex_Music.Core.Storages;
using Avayandex_Music.Presentation.ViewModels.Views;
using Avayandex_Music.Presentation.Views;
using Splat;

namespace Avayandex_Music.Presentation;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void RegisterServices()
    {
        Locator.CurrentMutable.Register(() => new LoginService(), typeof(ILoginService));
        Locator.CurrentMutable.Register(() => new LinuxStorage(), typeof(Storage));
        Locator.CurrentMutable.Register(() => new TrackPlayer(new LinuxStorage(), new VlcPlaybackAudio()),
            typeof(ITrackPlayer));

        base.RegisterServices();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new LoginWindow
            {
                DataContext = new LoginViewModel()
            };

        base.OnFrameworkInitializationCompleted();
    }
}