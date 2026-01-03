
using KeyboardGestures.Core.Commands;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace KeyboardGestures.UI.ViewModels
{
    public class OverlayViewModel : ReactiveObject
    {
        public ObservableCollection<CommandDefinition> Commands { get; } = new();
        private readonly ICommandService _commandService;
        public OverlayViewModel(ICommandService commandService) 
        {
            _commandService = commandService;
            RefreshCommands();
        }

        public void RefreshCommands()
        {
            Commands.Clear();
            foreach (var cmd in _commandService.LoadAll())
                Commands.Add(cmd);
        }
    }
}
