using Script.Character;
using Script.Character.Coffee;
using Script.Projectile.Coffee;
using UnityEngine;

namespace Game
{
    public partial class CombatSystem
    {
        protected void OnCoffeeRangedAttackSomeone(CoffeePoopProjectile poop,GlortonFighter victim)
        {
            var attacker = poop.launcher;
            var setting = attacker.combat.setting;
            DamagePlayer(victim, setting.rangedDamage);
            victim.UnShock();
            MakeTargetFly(attacker,victim);     
            var finalForce =GetFinalForce(poop,victim,setting.rangedForce,setting.rangedForceOffset);
            victim.rb.AddForce(finalForce,ForceMode2D.Impulse); 

        }

        protected void OnCoffeeSpecialAttackSomeone(CoffeeFighter attacker, GlortonFighter victim)
        {
            var setting = attacker.combat.setting;
            var target = victim; 
            DamagePlayer(victim,setting.saDamage);
            MakeTargetFly(target,victim);
            var finalForce =GetFinalForce(attacker,victim,setting.saForce,setting.saForceOffset);
            target.rb.AddForce(finalForce,ForceMode2D.Impulse); 
        }
    }
}