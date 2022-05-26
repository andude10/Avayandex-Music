using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avayandex_Music.Core.Services;
using DynamicData;
using Yandex.Music.Api;

namespace Avayandex_Music.Presentation.ViewModels.Controls;

public class SearchBarViewModel : ViewModelBase
{
    public SearchBarViewModel(IScreen screen)
    {
        _hostScreen = screen;

        _searchSuggestions.Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _suggestionsCollection)
            .Subscribe();

        GetSuggestionsCommand = ReactiveCommand.CreateFromTask(GetSuggestions);
        NavigateToSearchResultCommand = ReactiveCommand.Create(NavigateToSearchResult);

        this.WhenAnyValue(vm => vm.SearchText)
            .Throttle(TimeSpan.FromMilliseconds(300))
            .Select(_ => Unit.Default)
            .InvokeCommand(this, vm => vm.GetSuggestionsCommand);
    }

#region Fields

    private readonly IScreen _hostScreen;
    private string _searchText;
    private readonly SourceList<string> _searchSuggestions = new();
    private readonly ReadOnlyObservableCollection<string> _suggestionsCollection;

#endregion

#region Properties

    public string SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }

    public ReadOnlyObservableCollection<string> SuggestionsCollection => _suggestionsCollection;

#endregion

#region Commands

    public ReactiveCommand<Unit, Unit> GetSuggestionsCommand { get; }
    public ReactiveCommand<Unit, Unit> NavigateToSearchResultCommand { get; }

#endregion

#region Methods

    private async Task GetSuggestions()
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var response = await api.Search.SuggestAsync(storage, _searchText);
        _searchSuggestions.Clear();
        _searchSuggestions.AddRange(response.Result.Suggestions);
    }

    private void NavigateToSearchResult()
    {
        var vm = new SearchResultViewModel(_hostScreen, _searchText);
        _hostScreen.Router.Navigate.Execute(vm);
    }

#endregion
}