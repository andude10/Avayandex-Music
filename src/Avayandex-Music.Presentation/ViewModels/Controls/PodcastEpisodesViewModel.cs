using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avayandex_Music.Core.Players.Audio.Track;
using Avayandex_Music.Core.Services;
using DynamicData;
using Splat;
using Yandex.Music.Api;
using Yandex.Music.Api.Models.Common;
using Yandex.Music.Api.Models.Playlist;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Presentation.ViewModels.Controls;

public class PodcastEpisodesViewModel : ViewModelBase, IRoutableViewModel
{

    public PodcastEpisodesViewModel(IScreen screen)
    {
        HostScreen = screen;
        LoadPodcastsCommand = ReactiveCommand.CreateFromTask(LoadPodcastsAsync);
        
        _episodes.Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _episodesCollection)
            .Subscribe();

        _episodes.Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Take(5)
            .Bind(out _episodesShortCollection)
            .Subscribe();
    }
    
#region Properties

    public ReadOnlyObservableCollection<YTrack> EpisodesCollection => _episodesCollection;
    public ReadOnlyObservableCollection<YTrack> EpisodesShortCollection => _episodesShortCollection;

#endregion

#region Fields

    private readonly SourceList<YTrack> _episodes = new();
    private readonly ReadOnlyObservableCollection<YTrack> _episodesCollection;
    private readonly ReadOnlyObservableCollection<YTrack> _episodesShortCollection;
    
#endregion
    
#region IRoutableViewModel

    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

#endregion

#region Commands

    public ReactiveCommand<Unit, Unit> LoadPodcastsCommand { get; }

#endregion

#region Methods

    private async Task LoadPodcastsAsync()
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var episodes = await api.Playlist.PodcastsAsync(storage);
        
        if (episodes != null)
        {
            foreach (var episode in episodes.Result.Tracks)
            {
                _episodes.Add(episode.Track);
            }
        }
    }

#endregion
}