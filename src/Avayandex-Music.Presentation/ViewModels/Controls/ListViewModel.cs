using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using Yandex.Music.Api.Models.Common;

namespace Avayandex_Music.Presentation.ViewModels.Controls;

public class ListViewModel<T> where T : YBaseModel
{
#region Fields

    private readonly ReadOnlyObservableCollection<T> _collection;

#endregion

    public ListViewModel()
    {
        Source.Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _collection)
            .Subscribe();
    }

#region Properties

    public SourceList<T> Source { get; } = new();

    public ReadOnlyObservableCollection<T> Collection => _collection;

#endregion
}