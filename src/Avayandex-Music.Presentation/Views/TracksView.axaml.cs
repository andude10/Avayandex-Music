using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using Avayandex_Music.Core.Playbacks;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Presentation.Views;

public partial class TracksView : ReactiveUserControl<TracksViewModel>
{
    public TracksView()
    {
        this.WhenActivated(d =>
        {
            d(this.OneWayBind(ViewModel, vm => vm.TracksCollection,
                view => view.FindTracksListBox.Items));

            if (ViewModel != null)
                d(ViewModel.TrackPlayer.StateChange.Subscribe(ChangePlayButtonIcon));
        });
        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public ListBox FindTracksListBox => this.FindControl<ListBox>(nameof(TracksListBox));

#endregion

#region Methods

    /// <summary>
    ///     Change the track button icon
    /// </summary>
    /// <param name="args">The current state of the player and the current track</param>
    private void ChangePlayButtonIcon((PlaybackState, YTrack) args)
    {
        Console.WriteLine("ChangePlayButtonIcon work");
        if (ViewModel == null ||
            args.Item1 == PlaybackState.Nothing) return;

        // I haven't figured out something better yet...
        // Find the current track in the visual elements (to change the button icon)
        Button? playingTrackButton = null;
        var pageLabels = this.GetVisualDescendants().OfType<Label>();
        foreach (var label in pageLabels)
        {
            // Find the Label with the name of the current track
            if (!label.Content.Equals(args.Item2.Title)) continue;
            // In the DockPanel where the Label is located, find the button that we are looking for
            if (label.Parent is not DockPanel itemPanel) continue;

            var panelButtons = itemPanel.GetVisualDescendants().OfType<Button>();
            foreach (var button in panelButtons)
            {
                if (!(button.Classes.Contains("Play-button") | button.Classes.Contains("Pause-button"))) continue;
                playingTrackButton = button;
                break;
            }
        }

        if (playingTrackButton == null) return;

        // Change button icon
        if (args.Item1 == PlaybackState.Playing)
        {
            playingTrackButton.Classes.Add("Pause-button");
            playingTrackButton.Classes.Remove("Play-button");
        }
        else
        {
            playingTrackButton.Classes.Remove("Pause-button");
            playingTrackButton.Classes.Add("Play-button");
        }
    }

#endregion
}