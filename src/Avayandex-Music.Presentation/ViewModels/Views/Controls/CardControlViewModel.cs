using System.Windows.Input;
using Aura.UI.Data;
using ReactiveUI;

namespace Avayandex_Music.Presentation.ViewModels.Views.Controls;

public class CardControlViewModel : ViewModelBase, ICardControlTemplate
{
    private ICommand _command;
    private object _commandParameter;
    private object _content;
    private object _header;
    private object _secondaryHeader;

    public ICommand Command
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

    public object Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }
}