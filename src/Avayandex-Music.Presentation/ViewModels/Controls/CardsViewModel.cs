using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using Yandex.Music.Api.Models.Common;

namespace Avayandex_Music.Presentation.ViewModels.Controls;

public class CardsViewModel<T> where T : YBaseModel
{
#region Fields

    private readonly ReadOnlyObservableCollection<CardControlViewModel> _cardCollection;

#endregion

    /// <summary>
    ///     Initialization
    /// </summary>
    /// <param name="transformFactory">A transform 'T' into 'CardControlViewModel' function</param>
    public CardsViewModel(Func<T, CardControlViewModel> transformFactory)
    {
        Source.Connect()
            .Transform(transformFactory)
            .DisposeMany()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _cardCollection)
            .Subscribe();
    }

#region Properties

    public ReadOnlyObservableCollection<CardControlViewModel> CardCollection => _cardCollection;
    public SourceList<T> Source { get; } = new();

#endregion
}