using System.Reactive;
using ReactiveUI;

namespace Avayandex_Music.Presentation.ViewModels;

public class MainWindowViewModel : ViewModelBase, IScreen
{
    public MainWindowViewModel()
    {
        NavigateToHomeCommand = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new HomeViewModel(this))
        );
        NavigateToMyMusicCommand = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new MyMusicViewModel(this))
        );
    }

#region Navigation

    public RoutingState Router { get; } = new();

    public ReactiveCommand<Unit, IRoutableViewModel> NavigateToMyMusicCommand { get; }

    public ReactiveCommand<Unit, IRoutableViewModel> NavigateToHomeCommand { get; }

#endregion
}