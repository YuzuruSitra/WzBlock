using System.Collections.Generic;
using UnityEngine;

namespace InGame.Gimmick
{
    public class WallPool : MonoBehaviour
    {
        [SerializeField]
        private Transform _wallParent;
        [SerializeField]
        private GimmickWall _wallPrefab;
        private const int LaunchPoolCount = 5;
        private readonly List<GimmickWall> _availableWalls = new List<GimmickWall>();
        private readonly List<GimmickWall> _usedWalls = new List<GimmickWall>();

        private void Awake()
        {
            // 初期プールの作成
            for (var i = 0; i < LaunchPoolCount; i++)
            {
                var wall = Instantiate(_wallPrefab, _wallParent, true);
                wall.OnReturnToPool += ReturnBlock;
                _availableWalls.Add(wall);
            }
        }

        public void GetWall(Vector3 pos)
        {
            // プールから取得
            if (_availableWalls.Count <= 0)
            {
                var wall = Instantiate(_wallPrefab, _wallParent, true);
                wall.transform.position = pos;
                wall.OnReturnToPool += ReturnBlock;
                wall.ChangeLookActive(true);
                _usedWalls.Add(wall);
                return;
            }
            var bullet = _availableWalls[0];
            bullet.transform.position = pos;
            _availableWalls.RemoveAt(0);
            bullet.ChangeLookActive(true);
            _usedWalls.Add(bullet);
        }

        private void ReturnBlock(GimmickWall gimmickWall)
        {
            gimmickWall.ChangeLookActive(false);
            _usedWalls.Remove(gimmickWall);
            _availableWalls.Add(gimmickWall);
        }

        public void ReturnAllWall()
        {
            if (_usedWalls.Count == 0) return;
            for (var i = _usedWalls.Count - 1; i >= 0; i--)
            {
                var wall = _usedWalls[i];
                wall.ChangeLookActive(false);
                _usedWalls.RemoveAt(i);
                _availableWalls.Add(wall);
            }
        }
    }
}
