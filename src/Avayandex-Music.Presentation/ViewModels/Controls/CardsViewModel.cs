using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Media.Imaging;
using Avayandex_Music.Core.Storages;
using DynamicData;
using Splat;
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
        TryLoadCoverCommand = ReactiveCommand.CreateFromTask<CardControlViewModel?>(TryLoadCover);

        Source.Connect()
            .Transform(transformFactory)
            .DisposeMany()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _cardCollection)
            .Subscribe();

        Source.CountChanged
            .ObserveOn(RxApp.MainThreadScheduler)
            .Select(index => index != 0
                ? CardCollection[index - 1]
                : null)
            .InvokeCommand(TryLoadCoverCommand);
    }

#region Commands

    public ReactiveCommand<CardControlViewModel?, Unit> TryLoadCoverCommand { get; }

#endregion

#region Methods

    private async Task TryLoadCover(CardControlViewModel? card)
    {
        if (card is {CoverType: { }})
        {
            var storage = Locator.Current.GetService<Storage>() ??
                          throw new NullReferenceException();
            var path = await storage.LoadCoverAsync(card.CoverType);
            card.CoverBitmap = new Bitmap(path);
        }
    }

#endregion

#region Properties

    public ReadOnlyObservableCollection<CardControlViewModel> CardCollection => _cardCollection;
    public SourceList<T> Source { get; } = new();

#endregion
}