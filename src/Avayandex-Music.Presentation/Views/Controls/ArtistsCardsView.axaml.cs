using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Presentation.ViewModels.Controls;
using Yandex.Music.Api.Models.Artist;

namespace Avayandex_Music.Presentation.Views.Controls;

public partial class ArtistsCardsView : ReactiveUserControl<CardsViewModel<YArtist>>
{
    public ArtistsCardsView()
    {
        this.WhenActivated(d =>
        {
            d(this.OneWayBind(ViewModel, vm => vm.CardCollection,
                view => view.FindArtistsCardsListBox.Items));
        });
        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public ListBox FindArtistsCardsListBox =>
        this.FindControl<ListBox>(nameof(ArtistsCardsListBox));

#endregion
}