using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Presentation.ViewModels.Controls;
using Yandex.Music.Api.Models.Album;

namespace Avayandex_Music.Presentation.Views.Controls;

public partial class AlbumsCardsView : ReactiveUserControl<CardsViewModel<YAlbum>>
{
    public AlbumsCardsView()
    {
        this.WhenActivated(d =>
        {
            d(this.OneWayBind(ViewModel, vm => vm.CardCollection,
                view => view.FindAlbumsCardsListBox.Items));
        });
        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public ListBox FindAlbumsCardsListBox =>
        this.FindControl<ListBox>(nameof(AlbumsCardsListBox));

#endregion
}