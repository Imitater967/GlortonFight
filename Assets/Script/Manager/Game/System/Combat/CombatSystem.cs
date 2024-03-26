using System;
using Script;
using Script.Character;
using Script.Game;
using Script.Mechanism;
using Script.Projectile;
using UnityEngine;

namespace Game
{
     
    public partial class CombatSystem: System
    {
        public bool forceDebugging;
        public override void OnGameInit()
        {
            base.OnGameInit();
            if (_eventManager.debugging)
                forceDebugging = true;
        }

        public override void OnGamePrepared()
        {
            base.OnGamePrepared();
            var combatEvent = _eventManager.Combat;
            combatEvent.OnPlayerPunch += PlayerPunchDamage;
            combatEvent.OnPlayerKick += PlayerKickDamage;
            combatEvent.OnPlayerThrow += PlayerThrowDamage;
            combatEvent.OnPlayerUpPunch += PlayerUpPunchDamage;
            combatEvent.StrawBerry.OnStrawBerryRangedAttackSomeone += SBRangedDamage;
            combatEvent.StrawBerry.OnStrawBerrySpecialAttackSomeone += SBSpecialDamage;
            combatEvent.Ball.OnBallRangedAttackSomeone += BallRangedDamage;
            combatEvent.Ball.OnBallSpecialAttackSomeone += BallSpecialAttackSomeone;
            combatEvent.Peach.OnPeachBulletShootSomeone += OnPeachRangedAttack;
            combatEvent.Peach.OnPeachRocketDamageSomeone += OnPeachRocketExplodeDamage;
            combatEvent.Aubergine.OnAubergineRangedAttackSomeone += OnAubergineRangedAttackSomeone;
            combatEvent.Aubergine.OnAubergineSpecialAttackSomeone += OnAubergineSpecialAttackSomeone;
            combatEvent.Coffee.OnCoffeeRangedAttackSomeone += OnCoffeeRangedAttackSomeone;
            combatEvent.Coffee.OnCoffeeSpecialAttackSomeone += OnCoffeeSpecialAttackSomeone;
            combatEvent.Trash.OnTrashRangedAttackSomeone += OnTrashHit;
            _eventManager.Mechanism.OnPlayerTriggerExplosiveZone += PlayerExplodedDamage;
        }

        private void PlayerExplodedDamage(ExplosiveZone zone, GlortonFighter fighter)
        {
            Vector2 force=GetFinalForce(zone.transform.position, fighter, zone.Properties.ForceMultiplier,
                new Vector2(zone.Properties.HorizontalForce, zone.Properties.VerticalForce));
            fighter.RevDamage(zone.Properties.Damage);
            fighter.rb.AddForce(force,ForceMode2D.Impulse); 
            MakeTargetFly(fighter);
        }

        public Vector2 GetFinalForce(Vector3 pos, GlortonFighter victim,float force,Vector2 forceOffset)
        {
            var x=victim.transform.position.x-pos.x;
            if (x > 0)
            {
                x = 1;
            }

            if (x < 0)
            {
                x = -1;
                forceOffset.x = -forceOffset.x;
            }
            // if (forceDebugging)
            //     Debug.Log(x);
            Vector2 dir = new Vector2(x ,0);
            // if (forceDebugging)
            //     Debug.Log(dir);
            dir += forceOffset*new Vector2(x,1);
            // if (forceDebugging)
            //     Debug.Log(dir); 
            return dir * force * (1 + victim.combat.Damage);
        }
        protected Vector2 GetFinalForce(Projectile attacker, GlortonFighter victim,float force,Vector2 forceOffset)
        {
            var x=attacker.motion.velocity.x;
            if (x > 0)
            {
                x = 1;
            }

            if (x < 0)
            {
                x = -1;
                forceOffset.x = -forceOffset.x;
            }
            if (forceDebugging)
                Debug.Log(x);
            Vector2 dir = new Vector2(x ,0);
            if (forceDebugging)
                Debug.Log(dir);
            dir += forceOffset*new Vector2(x,1);
            if (forceDebugging)
                Debug.Log(dir);
            return dir * force * (1 + victim.combat.Damage);
        }
        protected Vector2 GetFinalForce(GlortonFighter attacker, GlortonFighter victim,float force,Vector2 forceOffset)
        {
            var x=attacker.transform.localScale.x;
            if (x > 0)
            {
                x = 1;
            }

            if (x < 0)
            {
                x = -1; 
            }
            if (forceDebugging)
                Debug.Log("dir"+x);
            Vector2 dir = new Vector2(x ,0);
            if (forceDebugging)
              Debug.Log("dir vector"+dir);
            dir += forceOffset*new Vector2(x,1);
            if (forceDebugging)
              Debug.Log("final dir"+dir);
            return dir * force * (1 + victim.combat.Damage);
        }

        protected void ShockPlayer(GlortonFighter fighter,GlortonFighter target)
        {
            var chrCombatSetting = fighter.combat.setting;
            var duration = Mathf.Clamp(chrCombatSetting.shockTime
                                       *chrCombatSetting.shockFactor
                                       * (1 + target.combat.Damage)
                                        ,0,chrCombatSetting.shockMaxTime);    
            target.Shock(duration);
        }
        protected void DamagePlayer(GlortonFighter target, float amount)
        {
            target.RevDamage(amount);
        }

        protected void MakeTargetFly(GlortonFighter attacker,GlortonFighter target)
        {
            var chrCombatSetting = attacker.combat.setting;
            var blockDur =
                Mathf.Clamp(
                    chrCombatSetting.inputBlockDuration * chrCombatSetting.inputBlockFactor *
                    (1 + target.combat.Damage), 0, chrCombatSetting.inputBlockMax);
            target.input.BlockInput(blockDur);
            target.animation.Fly();
        }

        public void MakeTargetFly(GlortonFighter target)
        {
            target.input.BlockInput(3);
            target.animation.Fly();
        }
        private void PlayerUpPunchDamage(GlortonFighter attacker, GlortonFighter victim)
        { 
            var setting = attacker.combat.setting;
            var target = victim;  
            MakeTargetFly(target,victim);
            DamagePlayer(victim,setting.upPunchDamage);
            var finalForce =GetFinalForce(attacker,victim,setting.upPunchForce,setting.upPunchForceOffset);
            target.rb.AddForce(finalForce,ForceMode2D.Impulse); 
        }

        protected void PlayerKickDamage(GlortonFighter attacker, GlortonFighter victim)
        {
            var setting = attacker.combat.setting;
            var target = victim;  
            DamagePlayer(victim,setting.kickDamage);
            MakeTargetFly(target,victim); 
            var finalForce =GetFinalForce(attacker,victim,setting.punchForce,setting.punchForceOffset);
            target.rb.AddForce(finalForce,ForceMode2D.Impulse); 

        }
        private void PlayerThrowDamage(GlortonFighter attacker, GlortonFighter victim)
        {               
            var attackerCombatSetting = attacker.combat.setting; 
            
            DamagePlayer(victim,attackerCombatSetting.throwDamage);
            Vector2 finalForce =GetFinalForce(attacker,victim,attackerCombatSetting.throwForce,attackerCombatSetting.throwForceOffset);
            MakeTargetFly(attacker,victim);
           
             victim.rb.AddForce(finalForce, ForceMode2D.Impulse);  
        }
        protected void PlayerPunchDamage(GlortonFighter attacker, GlortonFighter victim)
        { 
            var attackerCombatSetting = attacker.combat.setting;  
            DamagePlayer(victim,attackerCombatSetting.punchDamage);
            Vector2 finalForce =GetFinalForce(attacker,victim,attackerCombatSetting.punchForce,attackerCombatSetting.punchForceOffset);;
            MakeTargetFly(attacker,victim);
            victim.rb.AddForce(finalForce, ForceMode2D.Impulse);  
        }
    }
}