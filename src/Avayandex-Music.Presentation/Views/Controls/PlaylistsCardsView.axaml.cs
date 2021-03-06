using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Presentation.ViewModels.Controls;
using Yandex.Music.Api.Models.Playlist;

namespace Avayandex_Music.Presentation.Views.Controls;

public partial class PlaylistsCardsView : ReactiveUserControl<CardsViewModel<YPlaylist>>
{
    public PlaylistsCardsView()
    {
        this.WhenActivated(d =>
        {
            d(this.OneWayBind(ViewModel, vm => vm.CardCollection,
                view => view.FindPlaylistsCardsListBox.Items));
        });
        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public ListBox FindPlaylistsCardsListBox =>
        this.FindControl<ListBox>(nameof(PlaylistsCardsListBox));

#endregion
}