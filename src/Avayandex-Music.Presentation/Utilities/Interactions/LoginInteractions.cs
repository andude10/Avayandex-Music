using System.Reactive;
using ReactiveUI;

namespace Avayandex_Music.Presentation.Utilities.Interactions;

public static class LoginInteractions
{
    public static readonly Interaction<Unit, Unit> HideLoadScreen = 
        new Interaction<Unit, Unit>(RxApp.MainThreadScheduler);
    
    public static readonly Interaction<Unit, Unit> ShowLoadScreen = 
        new Interaction<Unit, Unit>(RxApp.MainThreadScheduler);
    
    public static readonly Interaction<Unit, Unit> ShowMainWindow = 
        new Interaction<Unit, Unit>(RxApp.MainThreadScheduler);
}