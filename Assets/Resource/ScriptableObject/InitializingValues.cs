using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InitializingValues", menuName = "ScriptableObjects/CreateInitializingValues")]
public class InitializingValues : ScriptableObject
{
    [SerializeField]
    private string _playerName;
    public string PlayerName => _playerName;
    [SerializeField]
    private int _rank;
    public int Rank => _rank;
    [SerializeField]
    private int _exp;
    public int Exp => _exp;
    [SerializeField]
    private int _sensitivity;
    public int Sensitivity => _sensitivity;
    [SerializeField]
    private float _volume;
    public float Volume => _volume;
    [SerializeField]
    private int _maxScore;
    public int MaxScore => _maxScore;
    [SerializeField]
    private int _todayMaxScore;
    public int TodayMaxScore => _todayMaxScore;
}
