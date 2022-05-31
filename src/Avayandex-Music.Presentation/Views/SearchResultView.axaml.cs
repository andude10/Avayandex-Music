using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Presentation.Views.Controls;
using ReactiveMarbles.ObservableEvents;

namespace Avayandex_Music.Presentation.Views;

public partial class SearchResultView : ReactiveUserControl<SearchResultViewModel>
{
    public SearchResultView()
    {
        this.Events().AttachedToVisualTree
            .Select(_ => Unit.Default)
            .InvokeCommand(this, view => view.ViewModel!.SearchCommand);

        this.WhenActivated(d =>
        {
            d(this.Bind(ViewModel, vm => vm.SearchBarViewModel,
                view => view.FindSearchBarView.ViewModel));
            d(this.Bind(ViewModel, vm => vm.ArtistsCardsViewModel,
                view => view.FindArtistsCardsView.ViewModel));
            d(this.Bind(ViewModel, vm => vm.PlaylistsCardsViewModel,
                view => view.FindPlaylistsCardsView.ViewModel));
            d(this.Bind(ViewModel, vm => vm.AlbumsCardsViewModel,
                view => view.FindAlbumsCardsView.ViewModel));
            d(this.Bind(ViewModel, vm => vm.TracksViewModel,
                view => view.FindTracksList.ViewModel));
        });

        AvaloniaXamlLoader.Load(this);
    }


#region Find Properties

    public SearchBarView FindSearchBarView => this.FindControl<SearchBarView>("SearchBarView");
    public ArtistsCardsView FindArtistsCardsView => this.FindControl<ArtistsCardsView>("ArtistsCardsView");
    public PlaylistsCardsView FindPlaylistsCardsView => this.FindControl<PlaylistsCardsView>("PlaylistsCardsView");
    public AlbumsCardsView FindAlbumsCardsView => this.FindControl<AlbumsCardsView>("AlbumsCardsView");
    public TracksListView FindTracksList => this.FindControl<TracksListView>("TracksList");

#endregion
}