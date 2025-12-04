using Avalonia.Threading;
using KeyboardGestures.Core.Commands;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;

namespace KeyboardGestures.UI.ViewModels
{
    public class SettingsViewModel : ReactiveObject
    {
        private readonly CommandRegistry _registry;
        public ObservableCollection<CommandDefinition> Commands { get; private set; } = new();
        private CommandDefinition? _selected;
        public CommandDefinition? Selected
        {
            get => _selected;
            set
            {
                this.RaiseAndSetIfChanged(ref _selected, value);
                this.RaisePropertyChanged(nameof(HasSelected));
                this.RaisePropertyChanged(nameof(IsLaunchApp));
            }
        }
        public bool HasSelected => Selected != null;
        public bool IsLaunchApp => Selected?.CommandType == CommandType.LaunchApp;

        public ReactiveCommand<Unit, Unit> AddNew { get; }
        public ReactiveCommand<Unit, Unit> DeleteSelected { get; }
        public ReactiveCommand<Unit, Unit> SaveSelected { get; }
        public SettingsViewModel(CommandRegistry registry) 
        {
            _registry = registry;
            foreach (var cmd in registry.GetAll())
            {
                Commands.Add(cmd);
            }
            AddNew = ReactiveCommand.Create(AddNewCommand, outputScheduler: RxApp.MainThreadScheduler);
            DeleteSelected = ReactiveCommand.Create(DeleteSelectedCommand, outputScheduler: RxApp.MainThreadScheduler);
            SaveSelected = ReactiveCommand.Create(SaveSelectedCommand, outputScheduler: RxApp.MainThreadScheduler);

        }

        public void AddNewCommand()
        {
           
        }

        public void DeleteSelectedCommand()
        {

        }
        public void SaveSelectedCommand()
        {

        }
    }
}
