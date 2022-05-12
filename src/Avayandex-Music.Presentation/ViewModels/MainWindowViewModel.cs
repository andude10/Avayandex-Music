using System;
using System.Reactive;
using Avayandex_Music.Core.Players.Audio.Track;
using Avayandex_Music.Presentation.ViewModels.Controls;
using ReactiveUI;
using Splat;

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

        var trackPlayer = Locator.Current.GetService<ITrackPlayer>()
                          ?? throw new InvalidOperationException();
        PlayerDockViewModel.Create(trackPlayer);
    }

#region Properties

    public PlayerDockViewModel PlayerDockViewModel => PlayerDockViewModel.Instance;

#endregion

#region Navigation

    public RoutingState Router { get; } = new();

    public ReactiveCommand<Unit, IRoutableViewModel> NavigateToMyMusicCommand { get; }

    public ReactiveCommand<Unit, IRoutableViewModel> NavigateToHomeCommand { get; }

#endregion
}