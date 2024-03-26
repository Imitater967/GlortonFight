using System;
using Script.Character;
using Script.Character.Aubergine;
using Script.Character.Coffee;
using Script.Character.FB;
using Script.Character.Peach;
using Script.Character.Trash;
using Script.Projectile;
using Script.Projectile.Aubergine;
using Script.Projectile.Coffee;
using Script.Projectile.Peach;
using Script.Projectile.Trash;

namespace Script.Game.Event
{
        public class CombatEvent
    {
        public Action<GlortonFighter,GlortonFighter> OnPlayerPunch;
        public Action<GlortonFighter, GlortonFighter> OnPlayerKick;
        public Action<GlortonFighter, GlortonFighter> OnPlayerThrow;
        public Action<GlortonFighter, GlortonFighter> OnPlayerUpPunch;
        public Action<GlortonFighter> OnPlayerDeath;
        public Action<GlortonFighter> OnPlayerRespawn;
        public Action<GlortonFighter, float> OnPlayerReceiveDamageClient;
        public Action<GlortonFighter, float> OnPlayerReceiveDamageServer;
        public StrawBerryEvent StrawBerry { get; private set; }
        public BallEvent Ball { get; }

        public PeachEvent Peach
        {
            get;
        }
        public AubergineEvent Aubergine { get; }
        public CoffeeEvent Coffee { get; }
        public TrashEvent Trash { get; }

        public CombatEvent()
        {
            Ball = new BallEvent();
            StrawBerry = new StrawBerryEvent();
            Peach = new PeachEvent();
            Aubergine = new AubergineEvent();
            Coffee = new CoffeeEvent();
            Trash = new TrashEvent();
        }
        public class TrashEvent
        {
            public Action<TrashFighter> OnTrashRangedAttack;
            public Action<TrashProjectile,GlortonFighter> OnTrashRangedAttackSomeone;
            public Action<TrashFighter> OnTrashSpecialAttack;
        }
        public class CoffeeEvent
        {
            public Action<CoffeeFighter> OnCoffeeSpecialAttack;
            public Action<CoffeeFighter,GlortonFighter> OnCoffeeSpecialAttackSomeone;
            public Action<CoffeeFighter> OnCoffeeRangedAttack;
            public Action<CoffeePoopProjectile,GlortonFighter> OnCoffeeRangedAttackSomeone;
        }
        public class AubergineEvent
        {
            public Action<AubergineFighter> OnAubergineSpecialAttack;
            public Action<AubergineFighter,GlortonFighter> OnAubergineSpecialAttackSomeone;
            public Action<AubergineFighter> OnAubergineRangedAttack;
            public Action<PencilProjectile, GlortonFighter> OnAubergineRangedAttackSomeone;
        }
        public class PeachEvent
        {
            public Action<PeachFighter> OnPeachFireRocket;
            public Action<PeachRocket> OnPeachRocketExplode;
            public Action<PeachRocket, GlortonFighter> OnPeachRocketDamageSomeone;
            public Action<PeachFighter> OnPeachFireBullet;
            public Action<PeachBullet, GlortonFighter> OnPeachBulletShootSomeone;
        }
        public class StrawBerryEvent
        {
            public Action<StrawBerryFighter> OnStrawBerrySpecialAttack;
            public Action<StrawBerryFighter, GlortonFighter> OnStrawBerrySpecialAttackSomeone;
            //草莓发出激光，不论是否打到人
            public Action<StrawBerryFighter> OnStrawBerryRangedAttack;
            //草莓发出激光，打到人
            public Action<StrawBerryFighter, GlortonFighter> OnStrawBerryRangedAttackSomeone;
        }
        public class BallEvent
        {
            public Action<BallFighter> OnBallRangedAttack;
            public Action<BallProjectile, GlortonFighter> OnBallRangedAttackSomeone;
            public Action<BallFighter> OnBallSpecialAttack;
            public Action<BallFighter, GlortonFighter> OnBallSpecialAttackSomeone;
        }

    } 
}