using Avalonia.Markup.Xaml;

namespace Avayandex_Music.Presentation.Views;

public partial class MyMusicView : ReactiveUserControl<MyMusicViewModel>
{
    public MyMusicView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}