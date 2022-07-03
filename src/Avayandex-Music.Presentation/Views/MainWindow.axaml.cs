using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avayandex_Music.Presentation.Views.Controls;
using DynamicData;

namespace Avayandex_Music.Presentation.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(d =>
        {
            d(this.Bind(ViewModel, vm => vm.PlayerDockViewModel,
                view => view.FindPlayerDockView.ViewModel));

            if (ViewModel != null) d(ViewModel.Router.CurrentViewModel.Subscribe(HighlightNavigationButton));
        });
        AvaloniaXamlLoader.Load(this);

#if DEBUG
        this.AttachDevTools();
#endif
    }

#region Methods

    private void HighlightNavigationButton(IRoutableViewModel? viewModel)
    {
        if (ViewModel == null) return;

        var vmType = ViewModel.Router.GetCurrentViewModel()?.GetType();

        if (vmType == typeof(HomeViewModel))
        {
            if (FindNavigateToHomeButton.Classes.Contains("Nav-button"))
                FindNavigateToHomeButton.Classes.Replace("Nav-button", "Nav-button-selected");
        }
        else
        {
            FindNavigateToHomeButton.Classes.Remove("Nav-button-selected");
            FindNavigateToHomeButton.Classes.Add("Nav-button");
        }

        if (vmType == typeof(UserTracksViewModel))
        {
            if (FindNavigateToTracksButton.Classes.Contains("Nav-button"))
                FindNavigateToTracksButton.Classes.Replace("Nav-button", "Nav-button-selected");
        }
        else
        {
            FindNavigateToTracksButton.Classes.Remove("Nav-button-selected");
            FindNavigateToTracksButton.Classes.Add("Nav-button");
        }

        if (vmType == typeof(MyMusicViewModel))
        {
            if (FindNavigateToMyMusicButton.Classes.Contains("Nav-button"))
                FindNavigateToMyMusicButton.Classes.Replace("Nav-button", "Nav-button-selected");
        }
        else
        {
            FindNavigateToMyMusicButton.Classes.Remove("Nav-button-selected");
            FindNavigateToMyMusicButton.Classes.Add("Nav-button");
        }
    }

#endregion

#region Find Properties

    public PlayerDockView FindPlayerDockView => this.FindControl<PlayerDockView>(nameof(PlayerDockView));
    public Button FindNavigateToHomeButton => this.FindControl<Button>(nameof(NavigateToHomeButton));

    public Button FindNavigateToTracksButton => this.FindControl<Button>(nameof(NavigateToTracksButton));

    public Button FindNavigateToMyMusicButton => this.FindControl<Button>(nameof(NavigateToMyMusicButton));

#endregion
}