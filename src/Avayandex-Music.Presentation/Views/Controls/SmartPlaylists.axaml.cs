using Aura.UI.Controls;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avayandex_Music.Presentation.ViewModels.Views.Controls;
using ReactiveUI;

namespace Avayandex_Music.Presentation.Views.Controls;

public partial class SmartPlaylists : ReactiveUserControl<SmartPlaylistsViewModel>
{
    public SmartPlaylists()
    {
        this.WhenActivated(d =>
        {
            d(this.OneWayBind(ViewModel, vm => vm.PlaylistsCard,
                view => view.FindPlaylistsCardCollection.Items));
        });
        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public CardCollection FindPlaylistsCardCollection =>
        this.FindControl<CardCollection>("PlaylistsCardCollection");

#endregion
}