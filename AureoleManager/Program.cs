using System;
using AureoleManager.SkillManager;

namespace AureoleManager {
    internal static class Program {
        #region Public members

        public static void Main() {
            test3();
        }

        #endregion

        #region Private members

        private static void test3() {
            AureolePlayers.Load(@"C:\Users\Sildra\Desktop\Dev\aureole.json");
            foreach (var player in AureolePlayers.Players) {
                player.Print();
            }
            Console.ReadKey();
        }

        private static void test1() {
            var sc1 = new SkillDamage(new Skill("Attack (Acc+1)", 2, 4, 1f, 0.5f));
            var sc2 = new SkillDamage("Attack", 3, 4, 1f, 0.5f);
            var combo = new SkillDamage(null, sc1, sc2);

            combo.GenerateGraph();
        }

        private static void test2() {
            var player = new Player("Merlu");
            player.AddSkill(new Skill("Attack", 2, 1, 1, 0));
            player.AddSkill(new Skill("Sneak", 2, 8, 2, 0));
            AureolePlayers.AddPlayer(player);
            AureolePlayers.Save(@"C:\Users\Sildra\Desktop\Dev\aureole.json");
            Console.ReadKey();
        }

        #endregion
    }
}