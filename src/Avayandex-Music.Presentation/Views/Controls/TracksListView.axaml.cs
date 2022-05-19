using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Presentation.ViewModels.Controls;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Presentation.Views.Controls;

public partial class TracksListView : ReactiveUserControl<ListViewModel<YTrack>>
{
    public TracksListView()
    {
        this.WhenActivated(d =>
        {
            d(this.OneWayBind(ViewModel, vm => vm.Collection,
                view => view.FindTracksListBox.Items));
        });
        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public ListBox FindTracksListBox => this.FindControl<ListBox>("TracksListBox");

#endregion
}