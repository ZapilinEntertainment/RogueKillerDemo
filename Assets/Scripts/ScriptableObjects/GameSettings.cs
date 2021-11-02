using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueKiller
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettingsObject", order = 1)]
    public sealed class GameSettings : ScriptableObject
    {
        [SerializeField]
        private float _playerSpeed = 10f, _enemyRespawnTimer = 5f, _executionAvailableDistance = 2f,
            _executionCinematicDistance = 1f;

        public float PlayerSpeed => _playerSpeed;
        public float EnemyRespawnTime => _enemyRespawnTimer;
        public float ExecutionAvailableDistance => _executionAvailableDistance;
        public float ExecutionCinematicDistance => _executionCinematicDistance;
    }
}
