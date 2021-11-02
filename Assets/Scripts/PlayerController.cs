using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueKiller
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform  _modelTransform;
        [SerializeField] private float _speed = 10f;
        private Transform _cameraTransform;
        private Vector3 _targetLookDirection, _torsoLookDirection, _legsMoveVector;
        private CameraController _cameraController;
        private const float MAX_TORSO_ROTATION_ANGLE = 90f;
        public void Prepare(CameraController i_cc)
        {            
            _cameraController = i_cc;
            _cameraTransform = _cameraController.Camera.transform;
            _torsoLookDirection = transform.forward;
            _legsMoveVector = _torsoLookDirection;
        }
        private void Update()
        {
            var _moveVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            if (_moveVector != Vector3.zero)
            {
                _moveVector = Vector3.ProjectOnPlane(_cameraTransform.TransformDirection(_moveVector), transform.up).normalized;
                transform.Translate(_moveVector * Time.deltaTime* _speed, Space.World);
                _legsMoveVector = _moveVector;
            }
           
            //
            var cd = _cameraController.CursorLookDirection;
            if (cd != Vector3.zero)
            {
                _targetLookDirection = Vector3.ProjectOnPlane( _cameraTransform.TransformDirection(cd), transform.up);                
                
                /*
                cd = transform.InverseTransformDirection(cd);
                float x = Vector3.Angle(Vector3.forward, cd);
                if (x > MAX_TORSO_ROTATION_ANGLE) x = MAX_TORSO_ROTATION_ANGLE;
                if (cd.x > 0f) x *= -1f;
                _animator.SetFloat("lookDir.x", _lookDirection.x);
                _animator.SetFloat("lookDir.y", _lookDirection.y);
                */                
            }
            if (_targetLookDirection != Vector3.zero)
            {
                _torsoLookDirection = _targetLookDirection;
            }
            transform.forward = _torsoLookDirection;
            //
            Vector3 legsVector = transform.InverseTransformDirection(_legsMoveVector);
            _animator.SetFloat("lookDir.x", legsVector.x);
            _animator.SetFloat("lookDir.y", legsVector.z);
            //
        }


    }
}
