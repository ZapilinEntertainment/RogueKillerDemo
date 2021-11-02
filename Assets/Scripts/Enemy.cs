using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueKiller
{
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField] private Renderer _mainRenderer;
        [SerializeField] private GameObject _ragdollPref, _model;
        public Renderer MainRenderer => _mainRenderer;
        private GameObject _ragdoll;
        private GameManager _gameManager;
        private GameSettings _gameSettings;
        private float _timer;
        private bool _isKilled = false;

        public void Prepare( GameManager i_gm)
        {
            _gameManager = i_gm;
        }
        public void Kill(Vector3 killerPos)
        {
            if (!_isKilled)
            {
                _ragdoll = Instantiate(_ragdollPref, transform.position, transform.rotation);
                _ragdoll.GetComponentInChildren<Rigidbody>().AddForce((transform.position - killerPos).normalized * 800f, ForceMode.Acceleration);
                _timer = _gameManager.GameSettings.EnemyRespawnTime;
                _isKilled = true;
                _model.SetActive(false);
            }
        }

        private void Update()
        {
            if (_isKilled)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    Destroy(_ragdoll);
                    _ragdoll = null;
                    _isKilled = false;
                    Respawn();
                }
            }
        }

        public void Respawn()
        {
            transform.position = _gameManager.GetRandomPosition();
            _model.SetActive(true);
            _gameManager.PlayerController.TrackTarget(this);
        }
    }
}
