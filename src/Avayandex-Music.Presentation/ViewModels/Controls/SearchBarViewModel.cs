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

        GetSuggestionsCommand = ReactiveCommand.CreateFromObservable(() => 
            Observable.StartAsync(GetSuggestions)
                .TakeUntil(CancelGetSuggestionsCommand));
        CancelGetSuggestionsCommand = ReactiveCommand.Create(() => { }, GetSuggestionsCommand.IsExecuting);
        
        NavigateToSearchResultCommand = ReactiveCommand.Create(NavigateToSearchResult);

        var searchTextObservable = this.WhenAnyValue(vm => vm.SearchText)
            .ObserveOn(RxApp.TaskpoolScheduler)
            .Throttle(TimeSpan.FromTicks(500))
            .Select(_ => Unit.Default);
        searchTextObservable.InvokeCommand(this, vm => vm.CancelGetSuggestionsCommand);
        searchTextObservable.InvokeCommand(this, vm => vm.GetSuggestionsCommand); // send suggestions request
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
    public ReactiveCommand<Unit, Unit> CancelGetSuggestionsCommand { get; }
    public ReactiveCommand<Unit, Unit> NavigateToSearchResultCommand { get; }

#endregion

#region Methods

    private async Task GetSuggestions(CancellationToken cancellationToken)
    {
        var api = new YandexMusicApi();
        var storage = AuthStorageService.GetInstance();

        var searchText = _searchText;
        var response = await api.Search.SuggestAsync(storage, searchText);

        Console.WriteLine($"GetSuggestions '{searchText}': IsCancellationRequested: {cancellationToken.IsCancellationRequested}");
        cancellationToken.ThrowIfCancellationRequested();

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