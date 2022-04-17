using System.Reactive;
using DynamicData;
using ReactiveUI;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Core.Entities.Abstractions;

public abstract class MusicPlayer : ReactiveObject
{
    public enum State
    {
        Playing,
        Stopped
    }

    public abstract ReactiveCommand<Unit, Unit> PlayCommand { get; }
    public abstract ReactiveCommand<Unit, Unit> PlayNextCommand { get; }
    public abstract ReactiveCommand<Unit, Unit> PlayPreviousCommand { get; }
    public abstract ReactiveCommand<Unit, Unit> StopCommand { get; }

#region Properties

    public abstract State CurrentState { get; }
    public abstract SourceCache<YTrack, string> Tracks { get; }
    public abstract YTrack Track { get; }

#endregion
}