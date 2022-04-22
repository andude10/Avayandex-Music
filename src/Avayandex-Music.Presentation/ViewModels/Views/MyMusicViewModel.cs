using System;
using ReactiveUI;

namespace Avayandex_Music.Presentation.ViewModels.Views;

public class MyMusicViewModel : ViewModelBase, IRoutableViewModel
{
    public MyMusicViewModel(IScreen screen)
    {
        HostScreen = screen;
    }

#region IRoutableViewModel

    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

#endregion
}