using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avayandex_Music.Presentation.ViewModels;
using ReactiveUI;

namespace Avayandex_Music.Presentation.Views;

public partial class PlaylistView : ReactiveUserControl<PlaylistViewModel>
{
    public PlaylistView()
    {
        this.WhenActivated(d =>
        {
            d(this.OneWayBind(ViewModel, vm => vm.TracksCollection,
                view => view.FindTracksListBox.Items));

            d(this.BindCommand(ViewModel, vm => vm.StartPlayCommand,
                view => view.FindStartPlayButton));
        });
        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public ListBox FindTracksListBox =>
        this.FindControl<ListBox>("TracksListBox");

    public Button FindStartPlayButton =>
        this.FindControl<Button>("StartPlayButton");

#endregion
}