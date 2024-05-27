using System.Collections.Generic;
using UnityEngine;

namespace InGame.Obj.Enemy
{
    public class BulletPool : MonoBehaviour
    {
        [SerializeField]
        private Transform _bulletParent;
        [SerializeField]
        private Bullet _bulletPrefab;
        private const int LaunchPoolCount = 10;
        private readonly List<Bullet> _availableBullets = new List<Bullet>();
        private readonly List<Bullet> _usedBullets = new List<Bullet>();

        private void Awake()
        {
            // 初期プールの作成
            for (var i = 0; i < LaunchPoolCount; i++)
            {
                var bullet = Instantiate(_bulletPrefab, _bulletParent, true);
                bullet.OnReturnToPool += ReturnBlock;
                _availableBullets.Add(bullet);
            }
        }

        public Bullet GetBullet(Vector3 pos)
        {
            // プールから取得
            if (_availableBullets.Count <= 0)
            {
                var insBullet = Instantiate(_bulletPrefab, _bulletParent, true);
                insBullet.transform.position = pos;
                insBullet.OnReturnToPool += ReturnBlock;
                insBullet.ChangeLookActive(true);
                _usedBullets.Add(insBullet);
                return insBullet;
            }
            var bullet = _availableBullets[0];
            bullet.transform.position = pos;
            _availableBullets.RemoveAt(0);
            bullet.ChangeLookActive(true);
            _usedBullets.Add(bullet);
            return bullet;
        }

        private void ReturnBlock(Bullet bullet)
        {
            bullet.ChangeLookActive(false);
            _usedBullets.Remove(bullet);
            _availableBullets.Add(bullet);
        }

        public void ReturnAllBlock()
        {
            if (_usedBullets.Count == 0) return;
            for (var i = _usedBullets.Count - 1; i >= 0; i--)
            {
                var bullet = _usedBullets[i];
                bullet.ChangeLookActive(false);
                _usedBullets.RemoveAt(i);
                _availableBullets.Add(bullet);
            }
        }

    }
}
