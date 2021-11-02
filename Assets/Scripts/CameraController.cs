using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueKiller
{
    public sealed class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _cam;
        public Camera Camera => _cam;
        private Transform _playerTransform;
        public Vector3 CursorLookDirection { get; private set; }

        public void Prepare(PlayerController pc)
        {
            _playerTransform = pc.transform;
        }

        private void LateUpdate()
        {
            transform.position = _playerTransform.transform.position;
            CursorLookDirection = (Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f)).normalized;
        }
    }
}
