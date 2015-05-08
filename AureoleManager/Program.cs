using AureoleManager.SkillManager;

namespace AureoleManager {
    static class Program {
        static void Main() {
            var sc1 = new SkillCalculator(new Skill("Attack (Acc+1)", 2, 4, 1f, 0.5f));
            var sc2 = new SkillCalculator("Attack", 3, 4, 1f, 0.5f);
            var combo = new SkillCalculator(null, sc1, sc2);
            
            combo.GenerateGraph();
        }
    }
}
