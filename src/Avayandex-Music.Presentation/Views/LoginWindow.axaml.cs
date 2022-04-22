using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avayandex_Music.Presentation.Utilities.Interactions;
using Avayandex_Music.Presentation.ViewModels.Views;
using ReactiveUI;

namespace Avayandex_Music.Presentation.Views;

public partial class LoginWindow : ReactiveWindow<LoginViewModel>
{
    public LoginWindow()
    {
        LoginInteractions.ShowMainWindow.RegisterHandler(async _ =>
        {
            await Observable.Range(0, 1).ObserveOn(RxApp.MainThreadScheduler);
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

            Hide();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
            desktop.MainWindow.Show();
        });

        LoginInteractions.HideLoadScreen.RegisterHandler(async _ =>
        {
            await Observable.Range(0, 1).ObserveOn(RxApp.MainThreadScheduler);
            Show();
        });

        LoginInteractions.ShowLoadScreen.RegisterHandler(async _ =>
        {
            await Observable.Range(0, 1).ObserveOn(RxApp.MainThreadScheduler);
            Hide();
        });

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

#region Find Properties

    public TextBox FindLoginTextBox => this.FindControl<TextBox>("LoginTextBox");
    public TextBox FindPasswordTextBox => this.FindControl<TextBox>("PasswordTextBox");
    public Button FindTryLoginButton => this.FindControl<Button>("TryLoginButton");

#endregion
}