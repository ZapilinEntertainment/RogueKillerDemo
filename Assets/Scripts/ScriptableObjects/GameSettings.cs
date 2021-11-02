using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueKiller
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettingsObject", order = 1)]
    public sealed class GameSettings : ScriptableObject
    {
        [SerializeField] private float _playerSpeed = 10f, _enemyRespawnTimer = 5f, _executionDistance = 2f;

        public float PlayerSpeed => _playerSpeed;
        public float EnemyRespawnTimer => _enemyRespawnTimer;
        public float ExecutionDistance => _executionDistance;
    }
}
