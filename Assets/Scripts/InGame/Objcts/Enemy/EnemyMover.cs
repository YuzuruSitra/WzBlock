using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    private Vector3 _direction = Vector3.right;
    [Range(0, 100)]
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _leftObj;
    [SerializeField]
    private GameObject _rightObj;
    private MoveRangeCalculator _moveRangeCalculator;
    private float _leftMaxPos => _moveRangeCalculator.LeftMaxPos;
    private float _rightMaxPos  => _moveRangeCalculator.RightMaxPos;
    
    void Start()
    {
        _moveRangeCalculator = new MoveRangeCalculator(gameObject, _leftObj, _rightObj);
    }

    void Update()
    {
        // â¬ìÆàÊÇÃêßå¿
        Vector3 posX = transform.position;
        if (posX.x <= _leftMaxPos)
        {
            _direction = Vector3.right;
            posX.x = _leftMaxPos;
            transform.position = posX;
        }
        if (posX.x >= _rightMaxPos) 
        {
            _direction = Vector3.left;
            posX.x = _rightMaxPos;
            transform.position = posX;
        }
        transform.position += _direction * _speed * Time.deltaTime;
    }
}
