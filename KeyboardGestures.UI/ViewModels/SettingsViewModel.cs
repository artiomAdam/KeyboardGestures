using KeyboardGestures.Core.Commands;
using KeyboardGestures.Core.Events;
using KeyboardGestures.Core.JsonStorage;
using KeyboardGestures.Core.KeyboardHook;
using Microsoft.Win32;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace KeyboardGestures.UI.ViewModels
{
    public class SettingsViewModel : ReactiveObject
    {
        public ObservableCollection<CommandDefinition> Commands { get; private set; } = new();
        private CommandDefinition? _selected;
        public CommandDefinition? Selected
        {
            get => _selected;
            set
            {
                if (IsRecording)
                    CancelRecording();

                this.RaiseAndSetIfChanged(ref _selected, value);

                this.RaisePropertyChanged(nameof(HasSelected));
                this.RaisePropertyChanged(nameof(IsLaunchApp));
                this.RaisePropertyChanged(nameof(IsLaunchWebpage));

                if (_selected != null)
                {
                    Editing = new CommandDefinition(_selected);
                }
                else
                {
                    Editing = null;
                }
            }
        }
        public bool HasSelected => Selected != null;
        public bool IsLaunchApp => Selected?.CommandType == CommandType.LaunchApp; // to show application path on LaunchApp type
        public bool IsLaunchWebpage => Selected?.CommandType == CommandType.LaunchWebpage; // to show URL on LaunchWebpage type

        private bool _isRecording;
        public bool IsRecording   // to control the sequence entry in the box
        {
            get => _isRecording;
            set => this.RaiseAndSetIfChanged(ref _isRecording, value);
        }
        


        // New editing field to remove all the temporary stuff:
        private CommandDefinition? _editing;
        public CommandDefinition? Editing
        {
            get => _editing;
            private set => this.RaiseAndSetIfChanged(ref _editing, value);
        }

        public IReadOnlyList<CommandType> CommandTypes { get; } = Enum.GetValues(typeof(CommandType)).Cast<CommandType>().ToList();

        // services
        private readonly IKeyboardHookService _keyboardHookService;

        private readonly ICommandService _commandService;

        // commands
        public ReactiveCommand<Unit, Unit> AddNew { get; }
        public ReactiveCommand<Unit, Unit> DeleteSelected { get; }
        public ReactiveCommand<Unit, Unit> SaveSelected { get; }
        public ReactiveCommand<Unit, Unit> AcceptSequence { get; }

        
        public SettingsViewModel(ICommandService commandService, IKeyboardHookService keyboard) 
        {
            _keyboardHookService = keyboard;
            _commandService = commandService;
            foreach (var cmd in commandService.LoadAll())
                Commands.Add(cmd);

            AddNew = ReactiveCommand.Create(AddNewCommand, outputScheduler: RxApp.MainThreadScheduler);
            DeleteSelected = ReactiveCommand.Create(DeleteSelectedCommand, outputScheduler: RxApp.MainThreadScheduler);
            SaveSelected = ReactiveCommand.Create(SaveSelectedCommand, outputScheduler: RxApp.MainThreadScheduler);
            AcceptSequence = ReactiveCommand.Create(AcceptSequenceCommand, outputScheduler: RxApp.MainThreadScheduler);

        }

        private void OnKey(KeyEvent ev)
        {
            if (!IsRecording) return;
            if (ev.Type != KeyEventType.KeyDown) return;
            int vk = ev.VirtualKeyCode;
            // ignore ctrl
            if (vk == 0x11 || vk == 0xA2 || vk == 0xA3)
                return;
            Editing?.AddToSequence(vk);

        }

        // start the recording of keys - clear the tempSequences
        private void BeginRecording()
        {
            if (Editing == null)
                return;

            Editing.ClearSequence();
            _keyboardHookService.KeyEventReceived += OnKey;
            IsRecording = true;
        }

        private void CancelRecording()
        {
            _keyboardHookService.KeyEventReceived -= OnKey;
            IsRecording = false;
        }

        public void OnSequenceInputLostFocus(bool acceptClicked)
        {
            if (acceptClicked) return;
            CancelRecording();
        }

        public void OnSequenceInputGotFocus()
        {
            BeginRecording();
        }

        private void AcceptSequenceCommand()
        {
            CancelRecording();
        }

        private void AddNewCommand()
        {
            var fresh = new CommandDefinition();
            Commands.Add(fresh);

            Selected = fresh;
        }

        private void DeleteSelectedCommand()
        {
            if (Selected == null) return;
            _commandService.Delete(Selected);
            Commands.Remove(Selected);
        }
        private void SaveSelectedCommand()
        {
            if (Editing == null || Selected == null)
                return;

            var oldSeq = Selected.Sequence.ToList();

            Selected.CommandType = Editing.CommandType;
            Selected.Description = Editing.Description;
            Selected.ApplicationPath = Editing.ApplicationPath;
            Selected.Url = Editing.Url;
            Selected.Sequence = Editing.Sequence.ToList();

            

            _commandService.UpdateCommand(Selected, oldSeq);
        }

        public void Dispose()
        {
            _keyboardHookService.KeyEventReceived -= OnKey;
        }

        
    }
}
