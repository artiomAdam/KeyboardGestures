
using KeyboardGestures.Core.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;

namespace KeyboardGestures.UI.ViewModels
{
    public class OverlayViewModel
    {
        public ObservableCollection<DisplayCommand> Commands { get; } = new();

        public OverlayViewModel(CommandRegistry registry) 
        {
            foreach (var cmd in registry.GetDisplayCommands())
            {
                Commands.Add(cmd);
            }
        }
    }
}
