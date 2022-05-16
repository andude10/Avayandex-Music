using Aura.UI.Controls;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Presentation.ViewModels.Controls;

namespace Avayandex_Music.Presentation.Views.Controls;

public partial class SmartPlaylistsView : ReactiveUserControl<SmartPlaylistsViewModel>
{
    public SmartPlaylistsView()
    {
        this.WhenActivated(d =>
        {
            d(this.OneWayBind(ViewModel, vm => vm.PlaylistsCollection,
                view => view.FindPlaylistsCardCollection.Items));
        });
        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public CardCollection FindPlaylistsCardCollection =>
        this.FindControl<CardCollection>("PlaylistsCardCollection");

#endregion
}