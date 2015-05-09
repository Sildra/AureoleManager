using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using AureoleManager.SkillManager;

namespace AureoleManager {
    [DataContract]
    internal class Player {
        #region Constants

        private static readonly DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof (Player));

        #endregion

        #region Fields

        [DataMember] private Dictionary<string, Skill> _skills = new Dictionary<string, Skill>();

        #endregion

        #region Properties

        [DataMember]
        public string Name { get; private set; }

        public Dictionary<string, Skill> Skills {
            get { return _skills; }
        }

        #endregion

        #region Constructors

        public Player(string name) {
            Name = name;
        }

        #endregion

        #region Public members

        /// <summary>
        ///     Save the player into a JSon file
        /// </summary>
        /// <param name="filename">File to save the JSon player</param>
        public void Save(string filename) {
            Serializer.WriteObject(File.Open(filename, FileMode.CreateNew), this);
        }

        /// <summary>
        ///     Add a new skill to the player
        /// </summary>
        /// <param name="skill">Skill to add</param>
        public void AddSkill(Skill skill) {
            Skills.Add(skill.Name, skill);
        }

        /// <summary>
        ///     Display player informations in a text format
        /// </summary>
        public void Print() {
            Console.WriteLine("Player: {0}", Name);
            foreach (var skill in Skills.Values) {
                Console.WriteLine("  Skill: {0}", skill.Name);
                skill.Print("    ");
            }
        }

        /// <summary>
        ///     Build player informations after deserialization
        /// </summary>
        public void Build() {
            foreach (var skill in Skills.Values) {
                skill.Build();
            }
        }

        #endregion
    }
}