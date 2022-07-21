using System.Reactive.Linq;
using Avalonia.Media.Imaging;
using Avayandex_Music.Core.Storages;
using Splat;
using Yandex.Music.Api.Models.Common.Cover;

namespace Avayandex_Music.Presentation.ViewModels.Controls;

public class CardControlViewModel : ViewModelBase
{
    public CardControlViewModel()
    {
        LoadCoverCommand = ReactiveCommand.CreateFromTask(LoadCover);

        this.WhenAnyValue(vm => vm.Cover)
            .Select(_ => Unit.Default)
            .InvokeCommand(LoadCoverCommand);
    }

#region Commands

    public ReactiveCommand<Unit, Unit> LoadCoverCommand { get; }

#endregion

#region Methods

    private async Task LoadCover()
    {
        var storage = Locator.Current.GetService<Storage>() ??
                      throw new NullReferenceException();

        if (Cover == null) return;

        var path = await storage.LoadCoverAsync(Cover);

        if (path != null) CoverBitmap = new Bitmap(path);
    }

#endregion

#region Fileds

    private ReactiveCommand<string, Unit> _command;
    private object _commandParameter;
    private Bitmap? _coverBitmap;
    private YCover? _cover;
    private object _header;
    private object _secondaryHeader;

#endregion

#region Properties

    public ReactiveCommand<string, Unit> Command
    {
        get => _command;
        set => this.RaiseAndSetIfChanged(ref _command, value);
    }

    public object CommandParameter
    {
        get => _commandParameter;
        set => this.RaiseAndSetIfChanged(ref _commandParameter, value);
    }

    public object Header
    {
        get => _header;
        set => this.RaiseAndSetIfChanged(ref _header, value);
    }

    public object SecondaryHeader
    {
        get => _secondaryHeader;
        set => this.RaiseAndSetIfChanged(ref _secondaryHeader, value);
    }

    public YCover? Cover
    {
        get => _cover;
        set => this.RaiseAndSetIfChanged(ref _cover, value);
    }

    public Bitmap? CoverBitmap
    {
        get => _coverBitmap;
        set => this.RaiseAndSetIfChanged(ref _coverBitmap, value);
    }

#endregion
}