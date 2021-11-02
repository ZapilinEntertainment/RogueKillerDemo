using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueKiller
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private float _gamezoneWidth = 50f, _gamezoneHeight = 50f;
        public static GameManager Current { get; private set; }
        public PlayerController PlayerController { get; private set; }
        public CameraController CameraController { get; private set; }
        public GameSettings GameSettings => _gameSettings;
        private Enemy _enemy;

        private void Awake()
        {
            Current = this;
            //
            PlayerController = FindObjectOfType<PlayerController>();
            if (PlayerController == null)
            {
                Debug.Log("error - no player controller found");
                return;
            }
            //
            CameraController = FindObjectOfType<CameraController>();
            if (CameraController == null)
            {
                Debug.Log("error - no camera controller found");
                return;
            }
            CameraController.Prepare(PlayerController);
            PlayerController.Prepare(CameraController, _gameSettings);
            //
            _enemy = FindObjectOfType<Enemy>();
            if (_enemy != null)
            {
                _enemy.Prepare(this);
                PlayerController.TrackTarget(_enemy);
            }
        }

        public Vector3 GetRandomPosition()
        {
            return new Vector3((Random.value - 0.5f) * _gamezoneWidth, 0f, (Random.value - 0.5f) * _gamezoneHeight);
        }

#if !UNITY_EDITOR
        private void OnGUI()
        {
            if (GUI.Button(new Rect(Screen.width * 0.9f, 0f, Screen.width * 0.1f, Screen.height * 0.05f), "Exit")) Application.Quit();
        }
#endif
    }
}
