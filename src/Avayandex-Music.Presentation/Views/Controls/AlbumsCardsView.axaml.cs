using Aura.UI.Controls;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Presentation.ViewModels.Controls;
using Yandex.Music.Api.Models.Album;
using Yandex.Music.Api.Models.Artist;
using Yandex.Music.Api.Models.Playlist;

namespace Avayandex_Music.Presentation.Views.Controls;

public partial class AlbumsCardsView : ReactiveUserControl<CardsViewModel<YAlbum>>
{
    public AlbumsCardsView()
    {
        this.WhenActivated(d =>
        {
            d(this.OneWayBind(ViewModel, vm => vm.CardCollection,
                view => view.FindPlaylistsCardCollection.Items));
        });
        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public CardCollection FindPlaylistsCardCollection =>
        this.FindControl<CardCollection>("AlbumsCardCollection");

#endregion
}