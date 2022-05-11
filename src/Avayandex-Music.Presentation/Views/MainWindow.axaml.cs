using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avayandex_Music.Presentation.ViewModels;
using ReactiveUI;

namespace Avayandex_Music.Presentation.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}