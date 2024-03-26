namespace Script.Character.FB
{
    public class BallSprite: GlortonFighterSprite
    {
        protected override void SpecialAttack()
        {
            base.SpecialAttack();
            FistOn();
            fighter.r_footRenderer.sprite = fighter.skinSheet.defaultSheet.l_foot;
        }
    }
}