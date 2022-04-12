using System.Reactive;
using ReactiveUI;

namespace Avayandex_Music.Presentation.ViewModels;

public class MainWindowViewModel : ViewModelBase, IScreen
{
    public MainWindowViewModel()
    {
        GoNext = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new HomeViewModel(this))
        );
    }

#region Navigation

    public RoutingState Router { get; } = new RoutingState();
    
    public ReactiveCommand<Unit, IRoutableViewModel?> GoBack => Router.NavigateBack;
    
    public ReactiveCommand<Unit, IRoutableViewModel> GoNext { get; }

#endregion

}