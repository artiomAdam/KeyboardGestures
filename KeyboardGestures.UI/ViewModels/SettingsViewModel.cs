using KeyboardGestures.Core.Commands;
using KeyboardGestures.Core.Events;
using KeyboardGestures.Core.JsonStorage;
using KeyboardGestures.Core.KeyboardHook;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

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

        private bool _isRecording;
        public bool IsRecording   // to control the sequence entry in the box
        {
            get => _isRecording;
            set => this.RaiseAndSetIfChanged(ref _isRecording, value);
        }
        
        // the UI is bound to those, and those are eventually saved or discarded
        private List<int> _tempSequence = new();
        private List<string> _tempSequenceText = new();
        public string TempSequenceTextJoined => string.Join(" ", _tempSequenceText); // sequence text displayed on the sequence box

        // services
        private readonly IKeyboardHookService _keyboardHookService;
        private readonly IJsonStorageService _jsonStorageService;
        private readonly ICommandService _commandService;

        // commands
        public ReactiveCommand<Unit, Unit> AddNew { get; }
        public ReactiveCommand<Unit, Unit> DeleteSelected { get; }
        public ReactiveCommand<Unit, Unit> SaveSelected { get; }
        public ReactiveCommand<Unit, Unit> AcceptSequence { get; }

        
        public SettingsViewModel(ICommandService commandService, IKeyboardHookService keyboard, IJsonStorageService jsonStorageService) 
        {
            _jsonStorageService = jsonStorageService;
            _keyboardHookService = keyboard;
            _commandService = commandService;
            foreach (var cmd in commandService.LoadAll())
                Commands.Add(cmd);

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

        private void AcceptSequenceCommand()
        {
            if (Selected == null) return;
            CancelRecording();
        }

        private void AddNewCommand()
        {
            Selected = null;
            Selected = new CommandDefinition();
            ResetSequenceText(forceClear : true);
        }

        private void DeleteSelectedCommand()
        {
            _commandService.Delete(Selected);
            Commands.Remove(Selected);
        }
        private void SaveSelectedCommand()
        {
            if (Selected != null && _originalSelected != null)
            {
                if (!Commands.Contains(Selected))
                {
                    var oldSeq = Selected.Sequence.ToList();
                    Selected.Sequence = _tempSequence.ToList();
                    Commands.Add(Selected);
                    _commandService.AddNew(Selected);
                }
                else
                {
                    _commandService.UpdateSequence(Selected, _tempSequence.ToList());
                }
            }
        }

        public void Dispose()
        {
            _keyboardHookService.KeyEventReceived -= OnKey;
        }

        // sets the _tempSequenceText and _tempSequence to selected:
        // 1. when selected is changed
        // 2. when focus is lost (recording is stopped) and the field was empty
        // otherwise, it stays as is.
        private void ResetSequenceText(bool forceClear = false)
        {
            if (_selected == null) return;
            if (forceClear || _tempSequence.Count == 0)
            {
                _tempSequenceText = _selected.SequenceAsTextList.ToList();
                _tempSequence = _selected.Sequence.ToList();
            }
            this.RaisePropertyChanged(nameof(TempSequenceTextJoined));
        }
    }
}
