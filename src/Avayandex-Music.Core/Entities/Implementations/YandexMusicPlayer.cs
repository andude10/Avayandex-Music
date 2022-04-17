using System.Reactive;
using Avayandex_Music.Core.Entities.Abstractions;
using DynamicData;
using ReactiveUI;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Core.Entities.Implementations;

public class YandexMusicPlayer : MusicPlayer
{
#region Fields

#endregion

    public YandexMusicPlayer()
    {
        var canPreviousNext = this.WhenAnyValue(
            mp => mp.Tracks.Count > 0);
    }

#region Properties

    public override State CurrentState { get; }
    public override SourceCache<YTrack, string> Tracks { get; } = new(t => t.Id);

    public override YTrack Track => Tracks.Items.Last();

#endregion

#region Commands

    public override ReactiveCommand<Unit, Unit> PlayCommand { get; }
    public override ReactiveCommand<Unit, Unit> PlayNextCommand { get; }
    public override ReactiveCommand<Unit, Unit> PlayPreviousCommand { get; }
    public override ReactiveCommand<Unit, Unit> StopCommand { get; }

#endregion

#region Methods

#endregion
}