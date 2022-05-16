using Aura.UI.Controls;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Presentation.ViewModels.Controls;

namespace Avayandex_Music.Presentation.Views.Controls;

public partial class PodcastEpisodesView : ReactiveUserControl<PodcastEpisodesViewModel>
{
    public PodcastEpisodesView()
    {
        this.WhenActivated(d =>
        {
            d(this.OneWayBind(ViewModel, vm => vm.EpisodesShortCollection,
                view => view.FindEpisodesListBox.Items));
        });
        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public ListBox FindEpisodesListBox => this.FindControl<ListBox>("EpisodesListBox");

#endregion
}