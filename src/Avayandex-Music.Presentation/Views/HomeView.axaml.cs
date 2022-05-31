using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Presentation.Views.Controls;
using ReactiveMarbles.ObservableEvents;

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
            d(this.Bind(ViewModel, vm => vm.SmartPlaylistsViewModel,
                view => view.FindSmartPlaylists.ViewModel));
            d(this.Bind(ViewModel, vm => vm.RecommendedEpisodesViewModel,
                view => view.FindPodcastEpisodesView.ViewModel));
            d(this.Bind(ViewModel, vm => vm.ChartTracksViewModel,
                view => view.FindChartTracks.ViewModel));
            
            d(this.Bind(ViewModel, vm => vm.SearchBarViewModel,
                view => view.FindSearchBarView.ViewModel));
        });

        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public PlaylistsCardsView FindSmartPlaylists => this.FindControl<PlaylistsCardsView>("SmartPlaylists");
    public TracksListView FindPodcastEpisodesView => this.FindControl<TracksListView>("PodcastEpisodes");
    public TracksListView FindChartTracks => this.FindControl<TracksListView>("ChartTracks");
    public SearchBarView FindSearchBarView => this.FindControl<SearchBarView>("SearchBarView");

#endregion
}