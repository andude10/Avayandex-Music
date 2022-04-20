using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avayandex_Music.Core.Players.Audio.Music;
using Avayandex_Music.Core.Services;
using DynamicData;
using ReactiveUI;
using Splat;
using Yandex.Music.Api;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Presentation.ViewModels;

public class HomeViewModel : ViewModelBase, IRoutableViewModel
{

#region Fields

    private IMusicPlayer _musicPlayer;

#endregion
    
    public HomeViewModel(IScreen screen)
    {
        HostScreen = screen;
        FindTrackCommand = ReactiveCommand.CreateFromTask(FindTrack);
        PlayCommand = ReactiveCommand.Create(PlayTrack);
        StopCommand = ReactiveCommand.Create(StopTrack);
        _musicPlayer = Locator.Current.GetService<IMusicPlayer>() 
                       ?? throw new InvalidOperationException();
    }

#region IRoutableViewModel

    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

#endregion

#region Commands

    public ReactiveCommand<Unit, Unit> FindTrackCommand { get; }
    public ReactiveCommand<Unit, Unit> PlayCommand { get; }
    public ReactiveCommand<Unit, Unit> StopCommand { get; }

#endregion

#region Methods

    // test method
    public async Task FindTrack()
    {
        var authStorage = AuthStorageService.GetInstance();
        var api = new YandexMusicApi();
        var getTrackId = (await api.Library.GetLikedTracksAsync(authStorage)).Result.Library.Tracks.First().Id;
        var track = (await api.Track.GetAsync(authStorage, getTrackId)).Result.First();
        _musicPlayer.Tracks.Add(track);
    }

    // test method
    public void PlayTrack()
    {
        _musicPlayer.SelectCommand.Execute(0);
        _musicPlayer.PlayCommand.Execute();
    }
    
    // test method
    public void StopTrack()
    {
        _musicPlayer.PauseCommand.Execute();
    }

#endregion
    
}