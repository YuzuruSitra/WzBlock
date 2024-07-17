using System;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.Obj.Enemy
{
    public class BulletPool : MonoBehaviour
    {
        [SerializeField]
        private Transform _bulletParent;
        
        [SerializeField]
        private BindBullet _bindBulletPrefab;
        private const int BindBulletPoolCount = 10;
        private readonly List<BindBullet> _availableBindBullets = new List<BindBullet>();
        private readonly List<BindBullet> _usedBindBullets = new List<BindBullet>();

        private void Awake()
        {
            // 初期プ�?�ルの作�??
            for (var i = 0; i < BindBulletPoolCount; i++)
            {
                var bullet = Instantiate(_bindBulletPrefab, _bulletParent, true);
                bullet.OnReturnToPool += ReturnBindPool;
                _availableBindBullets.Add(bullet);
            }
        }

        public BindBullet GetBullet()
        {
            BindBullet bullet;
            if (_availableBindBullets.Count <= 0)
            {
                bullet = Instantiate(_bindBulletPrefab, _bulletParent, true);
                bullet.OnReturnToPool += ReturnBindPool;
            }
            else
            {
                bullet = _availableBindBullets[0];
                _availableBindBullets.RemoveAt(0);
            }
            _usedBindBullets.Add(bullet);
            return bullet;
        }

        private void ReturnBindPool(BindBullet bindBullet)
        {
            bindBullet.ChangeLookActive(false);
            _usedBindBullets.Remove(bindBullet);
            _availableBindBullets.Add(bindBullet);
        }

        public void ReturnAllBlock()
        {
            if (_usedBindBullets.Count != 0)
            {
                for (var i = _usedBindBullets.Count - 1; i >= 0; i--)
                {
                    var bullet = _usedBindBullets[i];
                    bullet.ChangeLookActive(false);
                    _usedBindBullets.RemoveAt(i);
                    _availableBindBullets.Add(bullet);
                }
            }
            
        }

    }
}
