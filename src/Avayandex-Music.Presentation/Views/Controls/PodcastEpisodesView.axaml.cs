using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Presentation.ViewModels.Controls;
using Yandex.Music.Api.Models.Track;

namespace Avayandex_Music.Presentation.Views.Controls;

public partial class PodcastEpisodesView : ReactiveUserControl<ListViewModel<YTrack>>
{
    public PodcastEpisodesView()
    {
        this.WhenActivated(d =>
        {
            d(this.OneWayBind(ViewModel, vm => vm.Collection,
                view => view.FindEpisodesListBox.Items));
        });
        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public ListBox FindEpisodesListBox => this.FindControl<ListBox>("EpisodesListBox");

#endregion
}