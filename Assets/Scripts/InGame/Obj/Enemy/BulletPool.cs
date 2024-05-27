using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField]
    private Transform _bulletParent;
    [SerializeField]
    private Bullet _bulletPrefab;
    private const int LAUNCH_POOL_COUNT = 10;
    private List<Bullet> _availableBullets = new List<Bullet>();
    private List<Bullet> _usedBullets = new List<Bullet>();

    private void Awake()
    {
        // 初期プールの作成
        for (int i = 0; i < LAUNCH_POOL_COUNT; i++)
        {
            Bullet bullet = Instantiate(_bulletPrefab);
            bullet.transform.SetParent(_bulletParent);
            bullet.OnReturnToPool += ReturnBlock;
            _availableBullets.Add(bullet);
        }
    }

    public Bullet GetBullet(Vector3 pos)
    {
        Bullet bullet;
        // プールから取得
        if (_availableBullets.Count <= 0)
        {
            Bullet insBullet = Instantiate(_bulletPrefab);
            insBullet.transform.position = pos;
            insBullet.transform.SetParent(_bulletParent);
            insBullet.OnReturnToPool += ReturnBlock;
            insBullet.ChangeLookActive(true);
            _usedBullets.Add(insBullet);
            return insBullet;
        }
        bullet = _availableBullets[0];
        bullet.transform.position = pos;
        _availableBullets.RemoveAt(0);
        bullet.ChangeLookActive(true);
        _usedBullets.Add(bullet);
        return bullet;
    }

    public void ReturnBlock(Bullet bullet)
    {
        bullet.ChangeLookActive(false);
        _usedBullets.Remove(bullet);
        _availableBullets.Add(bullet);
    }

    public void ReturnALLBlock()
    {
        if (_usedBullets.Count == 0) return;
        for (int i = _usedBullets.Count - 1; i >= 0; i--)
        {
            Bullet bullet = _usedBullets[i];
            bullet.ChangeLookActive(false);
            _usedBullets.RemoveAt(i);
            _availableBullets.Add(bullet);
        }
    }

}
