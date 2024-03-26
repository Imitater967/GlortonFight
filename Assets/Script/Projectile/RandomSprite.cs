using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Projectile
{
    public class RandomSprite: MonoBehaviour
    {
        protected SpriteRenderer _renderer;
        public Sprite[] sprites;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            int n = Random.Range(0, sprites.Length);
            var sprite = sprites[n];
            _renderer.sprite = sprite;
        }
    }
}