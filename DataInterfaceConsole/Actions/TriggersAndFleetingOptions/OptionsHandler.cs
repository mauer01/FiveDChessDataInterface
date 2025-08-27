using DataInterfaceConsole.Actions.Settings;
using System;
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
            AddOption(new SettingsValuePrimitive<string>("PasteLobbyCode", "paste the lobby code", "auto inserts the lobby code of a private match", ""));
            AddOption(new SettingsValuePrimitive<Trigger>("ResumeGame", "Resumes finished Games", "Changes values so that the game can be resumed even though it already ended", new Trigger()));
        }

        public void WriteOptionIntoMemory(ISettingsValue changedOption)
        {
            var di = Program.instance.di;
            switch (changedOption.Id)
            {
                case "UndoSubmittedMoves":
                    di.MemLocUndoMoveReducedByValue.SetValue((byte)(changedOption.GetValueAsString() == "off" ? 0xFF : 0x00));
                    break;
                case "PasteLobbyCode":
                    string[] pieceMap = new string[]
                        {
                        ":pawn_white:",
                        ":knight_white:",
                        ":bishop_white:",
                        ":rook_white:",
                        ":queen_white:",
                        ":king_white:",
                        ":pawn_black:",
                        ":knight_black:",
                        ":bishop_black:",
                        ":rook_black:",
                        ":queen_black:",
                        ":king_black:"
                        };
                    string[] value = changedOption.GetValueAsString().Trim().Replace("\\", "").Replace("\"", "").Split(" ");
                    int[] newValue = value.Select((curVal) =>
                    {
                        return Array.IndexOf(pieceMap, curVal);
                    }).Append(6).ToArray();
                    di.JoiningRoomCodeArray.SetValue(newValue);
                    break;
                case "ResumeGame":
                    var a = new[] { di.FinishGameButton, di.EndOfGameDesc, di.BackgroundColorChange, di.PropertyAtEndOfGame };
                    foreach (var curItem in a)
                    {
                        curItem.SetValue(0);
                    }
                    break;
            }
        }
        public static OptionsHandler CreateNew()
        {
            return new OptionsHandler();
        }
    }
}
