using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueKiller
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform  _trackingArrow;
        [SerializeField] private GameObject _gun, _sword, _executionLabel;

        public static PlayerController Current { get; private set; }
        public bool ExecutionAvailable { get; private set; }

        private Transform _cameraTransform;
        private Vector3 _legsMoveVector;
        private CameraController _cameraController;
        private Enemy _trackingEnemy;
        private GameSettings _gameSettings;
        private ExecutionLabelUI _executionLabelUI;
        private bool _trackTarget = false, _drawMarker = false, _executionCinematicInProgress = false;
        private const float MAX_TORSO_ROTATION_ANGLE = 90f;
        public void Prepare(CameraController i_cc, GameSettings i_gs)
        {
            Current = this;
            _cameraController = i_cc;
            _gameSettings = i_gs;
            _cameraTransform = _cameraController.Camera.transform;
            _legsMoveVector = transform.forward;
            _trackingArrow.gameObject.SetActive(false);            
            _gun.SetActive(true);
            _sword.SetActive(true);
            //
            _executionLabelUI = _executionLabel.AddComponent<ExecutionLabelUI>();
            _executionLabel.SetActive(false);
        }
        private void Update()
        {
            if (!_executionCinematicInProgress)
            {
                var _moveVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
                if (_moveVector != Vector3.zero)
                {
                    _moveVector = Vector3.ProjectOnPlane(_cameraTransform.TransformDirection(_moveVector), transform.up).normalized;
                    transform.Translate(_moveVector * Time.deltaTime * _gameSettings.PlayerSpeed, Space.World);
                    _legsMoveVector = _moveVector;
                }

                //
                var cd = _cameraController.CursorLookDirection;
                if (cd != Vector3.zero)
                {
                    transform.forward = Vector3.ProjectOnPlane(_cameraTransform.TransformDirection(cd), transform.up);

                }
                //
                Vector3 legsVector = transform.InverseTransformDirection(_legsMoveVector);
                _animator.SetFloat("lookDir.x", legsVector.x);
                _animator.SetFloat("lookDir.y", legsVector.z);
                //

                ExecutionAvailable = false;
                if (_trackTarget)
                {
                    bool visible = _trackingEnemy.MainRenderer.isVisible;
                    if (visible == _drawMarker)
                    {
                        _drawMarker = !visible;
                        _trackingArrow.gameObject.SetActive(_drawMarker);
                        _executionLabel.SetActive(!_drawMarker);
                    }
                    Vector3 enemyPos = _trackingEnemy.transform.position,
                        playerPos = transform.position;
                    if (_drawMarker)
                    {
                        var d = (enemyPos - playerPos).normalized;
                        _trackingArrow.position = playerPos + d + Vector3.up * 0.02f;
                        _trackingArrow.rotation = Quaternion.LookRotation(transform.up, d);
                    }
                    else
                    {
                        ExecutionAvailable = Vector3.Distance(enemyPos, playerPos) <= _gameSettings.ExecutionDistance;
                        if (ExecutionAvailable && Input.GetKeyDown(KeyCode.Space))
                        {
                            Execution();
                        } 
                    }
                }
                //
            }
            else
            { // executing

            }
        }

        private void Execution()
        {
            if (!_executionCinematicInProgress)
            {
                _executionCinematicInProgress = true;
                _gun.SetActive(false);
                _sword.SetActive(true);
                _animator.Play("Execution");
            }
        }
        public void StopExecution()
        {
            if (_executionCinematicInProgress)
            {
                _executionCinematicInProgress = false;
                _gun.SetActive(true);
                _sword.SetActive(false);
            }
        }

        public void TrackTarget(Enemy e)
        {
            _trackingEnemy = e;
            _trackTarget = true;
            _drawMarker = false;
            _executionLabelUI.Prepare(this, _trackingEnemy.transform, _cameraController.Camera);
        }

    }
}
