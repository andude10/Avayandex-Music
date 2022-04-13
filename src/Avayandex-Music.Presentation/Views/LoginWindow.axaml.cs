using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avayandex_Music.Presentation.ViewModels;
using ReactiveUI;

namespace Avayandex_Music.Presentation.Views;

public partial class LoginWindow : ReactiveWindow<LoginViewModel>
{
    public LoginWindow()
    {
        this.WhenActivated(d =>
        {   
            d(this.Bind(ViewModel, vm => vm.Login,
                view => view.FindLoginTextBox.Text));
            d(this.Bind(ViewModel, vm => vm.Password,
                view => view.FindPasswordTextBox.Text));

            d(this.BindCommand(ViewModel, vm => vm.TryLoginCommand,
                view => view.FindTryLoginButton));
        });
        AvaloniaXamlLoader.Load(this);
    }

    public TextBox FindLoginTextBox => this.FindControl<TextBox>("LoginTextBox");
    public TextBox FindPasswordTextBox => this.FindControl<TextBox>("PasswordTextBox");
    public Button FindTryLoginButton => this.FindControl<Button>("TryLoginButton");
}