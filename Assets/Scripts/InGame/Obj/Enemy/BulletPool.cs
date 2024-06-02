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
        
        [SerializeField]
        private WallBullet _wallBulletPrefab;
        private const int WallBulletPoolCount = 5;
        private readonly List<WallBullet> _availableWallBullets = new List<WallBullet>();
        private readonly List<WallBullet> _usedWallBullets = new List<WallBullet>();

        private void Awake()
        {
            // 初期プールの作成
            for (var i = 0; i < BindBulletPoolCount; i++)
            {
                var bullet = Instantiate(_bindBulletPrefab, _bulletParent, true);
                bullet.OnReturnToPool += ReturnBindPool;
                _availableBindBullets.Add(bullet);
            }
            for (var i = 0; i < WallBulletPoolCount; i++)
            {
                var bullet = Instantiate(_wallBulletPrefab, _bulletParent, true);
                bullet.OnReturnToPool += ReturnWallPool;
                _availableWallBullets.Add(bullet);
            }
        }

        public void GetBullet(Vector3 pos, EnemyShooter.BulletType type)
        {
            switch (type)
            {
                case EnemyShooter.BulletType.Bind:
                    if (_availableBindBullets.Count <= 0)
                    {
                        var insBullet = Instantiate(_bindBulletPrefab, _bulletParent, true);
                        insBullet.transform.position = pos;
                        insBullet.OnReturnToPool += ReturnBindPool;
                        insBullet.ChangeLookActive(true);
                        _usedBindBullets.Add(insBullet);
                        return;
                    }
                    var bindBullet = _availableBindBullets[0];
                    bindBullet.transform.position = pos;
                    _availableBindBullets.RemoveAt(0);
                    bindBullet.ChangeLookActive(true);
                    _usedBindBullets.Add(bindBullet);
                    break;
                case EnemyShooter.BulletType.Wall:
                    if (_availableWallBullets.Count <= 0)
                    {
                        var insBullet = Instantiate(_wallBulletPrefab, _bulletParent, true);
                        insBullet.transform.position = pos;
                        insBullet.OnReturnToPool += ReturnWallPool;
                        insBullet.ChangeLookActive(true);
                        _usedWallBullets.Add(insBullet);
                        return;
                    }
                    var wallBullet = _availableWallBullets[0];
                    wallBullet.transform.position = pos;
                    _availableWallBullets.RemoveAt(0);
                    wallBullet.ChangeLookActive(true);
                    _usedWallBullets.Add(wallBullet);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

        }

        private void ReturnBindPool(BindBullet bindBullet)
        {
            bindBullet.ChangeLookActive(false);
            _usedBindBullets.Remove(bindBullet);
            _availableBindBullets.Add(bindBullet);
        }
        private void ReturnWallPool(WallBullet wallBullet)
        {
            wallBullet.ChangeLookActive(false);
            _usedWallBullets.Remove(wallBullet);
            _availableWallBullets.Add(wallBullet);
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
            
            if (_usedWallBullets.Count != 0)
            {
                for (var i = _usedWallBullets.Count - 1; i >= 0; i--)
                {
                    var bullet = _usedWallBullets[i];
                    bullet.ChangeLookActive(false);
                    _usedBindBullets.RemoveAt(i);
                    _availableWallBullets.Add(bullet);
                }
            }
        }

    }
}
