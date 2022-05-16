using System.Linq;
using System.Reactive.Linq;
using Avayandex_Music.Core.Playbacks;
using Avayandex_Music.Core.Playbacks.Audio;
using Avayandex_Music.Core.Players.Audio.Track;
using Avayandex_Music.Core.Storages;
using DynamicData;
using Yandex.Music.Api;
using Yandex.Music.Api.Common;

namespace Avayandex_Music.Core.Tests.PlayersTest.Audio.Track;

public class TrackPlayerTests
{
    private const string TestTrack1Id = "27373919";
    private const string TestTrack2Id = "3277402";

#region LinuxStorage

#region VlcPlaybackAudio

    [Fact]
    public async Task Select_track()
    {
        // Arrange
        var api = new YandexMusicApi();
        var authStorage = new AuthStorage();
        var player = new TrackPlayer(new LinuxStorage(), new VlcPlaybackAudio());
        var track = (await api.Track.GetAsync(authStorage, TestTrack1Id)).Result.First();

        // Act
        player.Tracks.Add(track);
        player.SelectCommand.Execute(0).Subscribe();

        // Assert
        Assert.Equal(track, player.SelectedTrack);
    }

    [Fact]
    public async Task Select_next()
    {
        // Arrange
        var api = new YandexMusicApi();
        var authStorage = new AuthStorage();
        var player = new TrackPlayer(new LinuxStorage(), new VlcPlaybackAudio());
        var track1 = (await api.Track.GetAsync(authStorage, TestTrack1Id)).Result.First();
        var track2 = (await api.Track.GetAsync(authStorage, TestTrack2Id)).Result.First();

        // Act
        player.Tracks.Add(track1);
        player.Tracks.Add(track2);
        player.SelectCommand.Execute(0).Subscribe();
        player.SelectNextCommand.Execute().Subscribe();

        // Assert
        Assert.Equal(track2, player.SelectedTrack);
    }

    [Fact]
    public async Task Select_previous()
    {
        // Arrange
        var api = new YandexMusicApi();
        var authStorage = new AuthStorage();
        var player = new TrackPlayer(new LinuxStorage(), new VlcPlaybackAudio());
        var track1 = (await api.Track.GetAsync(authStorage, TestTrack1Id)).Result.First();
        var track2 = (await api.Track.GetAsync(authStorage, TestTrack2Id)).Result.First();

        // Act
        player.Tracks.Add(track1);
        player.Tracks.Add(track2);
        player.SelectCommand.Execute(1).Subscribe();
        player.SelectPreviousCommand.Execute().Subscribe();

        // Assert
        Assert.Equal(track1, player.SelectedTrack);
    }

    [Fact]
    public async Task Play_track()
    {
        // Arrange
        var api = new YandexMusicApi();
        var authStorage = new AuthStorage();
        var player = new TrackPlayer(new LinuxStorage(), new VlcPlaybackAudio());
        var track = (await api.Track.GetAsync(authStorage, TestTrack1Id)).Result.First();

        // Act
        player.Tracks.Add(track);
        player.SelectCommand.Execute(0).Subscribe();
        await player.PlayAsyncCommand.Execute();

        // Assert
        Assert.True(player.State == PlaybackState.Playing);
    }

    [Fact]
    public async Task Play_and_pause_track()
    {
        // Arrange
        var api = new YandexMusicApi();
        var authStorage = new AuthStorage();
        var player = new TrackPlayer(new LinuxStorage(), new VlcPlaybackAudio());
        var track = (await api.Track.GetAsync(authStorage, TestTrack1Id)).Result.First();

        // Act
        player.Tracks.Add(track);
        player.SelectCommand.Execute(0).Subscribe();
        await player.PlayAsyncCommand.Execute();
        player.PauseCommand.Execute().Subscribe();

        // Assert
        Assert.True(player.State == PlaybackState.Paused);
    }

#endregion

#endregion
}