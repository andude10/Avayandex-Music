using System;
using System.Reactive;
using System.Threading.Tasks;
using Avayandex_Music.Core.Services;
using Avayandex_Music.Presentation.Utilities.Interactions;
using ReactiveUI;
using Splat;

namespace Avayandex_Music.Presentation.ViewModels;

public class LoginViewModel : ViewModelBase
{
    public LoginViewModel()
    {
        TryLoginCommand = ReactiveCommand.CreateFromTask(TryLogin);
        TryAutoLoginCommand = ReactiveCommand.CreateFromTask(TryAutoLogin);
    }

#region Fields

    private string _password = "pass here";
    private string _login = "login here";

#endregion

#region Properties

    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    public string Login
    {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref _login, value);
    }

#endregion

#region Commands

    public ReactiveCommand<Unit, Unit> TryLoginCommand { get; }
    public ReactiveCommand<Unit, Unit> TryAutoLoginCommand { get; }

#endregion

#region Methods

    private async Task TryLogin()
    {
        var loginService = Locator.Current.GetService<ILoginService>()
                           ?? throw new InvalidOperationException();

        var isSuccessful = await loginService.AuthorizeAsync(Login, Password);

        if (isSuccessful) LoginInteractions.ShowMainWindow.Handle(Unit.Default);
    }

    private async Task TryAutoLogin()
    {
        LoginInteractions.ShowLoadScreen.Handle(Unit.Default).Subscribe();

        var loginService = Locator.Current.GetService<ILoginService>()
                           ?? throw new InvalidOperationException();

        var isSuccessful = await loginService.AuthorizeAsync();

        if (isSuccessful)
            LoginInteractions.ShowMainWindow.Handle(Unit.Default).Subscribe();
        else
            LoginInteractions.HideLoadScreen.Handle(Unit.Default).Subscribe();
    }

#endregion
}