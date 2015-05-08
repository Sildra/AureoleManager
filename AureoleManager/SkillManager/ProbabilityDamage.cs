using System;

namespace AureoleManager.SkillManager {
    class ProbabilityDamage : IComparable<ProbabilityDamage> {
        float _proba;
        float _cumulProba;
        readonly int _damage;

        public float GetProbability() { return _proba; }
        public int GetDamage() { return _damage; }
        public float GetCumulProba() { return _cumulProba; }
        public void SetCumulProba(float proba) { _cumulProba = proba; }

        public ProbabilityDamage(float proba, float cumulProbability, int baseDamage, int bonusDamage) {
            _proba = proba;
            _cumulProba = cumulProbability;
            if (bonusDamage < 0)
                _damage = 0;
            else
                _damage = baseDamage + bonusDamage;
        }

        public ProbabilityDamage(float proba, int damage) {
            _proba = proba;
            _damage = damage;
            _cumulProba = 0;
        }

        public bool Merge(float proba, int damage) {
            if (_damage == damage) {
                _proba += proba;
                return true;
            }
            return false;
        }

        public static void PrintHeader() {
            Console.WriteLine("Proba\tCumul\tDamage");
        }

        public void Print() {
            Console.WriteLine("{0:0.##}%\t{1:0.##}%\t{2}", _proba, _cumulProba, _damage);
        }

        public int CompareTo(ProbabilityDamage obj) {
            return _damage - obj._damage;
        }
    }
}
