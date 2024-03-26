using System;
using Script;
using Script.Character;
using Script.Game;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game
{
     
    public class SoundManager: MonoBehaviour
    {
        [NonSerialized]
        private SoundSetting _setting;

        private EventManager _eventManager=>ApplicationManager.Instance.EventManager;

        public  void RegCombatEvents()
        {
            _eventManager.Combat.OnPlayerKick += PlayAttackAudio;
            _eventManager.Combat.OnPlayerPunch += PlayAttackAudio;
            _eventManager.Combat.OnPlayerUpPunch += PlayAttackAudio;
            _eventManager.Combat.StrawBerry.OnStrawBerrySpecialAttack += (a) =>
            {
                PlayerClip(a.transform.position,_setting.strawBerrySpecialAttack);
            };
            _eventManager.Combat.StrawBerry.OnStrawBerryRangedAttack += (a) =>
            {  
                PlayerClip(a.lasterObj.transform.position,_setting.strawBerryRangedAttack);
            };
            _eventManager.Combat.Ball.OnBallSpecialAttack += (a) =>
            {
                PlayerClip(a.transform.position,_setting.ballSpecialAttack);
            };
            _eventManager.Combat.Ball.OnBallRangedAttackSomeone += (a,b) =>
            {
                PlayerClip(b.transform.position,_setting.ballRangedAttackSomeone);
            };
            _eventManager.Combat.Peach.OnPeachFireRocket += (a) =>
            {
                PlayerClip(a.transform.position,_setting.peachRocketLaunch);
            };
            _eventManager.Combat.Peach.OnPeachRocketExplode += (a) =>
            {
                PlayerClip(a.head.position,_setting.boom);
            };
            _eventManager.Combat.Peach.OnPeachFireBullet += (a) =>
            {
                PlayerClip(a.transform.position,_setting.peachGunFire);
            };
            _eventManager.Combat.Aubergine.OnAubergineSpecialAttack += (a) =>
            {
                PlayerClip(a.transform.position, _setting.aubergineSpecialAttack);
            };
            _eventManager.Combat.Coffee.OnCoffeeSpecialAttack += (a) =>
            {
                PlayerClip(a.transform.position, _setting.coffeeSpecialAttack);
            };
            _eventManager.Combat.Coffee.OnCoffeeRangedAttack += (a) =>
            {
                PlayFartAudio(a.transform.position);
            };
        }

        public void PlayFartAudio(Vector3 pos)
        {
            int n = Random.Range(0, _setting.coffeeFart.Length);
            var clip = _setting.coffeeFart[n];
            PlayerClip(pos,clip);
            // settings.coffeeFart[n] = settings.coffeeFart[0];
            // settings.coffeeFart[0] = clip;

        }
        public void PlayAttackAudio(GlortonFighter attacker, GlortonFighter victim)
        {
            int n = Random.Range(0, _setting.attackAudio.Length);
            var clip = _setting.attackAudio[n];
            var center = (attacker.transform.position + victim.transform.position) / 2;
            AudioSource.PlayClipAtPoint(clip.audioClip,center,_setting.Volume*clip.volume);;
            // move picked sound to index 0 so it's not picked next time
            // settings.attackAudio[n] = settings.attackAudio[0];
            // settings.attackAudio[0] = clip;
        }

        protected void PlayerClip(Vector3 pos, AudioAsset clip)
        {
            AudioSource.PlayClipAtPoint(clip.audioClip,pos,_setting.Volume*clip.volume);
        }

        public void Init(SoundSetting soundSetting)
        {
            this._setting = soundSetting;
        }
    }
}