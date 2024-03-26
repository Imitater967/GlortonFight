using Game;
using Script.Projectile.Aubergine;
using Script.Projectile.Coffee;
using UnityEngine;

namespace Script.Character.Coffee
{
    public class CoffeeCombat: GlortonFighterCombat
    {
        public override void SpecialAttack()
        {
            base.SpecialAttack();
            var coffeeFighter = fighter as CoffeeFighter;
            var specialAttackArea = coffeeFighter.specialAttackCheckArea;
            var param = specialAttackArea.GetBoxCheckParam();
            //拳击是单体攻击
            Collider2D[] hit=Physics2D.OverlapBoxAll(param.center,param.size,0,checkLayer);
            
            EventManager.Instance.Combat.Coffee.OnCoffeeSpecialAttack?.Invoke(coffeeFighter);
            foreach (var targetCollider in hit)
            {
                if (targetCollider.TryGetComponent(out GlortonFighter target))
                { 
                    EventManager.Instance.Combat.Coffee.OnCoffeeSpecialAttackSomeone?.Invoke(coffeeFighter,target);
                }
            }
        }
        public override void RangedAttack()
        {
            base.RangedAttack();
            var ballFighter = (CoffeeFighter)fighter;
            EventManager.Instance.Combat.Coffee.OnCoffeeRangedAttack?.Invoke(ballFighter);
            if (ballFighter.rangedProjectilePrefab != null)
            {
                var projectile= Instantiate(ballFighter.rangedProjectilePrefab, ballFighter.rangedMuzzleTransform.position,ballFighter.rangedMuzzleTransform.rotation);
                projectile.transform.localScale = new Vector3(fighter.transform.localScale.x,1,1);
                projectile.GetComponent<CoffeePoopProjectile>().Init(fighter, fighter.rangedMuzzleTransform);
            }
            
        }
    }
}