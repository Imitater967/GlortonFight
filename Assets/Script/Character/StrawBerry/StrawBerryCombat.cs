using Game;
using UnityEngine;

namespace Script.Character
{
    public class StrawBerryCombat : GlortonFighterCombat
    {
        public BoxCollider2D specialAttackArea;
        public BoxCollider2D rangedAttackArea;
        public override void RangedAttack()
        {

            var param=rangedAttackArea.GetBoxCheckParam();
            //拳击是单体攻击
            Collider2D[] hit=Physics2D.OverlapBoxAll(param.center,param.size,0,checkLayer);
            
            EventManager.Instance.Combat.StrawBerry.OnStrawBerryRangedAttack?.Invoke((StrawBerryFighter)fighter); 
            foreach (var targetCollider in hit)
            {
                if (targetCollider.TryGetComponent(out GlortonFighter target))
                { 
                    EventManager.Instance.Combat.StrawBerry.OnStrawBerryRangedAttackSomeone?.Invoke((StrawBerryFighter)fighter,target); 
                }
            }
        }

        public override void SpecialAttack()
        {
            base.SpecialAttack();

            var param = specialAttackArea.GetBoxCheckParam();
            //拳击是单体攻击
            Collider2D[] hit=Physics2D.OverlapBoxAll(param.center,param.size,0,checkLayer);
            
            EventManager.Instance.Combat.StrawBerry.OnStrawBerrySpecialAttack?.Invoke((StrawBerryFighter)fighter); 
            foreach (var targetCollider in hit)
            {
                if (targetCollider.TryGetComponent(out GlortonFighter target))
                { 
                    EventManager.Instance.Combat.StrawBerry.OnStrawBerrySpecialAttackSomeone?.Invoke((StrawBerryFighter)fighter,target); 
                }
            }
        }
    }
}