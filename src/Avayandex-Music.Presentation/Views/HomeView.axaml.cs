using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avayandex_Music.Presentation.ViewModels;
using ReactiveUI;

namespace Avayandex_Music.Presentation.Views;

public partial class HomeView : ReactiveUserControl<HomeViewModel>
{
    public HomeView()
    {
        this.WhenActivated(d =>
        {
            d(this.BindCommand(ViewModel, vm => vm.FindTrackCommand,
                view => view.FindFindTrackButton));
            d(this.BindCommand(ViewModel, vm => vm.PlayCommand,
                view => view.FindPlayTrackButton));
            d(this.BindCommand(ViewModel, vm => vm.StopCommand,
                view => view.FindStopTrackButton));
        });
        AvaloniaXamlLoader.Load(this);
    }
    
    

#region Find Properties

    public Button FindFindTrackButton => this.FindControl<Button>("FindTrackButton");
    public Button FindPlayTrackButton => this.FindControl<Button>("PlayTrackButton");
    public Button FindStopTrackButton => this.FindControl<Button>("StopTrackButton");

#endregion
}