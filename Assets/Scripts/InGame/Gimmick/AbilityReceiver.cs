using System;
using UnityEngine;

namespace InGame.Gimmick
{
    public class AbilityReceiver
    {
        private static AbilityReceiver _instance;
        public static AbilityReceiver Instance => _instance ??= new AbilityReceiver();
        public Condition CurrentCondition { get; private set; }

        public event Action<Condition> ConditionChanged;
        private const float CoolTime = 0.5f;
        private DateTime _lastConditionChange;
        public bool IsEnable => (DateTime.Now - _lastConditionChange).TotalSeconds >= CoolTime;
        private readonly GameStateHandler _gameStateHandler;
        
        private WallPool _wallPool;

        public enum Condition
        {
            Default,
            Stan
        }

        private AbilityReceiver() 
        {
            _gameStateHandler = GameStateHandler.Instance;
            _gameStateHandler.ChangeGameState += ChangeStateAbility;
            _wallPool = GameObject.FindWithTag("WallPool").GetComponent<WallPool>();
        }

        public void ChangeCondition(Condition newCondition)
        {
            if (CurrentCondition == newCondition) return;
            if (CurrentCondition != Condition.Default && newCondition != Condition.Default) return;
            if (!IsEnable) return;

            _lastConditionChange = DateTime.Now;
            CurrentCondition = newCondition;
            ConditionChanged?.Invoke(newCondition);
        }

        private void ChangeStateAbility(GameStateHandler.GameState newState)
        {
            if (newState != GameStateHandler.GameState.Launch) return;
            ConditionChanged?.Invoke(Condition.Default);
            _wallPool.ReturnAllWall();
        }
        
        public void GenerateWall(Vector3 pos)
        {
            _wallPool.GetWall(pos);
        }

    }
}
