namespace Avayandex_Music.Presentation.Utilities.Interactions;

public static class LoginInteractions
{
    public static readonly Interaction<Unit, Unit> ShowMainWindow = new(RxApp.MainThreadScheduler);
}