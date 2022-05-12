using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avayandex_Music.Presentation.ViewModels.Controls;
using ReactiveUI;

namespace Avayandex_Music.Presentation.Views.Controls;

public partial class PlayerDockView : ReactiveUserControl<PlayerDockViewModel>
{
    public PlayerDockView()
    {
        this.WhenActivated(d =>
        {
            d(this.BindCommand(ViewModel, vm => vm.TrackPlayer.PlayAsyncCommand,
                view => view.FindPlayButton));
            d(this.BindCommand(ViewModel, vm => vm.TrackPlayer.PauseCommand,
                view => view.FindPauseButton));
            d(this.BindCommand(ViewModel, vm => vm.TrackPlayer.SelectNextCommand,
                view => view.FindNextButton));
            d(this.BindCommand(ViewModel, vm => vm.TrackPlayer.SelectPreviousCommand,
                view => view.FindPreviousButton));

            d(this.Bind(ViewModel, vm => vm.TrackPlayer.SelectedTrack,
                view => view.FindCurrantTrackTextBlock.Content));
        });
        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public Label FindCurrantTrackTextBlock => this.FindControl<Label>("CurrantTrackLabel");
    public Button FindNextButton => this.FindControl<Button>("NextButton");
    public Button FindPreviousButton => this.FindControl<Button>("PreviousButton");
    public Button FindPlayButton => this.FindControl<Button>("PlayButton");
    public Button FindPauseButton => this.FindControl<Button>("PauseButton");

#endregion
}