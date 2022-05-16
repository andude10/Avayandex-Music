using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Presentation.Views.Controls;

namespace Avayandex_Music.Presentation.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(d =>
        {
            d(this.Bind(ViewModel, vm => vm.PlayerDockViewModel,
                view => view.FindPlayerDockView.ViewModel));
        });
        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public PlayerDockView FindPlayerDockView => this.FindControl<PlayerDockView>("PlayerDockView");

#endregion
}