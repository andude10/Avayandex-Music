using System;
using Avalonia;
using Avalonia.ReactiveUI;
using Avayandex_Music.Presentation.ViewModels.Views;
using Avayandex_Music.Presentation.Views;
using ReactiveUI;
using Splat;

namespace Avayandex_Music.Presentation;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        Locator.CurrentMutable.Register(() => new HomeView(), typeof(IViewFor<HomeViewModel>));

        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
    }
}