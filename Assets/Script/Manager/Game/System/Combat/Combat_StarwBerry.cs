using Script.Character;
using UnityEngine;

namespace Game
{
    public partial class CombatSystem
    {
        private void SBSpecialDamage(StrawBerryFighter attacker,GlortonFighter victim)
        {
            var setting = attacker.combat.setting;
            var target = victim; 
            DamagePlayer(victim,setting.saDamage);
            MakeTargetFly(target,victim);
            var finalForce =GetFinalForce(attacker,victim,setting.saForce,setting.saForceOffset);
            target.rb.AddForce(finalForce,ForceMode2D.Impulse);
            Debug.Log(attacker.name +" "+(attacker.combat.Damage*100).ToString("f0")+"%"+" ranged "+
                      target.name+" "+(target.combat.Damage*100).ToString("f0")+"%"); 
        }
        private void SBRangedDamage(StrawBerryFighter attacker,GlortonFighter victim)
        {
            var setting = attacker.combat.setting;
            var target = victim;

            DamagePlayer(victim, setting.rangedDamage);
            MakeTargetFly(target,victim);
            var finalForce =GetFinalForce(attacker,victim,setting.rangedForce,setting.rangedForceOffset);
            target.rb.AddForce(finalForce,ForceMode2D.Impulse);
            Debug.Log(attacker.name +" "+(attacker.combat.Damage*100).ToString("f0")+"%"+" ranged "+
                      target.name+" "+(target.combat.Damage*100).ToString("f0")+"%"); 
        }
    }
}