using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueKiller
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _trackingArrow;
        [SerializeField] private GameObject _gun, _sword, _executionLabel;

        public static PlayerController Current { get; private set; }
        public bool ExecutionAvailable { get; private set; }

        private Transform _cameraTransform;
        private Vector3 _legsMoveVector, _legsVectorCurrent, _legsVectorTarget;
        private CameraController _cameraController;
        private Enemy _trackingEnemy;
        private GameSettings _gameSettings;
        private ExecutionLabelUI _executionLabelUI;
        private CinematicPrepareData _cinematicData;
        private bool _trackTarget = false, _drawMarker = false, _executionCinematicInProgress = false;
        private float _cinematicProgress = 0f;
        private const float MAX_TORSO_ROTATION_ANGLE = 90f, _cinematicTargetingTime = 0.5f, _animationChangeSpeed = 10f;
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
            _legsVectorCurrent = Vector3.zero;
            _legsVectorTarget = _legsVectorCurrent;
            ChangeLegsAnimation(_legsVectorCurrent);
            //
            _executionLabelUI = _executionLabel.AddComponent<ExecutionLabelUI>();
            _executionLabel.SetActive(false);
        }
        private void Update()
        {
            float t = Time.deltaTime;
            if (!_executionCinematicInProgress)
            {
                var _moveVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
                if (_moveVector != Vector3.zero)
                {
                    _moveVector = Vector3.ProjectOnPlane(_cameraTransform.TransformDirection(_moveVector), transform.up).normalized;
                    transform.Translate(_moveVector * t * _gameSettings.PlayerSpeed, Space.World);
                    _legsMoveVector = _moveVector;
                }

                //
                var cd = _cameraController.CursorLookDirection;
                if (cd != Vector3.zero)
                {
                    transform.forward = Vector3.ProjectOnPlane(_cameraTransform.TransformDirection(cd), transform.up);

                }
                //
                _legsVectorTarget = transform.InverseTransformDirection(_legsMoveVector) * _moveVector.magnitude;
                if (_legsVectorCurrent != _legsVectorTarget)
                {
                    _legsVectorCurrent = Vector3.MoveTowards(_legsVectorCurrent, _legsVectorTarget, _animationChangeSpeed * t);
                    ChangeLegsAnimation(_legsVectorCurrent);
                }
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
                        ExecutionAvailable = Vector3.Distance(enemyPos, playerPos) <= _gameSettings.ExecutionAvailableDistance;
                        if (ExecutionAvailable && Input.GetKeyDown(KeyCode.Space))
                        {
                            Execution();
                        }
                    }
                }
                //
            }
            else
            {
                _cinematicProgress = Mathf.MoveTowards(_cinematicProgress, 1f, t / _cinematicTargetingTime);
                _cinematicData.SetPosition(transform, _cinematicProgress);
            }
        }

        private void Execution()
        {
            if (!_executionCinematicInProgress)
            {
                _executionCinematicInProgress = true;
                ExecutionAvailable = false;
                _gun.SetActive(false);
                _sword.SetActive(true);
                _animator.Play("Execution");

                _cinematicData = new CinematicPrepareData(transform, _trackingEnemy.transform, _gameSettings.ExecutionCinematicDistance);
                _cinematicProgress = 0f;
            }
        }
        public void StopExecution()
        {
            if (_executionCinematicInProgress)
            {
                _trackingEnemy.Kill(transform.position);
                _executionCinematicInProgress = false;
                _legsVectorCurrent = _legsVectorTarget = Vector3.zero;
                ChangeLegsAnimation(_legsVectorCurrent);
                _gun.SetActive(true);
                _sword.SetActive(false);
                _trackTarget = false;
                _trackingEnemy = null;
            }
        }
        private void ChangeLegsAnimation (Vector3 v)
        {
            _animator.SetFloat("lookDir.x", v.x);
            _animator.SetFloat("lookDir.y", v.z);
        }

        public void TrackTarget(Enemy e)
        {
            _trackingEnemy = e;
            _trackTarget = true;
            _drawMarker = false;
            _executionLabelUI.Prepare(this, _trackingEnemy.transform, _cameraController.Camera);
        }

        private struct CinematicPrepareData {
            private Vector3 _startPos, _endPos;
            private Quaternion _startRot, _endRot;
            public CinematicPrepareData(Transform player, Transform enemy, float x)
            {
                _startPos = player.position;
                _startRot = player.rotation;
                Vector3 enemypos = enemy.position,
                 d = (enemypos - _startPos).normalized;
                _endPos = enemypos - d * x;
                _endRot = Quaternion.LookRotation(d, player.up);
            }

            public void SetPosition(Transform t, float lerpval)
            {
                float x = Mathf.SmoothStep(0f, 1f, lerpval);
                t.position = Vector3.Lerp(_startPos, _endPos, x);
                t.rotation = Quaternion.Lerp(_startRot, _endRot, x);
            }
        }

    }
}
