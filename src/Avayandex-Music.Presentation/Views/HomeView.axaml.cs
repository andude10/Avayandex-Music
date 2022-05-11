using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avayandex_Music.Presentation.ViewModels;
using Avayandex_Music.Presentation.Views.Controls;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;

namespace Avayandex_Music.Presentation.Views;

public partial class HomeView : ReactiveUserControl<HomeViewModel>
{
    public HomeView()
    {
        this.Events().AttachedToVisualTree
            .Select(_ => Unit.Default)
            .InvokeCommand(this, view => view.ViewModel!.LoadDataCommand);

        this.WhenActivated(d =>
        {
            d(this.BindCommand(ViewModel, vm => vm.FindTrackCommand,
                view => view.FindFindTrackButton));
            d(this.BindCommand(ViewModel, vm => vm.PlayCommand,
                view => view.FindPlayTrackButton));
            d(this.BindCommand(ViewModel, vm => vm.StopCommand,
                view => view.FindStopTrackButton));

            d(this.Bind(ViewModel, vm => vm.SmartPlaylistsViewModel,
                view => view.FindSmartPlaylists.ViewModel));
        });

        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public Button FindFindTrackButton => this.FindControl<Button>("FindTrackButton");
    public Button FindPlayTrackButton => this.FindControl<Button>("PlayTrackButton");
    public Button FindStopTrackButton => this.FindControl<Button>("StopTrackButton");
    public SmartPlaylists FindSmartPlaylists => this.FindControl<SmartPlaylists>("SmartPlaylists");

#endregion
}