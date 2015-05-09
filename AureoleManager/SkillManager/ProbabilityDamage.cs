using System;

namespace AureoleManager.SkillManager {
    public class ProbabilityDamage : IComparable<ProbabilityDamage> {
        #region Properties

        public int Damage { get; private set; }
        public float CumulProba { get; set; }
        public float Proba { get; private set; }

        #endregion

        #region Constructors

        public ProbabilityDamage(float proba, float cumulProbability, int baseDamage, int bonusDamage) {
            Proba = proba;
            CumulProba = cumulProbability;
            if (bonusDamage < 0)
                Damage = 0;
            else
                Damage = baseDamage + bonusDamage;
        }

        public ProbabilityDamage(float proba, int damage) {
            Proba = proba;
            Damage = damage;
            CumulProba = 0;
        }

        #endregion

        #region Interfaces

        public int CompareTo(ProbabilityDamage obj) {
            return Damage - obj.Damage;
        }

        #endregion

        #region Public members

        public bool Merge(float proba, int damage) {
            if (Damage != damage)
                return false;
            Proba += proba;
            return true;
        }

        public static void PrintHeader() {
            Console.WriteLine("Proba\tCumul\tDamage");
        }

        public void Print(string offset = "") {
            Console.WriteLine(offset + "{0:0.##}%  \t{1:0.##}%\t{2}", Proba, CumulProba, Damage);
        }

        #endregion
    }
}