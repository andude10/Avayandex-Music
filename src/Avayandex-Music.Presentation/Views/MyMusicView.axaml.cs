using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avayandex_Music.Presentation.ViewModels;
using ReactiveUI;

namespace Avayandex_Music.Presentation.Views;

public partial class MyMusicView : ReactiveUserControl<MyMusicViewModel>
{
    public MyMusicView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}