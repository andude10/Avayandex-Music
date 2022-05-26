using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveMarbles.ObservableEvents;

namespace Avayandex_Music.Presentation.Views;

public partial class UserTracksView : ReactiveUserControl<UserTracksViewModel>
{
    public UserTracksView()
    {
        this.Events().AttachedToVisualTree
            .Select(_ => Unit.Default)
            .InvokeCommand(this, view => view.ViewModel!.LoadTracksCommand);
        
        this.WhenActivated(d =>
        {
            d(this.OneWayBind(ViewModel, vm => vm.TracksCollection,
                view => view.FindTracksListBox.Items));

            d(this.BindCommand(ViewModel, vm => vm.StartPlayCommand,
                view => view.FindStartPlayButton));
        });
        AvaloniaXamlLoader.Load(this);
    }
    
#region Find Properties

    public ListBox FindTracksListBox =>
        this.FindControl<ListBox>("TracksListBox");

    public Button FindStartPlayButton =>
        this.FindControl<Button>("StartPlayButton");

#endregion
}