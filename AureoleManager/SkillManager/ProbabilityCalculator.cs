using System;
using System.Collections.Generic;
using System.Linq;

namespace AureoleManager.SkillManager {
    /// <summary>
    ///     Base calculator for skills
    /// </summary>
    internal class ProbabilityCalculator {
        #region Fields

        private readonly float _majorBonus;
        private readonly uint _minAccuracy;
        private readonly float _minorBonus;
        private readonly List<Probability> _probabilities = new List<Probability>();

        #endregion

        #region Properties

        public IEnumerable<Probability> Probabilities {
            get { return _probabilities; }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructor of the Probability Calculator
        /// </summary>
        /// <param name="minAccuracy"></param>
        /// <param name="majorBonus"></param>
        /// <param name="minorBonus"></param>
        public ProbabilityCalculator(uint minAccuracy, float majorBonus = 0, float minorBonus = 0) {
            _minAccuracy = minAccuracy;
            _majorBonus = majorBonus;
            _minorBonus = minorBonus;
            Create();
        }

        #endregion

        #region Public members

        /// <summary>
        ///     Print the probability list
        /// </summary>
        public void Print(string offset = "") {
            Probability.PrintHeader(offset);
            foreach (var item in _probabilities) {
                item.Print(offset);
            }
        }

        #endregion

        #region Private members

        /// <summary>
        ///     Merge 2 probas with same damages
        /// </summary>
        /// <param name="proba">Proba to be merged</param>
        /// <param name="bonusDamage">Damage of the proba</param>
        private void Merge(float proba, int bonusDamage) {
            if (_probabilities.Any(item => item.Merge(proba, bonusDamage))) {
                return;
            }
            _probabilities.Add(new Probability(proba, bonusDamage));
        }

        /// <summary>
        ///     Create a new probability from a dice roll
        /// </summary>
        /// <param name="a">First dice</param>
        /// <param name="b">Second dice</param>
        private void CreateEntry(int a, int b) {
            // Fumble
            if ((a == 0 && (b == 0 || b == 1)) ||
                (a == 1 && (b == 0 || b == 2)) ||
                (a == 2 && b == 1))
                Merge(1f, -1);
            // Critic
            else if (a == b)
                Merge(1f, a * 2);
            // Miss
            else if (Math.Max(a, b) - Math.Min(a, b) < _minAccuracy)
                Merge(1f, -1);
            // Hit + minor
            else if (a < b)
                Merge(1f, (int)Math.Floor((b - a) * _minorBonus));
            // Hit + major
            else
                Merge(1f, (int)Math.Floor((a - b) * _majorBonus));
        }

        /// <summary>
        ///     Create the probability list
        /// </summary>
        private void Create() {
            float currentProba = 0;

            for (var i = 0; i < 10; i++) {
                for (var j = 0; j < 10; j++) {
                    CreateEntry(i, j);
                }
            }
            _probabilities.Sort();
            foreach (var item in _probabilities) {
                currentProba += item.Proba;
                item.CumulProba = currentProba;
            }
        }

        #endregion
    }
}