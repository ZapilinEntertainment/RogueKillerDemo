using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RogueKiller
{
    public sealed class ExecutionLabelUI : MonoBehaviour
    {
        private PlayerController _playerController;
        private Transform _enemyTransform;
        private Camera _cam;
        private Text _text;
        private bool _prepared = false, _shown = true;
        private float sz;
        public void Prepare(PlayerController i_pc,Transform i_enemyTransform, Camera i_cam)
        {
            _playerController =  i_pc;
            _enemyTransform = i_enemyTransform;
            _cam = i_cam;
            sz = Screen.height / 15f;
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sz);
            
            _shown = _playerController.ExecutionAvailable;
            if (_text == null) _text = GetComponentInChildren<Text>();
            _text.enabled = _shown;
            _prepared = true;
        }

        private void Update()
        {
            if (_prepared)
            {
                if (_shown != _playerController.ExecutionAvailable)
                {
                    _shown = _playerController.ExecutionAvailable;
                    _text.enabled = _shown;
                }
                if (_shown)
                {
                    transform.position = _cam.WorldToScreenPoint(_enemyTransform.position) + Vector3.left * sz * 4f;
                }
            }
        }
    }
}
