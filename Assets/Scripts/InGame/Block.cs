using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private int _score;
    private int _blockID;
    public int BlockID => _blockID;
    private bool _isActive = false;
    public bool IsActive => _isActive;
    private ScoreHandler _scoreHandler;

    void Start()
    {
        _scoreHandler = ScoreHandler.Instance;
    }

    public void SetID(int newID)
    {
        _blockID = newID;
    }

    public void ChangeActive(bool newActive)
    {
        _isActive = newActive;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            _scoreHandler.AddScore(_score);
            gameObject.SetActive(false);
            ChangeActive(false);
        }
    }
}
