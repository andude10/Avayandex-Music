using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Avayandex_Music.Presentation.Views.Controls;

public partial class SearchBarView : ReactiveUserControl<SearchBarViewModel>
{
    public SearchBarView()
    {
        this.WhenActivated(d =>
        {
            d(this.BindCommand(ViewModel, vm => vm.NavigateToSearchResultCommand,
                view => view.FindSearchButton));

            d(this.Bind(ViewModel, vm => vm.SearchText,
                view => view.FindSearchBox.Text));
            d(this.OneWayBind(ViewModel, vm => vm.SuggestionsCollection,
                view => view.FindSearchBox.Items));
        });

        AvaloniaXamlLoader.Load(this);
    }

#region Find Properties

    public AutoCompleteBox FindSearchBox => this.FindControl<AutoCompleteBox>("SearchBox");
    public Button FindSearchButton => this.FindControl<Button>("SearchButton");

#endregion
}