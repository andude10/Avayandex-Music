using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Core.Services;
using Avayandex_Music.Presentation.ViewModels;
using Avayandex_Music.Presentation.Views;
using MainWindow = Avayandex_Music.Presentation.Views.MainWindow;

namespace Avayandex_Music.Presentation;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (AuthorizationService.IsAuthorized())
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel()
                };
            }
            else
            {
                var viewModel = new LoginViewModel();
                
                viewModel.ShowMainWindow.RegisterHandler(context =>
                {
                    desktop.MainWindow = new MainWindow
                    {
                        DataContext = new MainWindowViewModel()
                    };
                });
                
                desktop.MainWindow = new LoginWindow()
                {
                    DataContext = viewModel
                };
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}