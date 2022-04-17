using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Core.Services.Abstractions;
using Avayandex_Music.Core.Services.Implementations;
using Avayandex_Music.Presentation.ViewModels;
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