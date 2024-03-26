using Script.Character;
using Script.Projectile;
using Script.Projectile.Peach;
using UnityEngine;

namespace Game
{
    public partial class CombatSystem
    {
        public void OnPeachRangedAttack(Projectile projectile,GlortonFighter victim)
        {
            var attacker = projectile.launcher;
            var setting = attacker.combat.setting;
            DamagePlayer(victim, setting.rangedDamage);
            victim.UnShock();
            MakeTargetFly(attacker,victim);     
            var finalForce =GetFinalForce(projectile,victim,setting.rangedForce,setting.rangedForceOffset);
            victim.rb.AddForce(finalForce,ForceMode2D.Impulse); 
        }
        public void OnPeachRocketExplodeDamage(Projectile projectile,GlortonFighter victim)
        {
            var attacker = projectile.launcher;
            var setting = attacker.combat.setting;
            var rocket = projectile as PeachRocket;
            DamagePlayer(victim, setting.rangedDamage);
            victim.UnShock();
            MakeTargetFly(attacker,victim);     
            var finalForce =GetFinalForce(rocket.head.position,victim,setting.saForce,setting.saForceOffset);
            victim.rb.AddForce(finalForce,ForceMode2D.Impulse); 
        }
    }
}