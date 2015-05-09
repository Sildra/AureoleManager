using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace AureoleManager {
    [DataContract]
    internal static class AureolePlayers {
        #region Constants

        private static readonly DataContractJsonSerializer Serializer =
            new DataContractJsonSerializer(typeof (List<Player>));

        [DataMember] private static List<Player> _players = new List<Player>();

        #endregion

        #region Properties

        public static List<Player> Players {
            get { return _players; }
        }

        #endregion

        #region Public members

        public static void Load(string filename) {
            _players = (List<Player>)Serializer.ReadObject(File.OpenRead(filename));
            foreach (var player in _players) {
                player.Build();
            }
        }

        public static void Save(string filename) {
            Serializer.WriteObject(File.Open(filename, FileMode.CreateNew), _players);
        }

        public static void AddPlayer(Player player) {
            Players.Add(player);
        }

        #endregion
    }
}