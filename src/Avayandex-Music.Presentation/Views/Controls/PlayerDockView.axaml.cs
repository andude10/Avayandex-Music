using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Core.Playbacks;
using Avayandex_Music.Presentation.ViewModels.Controls;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Presentation.Views.Controls;

public partial class PlayerDockView : ReactiveUserControl<PlayerDockViewModel>
{
    public PlayerDockView()
    {
        this.WhenActivated(d =>
        {
            d(this.BindCommand(ViewModel, vm => vm.PlayOrPauseCommand,
                view => view.FindPlayOrPauseButton));
            d(this.BindCommand(ViewModel, vm => vm.TrackPlayer.SelectNextCommand,
                view => view.FindNextButton));
            d(this.BindCommand(ViewModel, vm => vm.TrackPlayer.SelectPreviousCommand,
                view => view.FindPreviousButton));

            d(this.Bind(ViewModel, vm => vm.TrackPlayer.SelectedTrack,
                view => view.FindCurrantTrackPresenter.Content));

            // refactor this(?)
            d(this.WhenAnyValue(view => view.ViewModel)
                .Subscribe(vm =>
                {
                    if (ViewModel == null) return;

                    d(ViewModel.WhenAnyValue(x => x.TrackPlayer)
                        .Subscribe(player => { ViewModel.TrackPlayer.StateChange.Subscribe(ChangePlayButtonIcon); }));
                }));
        });
        AvaloniaXamlLoader.Load(this);
    }

#region Methods

    /// <summary>
    ///     Change the track button icon
    /// </summary>
    /// <param name="args">The current state of the player and the current track</param>
    private void ChangePlayButtonIcon((PlaybackState, YTrack) args)
    {
        if (ViewModel == null ||
            args.Item1 == PlaybackState.Nothing) return;

        // Change button icon
        if (args.Item1 == PlaybackState.Playing)
        {
            FindPlayOrPauseButton.Classes.Add("Pause-button");
            FindPlayOrPauseButton.Classes.Remove("Play-button");
        }
        else
        {
            FindPlayOrPauseButton.Classes.Remove("Pause-button");
            FindPlayOrPauseButton.Classes.Add("Play-button");
        }
    }

#endregion

#region Find Properties

    public ContentControl FindCurrantTrackPresenter => this.FindControl<ContentControl>(nameof(CurrantTrackPresenter));
    public Button FindNextButton => this.FindControl<Button>(nameof(NextButton));
    public Button FindPreviousButton => this.FindControl<Button>(nameof(PreviousButton));
    public Button FindPlayOrPauseButton => this.FindControl<Button>(nameof(PlayOrPauseButton));

#endregion
}