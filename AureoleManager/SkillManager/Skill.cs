using System.Runtime.Serialization;

namespace AureoleManager.SkillManager {
    [DataContract]
    class Skill {
        [DataMember]
        string _name;
        [DataMember]
        uint _minAccuracy;
        [DataMember]
        int _baseDamage;
        [DataMember]
        float _majorBonus;
        [DataMember]
        float _minorBonus;

        SkillCalculator _skillCalculator;

        public Skill(string name, uint minAccuracy, int baseDamage, float majorBonus, float minorBonus) {
            _name = name;
            _minAccuracy = minAccuracy;
            _baseDamage = baseDamage;
            _majorBonus = majorBonus;
            _minorBonus = minorBonus;
            _skillCalculator = new SkillCalculator(_name, _minAccuracy,
                _baseDamage, _majorBonus, _minorBonus);
        }
    }
}
