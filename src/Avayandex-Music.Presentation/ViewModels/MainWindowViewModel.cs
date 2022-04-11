using System.Reactive;
using Avayandex_Music.Infrastructure;
using ReactiveUI;

namespace Avayandex_Music.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _greeting = "Welcome to Avalonia!";

    public MainWindowViewModel()
    {
        ChangeGreetingCommand = ReactiveCommand.Create(ChangeGreeting);
        AuthorizeCommand = ReactiveCommand.Create(Authorize);
    }

    public string Greeting
    {
        get => _greeting;
        set => this.RaiseAndSetIfChanged(ref _greeting, value);
    }

    public ReactiveCommand<Unit, Unit> AuthorizeCommand { get; }

    public ReactiveCommand<Unit, Unit> ChangeGreetingCommand { get; }

    private void ChangeGreeting()
    {
        Greeting = "yup a";
    }

    private static void Authorize()
    {
    }
}