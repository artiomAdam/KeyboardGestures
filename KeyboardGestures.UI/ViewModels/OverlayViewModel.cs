
using KeyboardGestures.Core.Commands;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;

namespace KeyboardGestures.UI.ViewModels
{
    public class OverlayViewModel : ReactiveObject
    {
        public ObservableCollection<CommandDefinition> Commands { get; } = new();

        public OverlayViewModel(ICommandService commandService) 
        {
            foreach (var cmd in commandService.LoadAll())
                Commands.Add(cmd);
        }
    }
}
