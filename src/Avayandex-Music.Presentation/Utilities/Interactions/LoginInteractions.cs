using System.Reactive;
using ReactiveUI;

namespace Avayandex_Music.Presentation.Utilities.Interactions;

public static class LoginInteractions
{
    public static readonly Interaction<Unit, Unit> HideLoadScreen = new(RxApp.MainThreadScheduler);

    public static readonly Interaction<Unit, Unit> ShowLoadScreen = new(RxApp.MainThreadScheduler);

    public static readonly Interaction<Unit, Unit> ShowMainWindow = new(RxApp.MainThreadScheduler);
}