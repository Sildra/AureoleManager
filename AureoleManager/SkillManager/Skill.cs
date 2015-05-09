using System.Runtime.Serialization;

namespace AureoleManager.SkillManager {
    [DataContract]
    public class Skill {
        #region Fields

        private SkillDamage _skillCalculator;

        #endregion

        #region Properties

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public uint MinAccuracy { get; private set; }

        [DataMember]
        public int BaseDamage { get; private set; }

        [DataMember]
        public float MajorBonus { get; private set; }

        [DataMember]
        public float MinorBonus { get; private set; }

        #endregion

        #region Constructors

        public Skill(string name, uint minAccuracy, int baseDamage, float majorBonus, float minorBonus) {
            Name = name;
            MinAccuracy = minAccuracy;
            BaseDamage = baseDamage;
            MajorBonus = majorBonus;
            MinorBonus = minorBonus;
            _skillCalculator = new SkillDamage(Name, MinAccuracy,
                BaseDamage, MajorBonus, MinorBonus);
        }

        #endregion

        #region Public members

        public void Build() {
            if (_skillCalculator == null)
                _skillCalculator = new SkillDamage(Name, MinAccuracy,
                    BaseDamage, MajorBonus, MinorBonus);
        }

        public void Print(string offset = "") {
            _skillCalculator.Print(offset);
        }

        #endregion
    }
}