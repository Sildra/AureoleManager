using System;

namespace AureoleManager.SkillManager {
    internal class Probability : IComparable<Probability> {
        #region Properties

        public float Proba { get; private set; }
        public float CumulProba { get; set; }
        public int BonusDamage { get; private set; }

        #endregion

        #region Constructors

        public Probability(float proba, int bonusDamage) {
            Proba = proba;
            BonusDamage = bonusDamage;
        }

        #endregion

        #region Interfaces

        public int CompareTo(Probability obj) {
            return BonusDamage - obj.BonusDamage;
        }

        #endregion

        #region Public members

        public static void PrintHeader(string offset = "") {
            Console.WriteLine(offset + "Proba\tCumul\tDamage");
        }

        public bool Merge(float proba, int bonusDamage) {
            if (BonusDamage != bonusDamage)
                return false;
            Proba += proba;
            return true;
        }

        public void Print(string offset = "") {
            if (BonusDamage >= 0)
                Console.WriteLine(offset + "{0}  \t{1}%\tBase + (b * {2})", Proba, CumulProba, BonusDamage);
            else
                Console.WriteLine(offset + "{0}  \t{1}%\t0", Proba, CumulProba);
        }

        #endregion
    }
}