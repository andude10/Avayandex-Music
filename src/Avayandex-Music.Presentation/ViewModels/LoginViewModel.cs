using System;
using System.Reactive;
using System.Threading.Tasks;
using Avayandex_Music.Core.Services;
using ReactiveUI;

namespace Avayandex_Music.Presentation.ViewModels;

public class LoginViewModel : ViewModelBase
{
    public LoginViewModel()
    {
        TryLoginCommand = ReactiveCommand.CreateFromTask(TryLogin);
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

#endregion

#region Interactions

    public readonly Interaction<Unit, Unit> ShowMainWindow = new Interaction<Unit, Unit>();

#endregion

#region Methods

    private async Task TryLogin()
    {
        var isSuccessful = await AuthorizationService.Authorize(Login, Password);

        if (isSuccessful)
        {
            ShowMainWindow.Handle(Unit.Default);
        }
    }

#endregion
    
}