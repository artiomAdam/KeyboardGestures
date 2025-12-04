
using KeyboardGestures.Core.Commands;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;

namespace KeyboardGestures.UI.ViewModels
{
    public class OverlayViewModel : ReactiveObject
    {
        public ObservableCollection<CommandDefinition> Commands { get; } = new();

        public OverlayViewModel(CommandRegistry registry) 
        {
            foreach (var cmd in registry.GetAll())
            {
                Commands.Add(cmd);
            }
        }
    }
}
