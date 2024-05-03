using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEffecter : MonoBehaviour
{
    private Rigidbody _rigidBody;
    [SerializeField]
    private GameObject _effect;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // リジッドボディの速度を取得
        Vector3 velocity = _rigidBody.velocity;

        // 速度がゼロに近くないか確認
        if (velocity.sqrMagnitude > 0.01f)
        {
            // XY平面上での進行方向から角度を計算
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            // 目標のZ軸回転を作成
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            // 新しい回転にスムーズに補間
            _effect.transform.rotation = Quaternion.Lerp(_effect.transform.rotation, targetRotation, Time.deltaTime * 50f);
        }
    }
}
