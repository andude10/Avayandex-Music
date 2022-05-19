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
                view => view.FindPodcastEpisodes.ViewModel));
        });

        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public PlaylistsCardsView FindSmartPlaylists => this.FindControl<PlaylistsCardsView>("SmartPlaylists");
    public TracksListView FindPodcastEpisodes => this.FindControl<TracksListView>("PodcastEpisodes");

#endregion
}