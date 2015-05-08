using System;

namespace AureoleManager.SkillManager {
    class Probability : IComparable<Probability> {
        public float Proba { get; private set; }
        public float CumulProba { get; set; }
        public int BonusDamage { get; private set; }

        public Probability(float proba, int bonusDamage) {
            Proba = proba;
            BonusDamage = bonusDamage;
        }

        public static void PrintHeader() {
            Console.WriteLine("Proba\tCumul\tDamage");
        }

        public bool Merge(float proba, int bonusDamage) {
            if (BonusDamage == bonusDamage) {
                Proba += proba;
                return true;
            }
            return false;
        }

        public void Print() {
            if (BonusDamage >= 0)
                Console.WriteLine("{0}\t{1}%\tBase + (b * {2})", Proba, CumulProba, BonusDamage);
            else
                Console.WriteLine("{0}\t{1}%\t0", Proba, CumulProba);
        }

        public int CompareTo(Probability obj) {
            return BonusDamage - obj.BonusDamage;
        }
    }
}
