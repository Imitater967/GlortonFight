namespace Script.Character.Peach
{
    public class PeachSprite: GlortonFighterSprite
    {
        protected PeachSkinSheet _sheet;
        protected override void OnInit()
        {
            base.OnInit();
            _sheet=fighter.skinSheet as PeachSkinSheet;

        }

        protected override void SpecialAttack()
        {
            base.SpecialAttack();
            fighter.r_footRenderer.sprite = fighter.skinSheet.defaultSheet.l_foot;
        }

        protected override void Ranged()
        {
            base.Ranged();
            fighter.r_handRenderer.sprite=_sheet.r_shotHand;
        }
    }
}