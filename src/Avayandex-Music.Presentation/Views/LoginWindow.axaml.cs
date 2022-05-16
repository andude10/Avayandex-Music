using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Presentation.Utilities.Interactions;
using ReactiveMarbles.ObservableEvents;

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

        this.Events().Opened
            .SelectMany(async args =>
            {
                if (ViewModel == null) return Unit.Default;

                var isSuccessful = await ViewModel!.TryAutoLoginCommand.Execute();
                if (!isSuccessful) return Unit.Default;

                if (Application.Current?.ApplicationLifetime is not
                    IClassicDesktopStyleApplicationLifetime desktop) return Unit.Default;

                Hide();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel()
                };
                desktop.MainWindow.Show();

                return Unit.Default;
            })
            .Subscribe();

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