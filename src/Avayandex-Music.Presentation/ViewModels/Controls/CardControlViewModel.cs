using Avalonia.Media.Imaging;
using Yandex.Music.Api.Models.Common.Cover;

namespace Avayandex_Music.Presentation.ViewModels.Controls;

public class CardControlViewModel : ViewModelBase
{
    private ReactiveCommand<string, Unit> _command;
    private object _commandParameter;
    private object _content;
    private Bitmap? _coverBitmap;
    private YCover? _coverType;
    private object _header;
    private object _secondaryHeader;

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

    public YCover? CoverType
    {
        get => _coverType;
        set => this.RaiseAndSetIfChanged(ref _coverType, value);
    }

    public Bitmap? CoverBitmap
    {
        get => _coverBitmap;
        set => this.RaiseAndSetIfChanged(ref _coverBitmap, value);
    }
}