using System;
using ReactiveUI;

namespace Avayandex_Music.Presentation.ViewModels;

public class HomeViewModel : ViewModelBase, IRoutableViewModel
{
    public HomeViewModel(IScreen screen)
    {
        HostScreen = screen;
    }
    
#region IRoutableViewModel

    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

#endregion 
    
}