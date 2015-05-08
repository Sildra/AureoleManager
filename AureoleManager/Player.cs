using System.Collections.Generic;
using System.Runtime.Serialization;
using AureoleManager.SkillManager;

namespace AureoleManager {
    [DataContract]
    class Player {
        [DataMember]
        string _name;
        [DataMember]
        List<Skill> _skills = new List<Skill>();

        public Player(string name) {
            _name = name;
        }

        public void Save(string filename) {

        }

        public void AddSkill(Skill skill) {
            _skills.Add(skill);
        }
    }
}
