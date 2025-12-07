using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace KeyboardGestures.Core.Commands
{
    public enum CommandType
    {
        LaunchApp,
        LaunchWebpage,
        CopyCurrentPath,
        ToggleMute,
        TakeScreenshot, // This is only useful if you can just take a screenshot without this new win11 UI... if not should probably delete

    }
    public class CommandDefinition : INotifyPropertyChanged
    {
        private List<int> _sequence = new();
        public List<int> Sequence
        {
            get => _sequence;
            set
            {
                _sequence = value;
                OnPropertyChanged(); // Sequence changed
                OnPropertyChanged(nameof(SequenceAsTextList)); // computed
                OnPropertyChanged(nameof(SequenceText));
            }
        }
        public CommandType CommandType { get; set; } = CommandType.LaunchApp;

        public string? Description { get; set; }
        public string? ApplicationPath { get; set; } // only for LaunchApp type
        public string? Url { get; set; } // only for LaunchWebpage type



        [JsonIgnore]
        public string SequenceText =>
            string.Join(" ", Sequence.Select(CommandDisplayHelper.ToDisplayName));

        [JsonIgnore]
        public string DisplayDescription =>
            CommandDisplayHelper.GetDescription(this);

        [JsonIgnore]
        public string DisplayText =>
            $"{SequenceText}  –  {DisplayDescription}";
        [JsonIgnore]
        public List<string> SequenceAsTextList => Sequence.Select(CommandDisplayHelper.ToDisplayName).ToList();

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
