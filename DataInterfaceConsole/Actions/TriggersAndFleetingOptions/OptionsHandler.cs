using DataInterfaceConsole.Actions.Settings;
using System.Collections.Generic;
using System.Linq;

namespace DataInterfaceConsole.Actions.TriggersAndFleetingOptions
{
    class OptionsHandler
    {

        public ISettingsValue[] GetOptions() => this.OptionsStore.Values.ToArray();

        private readonly Dictionary<string, ISettingsValue> OptionsStore = new Dictionary<string, ISettingsValue>(); // string : OptionsValue<T>
        private void AddOption(ISettingsValue s)
        {
            this.OptionsStore.Add(s.Id, s);
        }

        private ISettingsValue GetOption(string Id) => this.OptionsStore[Id];


        public OptionsHandler()
        {
            AddOption(new SettingsValueWhitelisted<string>("UndoSubmittedMoves", "Undo Submitted Moves", "unlocks the ability to Undo submitted moves in Single/Local Games",
                new[] { "on", "off" }, "off"));

        }

        public void WriteOptionIntoMemory(ISettingsValue changedOption)
        {
            var di = Program.instance.di;
            switch (changedOption.Id)
            {
                case "UndoSubmittedMoves":
                    di.MemLocUndoMoveReducedByValue.SetValue((byte)(changedOption.GetValueAsString() == "off" ? 0xFF : 0x00));
                    break;
            }
        }
        public static OptionsHandler LoadOrCreateNew()
        {
            var oh = new OptionsHandler();
            return oh;
        }
    }
}
