using System;
using System.Collections.Generic;
using System.Linq;
using AureoleManager.Display;

namespace AureoleManager.SkillManager {
    public class SkillDamage {
        #region Fields

        private readonly List<ProbabilityDamage> _hits = new List<ProbabilityDamage>();

        #endregion

        #region Properties

        public string Name { get; private set; }
        public float MeanValue { get; private set; }

        public List<ProbabilityDamage> Hits {
            get { return _hits; }
        }

        #endregion

        #region Constructors

        public SkillDamage(string name, uint minAccuracy, int baseDamage, float majorBonus, float minorBonus) {
            Name = name;
            MeanValue = 0;

            Build(minAccuracy, baseDamage, majorBonus, minorBonus);
        }

        public SkillDamage(string name, SkillDamage skill1, SkillDamage skill2) {
            float cumulProba = 0;

            // Override name if any
            if (name != null)
                Name = name;
            else
                Name = skill1.Name + " + " + skill2.Name;

            // Merge probabilities from both skills
            foreach (var item1 in skill1.Hits) {
                foreach (var item2 in skill2.Hits) {
                    Merge((item1.Proba * item2.Proba) / 100,
                        item1.Damage + item2.Damage);
                }
            }
            // Merge probabilities with same damage
            Hits.Sort();
            foreach (var item in Hits) {
                cumulProba += item.Proba;
                item.CumulProba = cumulProba;
            }
            CalculateMeanValue();
        }

        public SkillDamage(Skill skill) {
            Name = skill.Name;

            Build(skill.MinAccuracy, skill.BaseDamage, skill.MajorBonus, skill.MinorBonus);
        }

        #endregion

        #region Public members

        /// <summary>
        ///     Print the Hit probability and damage
        /// </summary>
        /// <param name="offset">String offset used for pretty print</param>
        public void Print(string offset = "") {
            Console.WriteLine(offset + "Skill {0}, Mean damage : {1}", Name, MeanValue);
            Probability.PrintHeader(offset);
            foreach (var item in Hits) {
                item.Print(offset);
            }
            Console.WriteLine();
        }

        /// <summary>
        ///     Generates the graph of the skill's damages
        /// </summary>
        public void GenerateGraph() {
            Excel.ToGraph(this, @"C:\Users\Sildra\Desktop\Dev" + Name);
        }

        #endregion

        #region Private members

        private void Merge(float proba, int damage) {
            if (Hits.Any(item => item.Merge(proba, damage))) {
                return;
            }
            Hits.Add(new ProbabilityDamage(proba, damage));
        }

        private void Build(uint minAccuracy, int baseDamage, float majorBonus, float minorBonus) {
            var pc = new ProbabilityCalculator(minAccuracy, majorBonus, minorBonus);
            foreach (var item in pc.Probabilities) {
                Hits.Add(new ProbabilityDamage(item.Proba, item.CumulProba,
                    baseDamage, item.BonusDamage));
            }
            CalculateMeanValue();
        }

        private void CalculateMeanValue() {
            foreach (var item in Hits) {
                MeanValue += (item.Proba * item.Damage);
            }
            MeanValue /= 100;
        }

        #endregion
    }
}