using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using Unity.Cinemachine;
using UnityEngine.InputSystem.DualShock.LowLevel;
using NUnit.Framework;





#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class FirstPersonController : MonoBehaviour
    {

        [Header("Player")]
        public float MoveSpeed = 3.0f;
        public float SprintSpeed = 6.0f;
        public float RotationSpeed = 1.0f;
        public float SpeedChangeRate = 10.0f;


        [Space(10)]
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;

        [Space(10)]
        public float JumpTimeout = 0.3f;
        public float FallTimeout = 0.15f;


        [Header("Player Grounded")]
        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.1f;
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        public GameObject CinemachineCameraTarget;
        public float TopClamp = 90.0f;
        public float BottomClamp = -90.0f;

        [Header("Fall Effect (Rotation)")]
        [Tooltip("Absolute roll angle (Z axis) to rotate to during the fall, e.g. 70 = tipped hard to one side")]
        public float FallTargetRoll = 70f;
        public float FallTiltDuration = 0.4f;
        public float FallHoldDuration = 1.2f;
        public float FallRecoverDuration = 0.6f;

        [Header("Fall Shake")]
        public float ShakeAmount = 2f;
        public float ShakeSpeed = 25f;
        public float ShakeDurationAfterDrop = 0.4f;


        // [Range(0.1f, 1f)]
        public float CrouchCameraHeight = 0.8f;


        [Header("Crouch")]
        public float CrouchSpeed = 2f;
        private bool _forcedCrouch;
        [Tooltip("Multiplier for standing camera height when crouching (e.g., 0.6 = 60% of standing height)")]
        //[Range(0.1f, 1f)]
        public float CrouchCameraHeightMultiplier = 0.6f; 
        public float CrouchTransitionSpeed = 10f;
        public bool _isCrouching;
        public float _standingCameraHeight;
        public float _playerStandingHeight;
        public float _playerCrouchHeight;
        private Vector3 _standingCenterY;


        private bool _isFalling = false;
        private float _pitchBeforeFall;
        private Vector3 _cameraTargetOriginalLocalPos;


        [Header("Climbing")]
        [SerializeField] private bool IsClimbing = false;
        private float climbSpeed = 3f;

        [Header("Window Jump")]
        private float windowCheakdistance = 1.5f;
        public LayerMask windowLayer;
        public float jumpDuration = 1f;
        public float goinDownDuriation = 0.2f;
        private bool isWindowJumping = false;

        [Header("chair")]
        private RollingChair ridingChair;


        // cinemachine
        private float _cinemachineTargetPitch;


        [Header("Sound")]
        public int sprintNoiseRadius = 12;
        public int walkNoiseRadius = 4;
        public bool enableSoundVisualizer;


        // player
        private float _speed;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;


        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        [Tooltip("Absolute pitch angle to look at during the fall, e.g. -60 = looking mostly down")]
        public float FallTargetPitch = -60f;

        [Header("Fall FOV")]
        public Camera playerCamera;
        public float FallFOVPunch = 15f;
        public float FOVPunchDuration = 0.3f;

        private float _normalFOV;
        private PlayerStateManager playerManager;

        [Header("Stamina")]
        public float maxStamina = 10f;
        public float staminaDrainRate = 1.5f;
        public float staminaRegenRate = 0.5f;
        [SerializeField] private float currentStamina;
        private bool isExhausted;
        [SerializeField] private float staminaRegenDelay = 2f;
        private float staminaRegenTimer;


#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }

        public PlayerStateManager PlayerManager { get => playerManager; set => playerManager = value; }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();

            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            AddressablesAudioManager.Instance.PlayMusic("AmbientBackground");
            //  _cameraTargetOriginalLocalPos = CinemachineCameraTarget.transform.localPosition;
            _standingCameraHeight = CinemachineCameraTarget.transform.localPosition.y;
            _playerStandingHeight = _controller.height;
            _standingCenterY = _controller.center;
            if (_playerCrouchHeight <= 0f) _playerCrouchHeight = _playerStandingHeight * 0.5f;

            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#else
            Debug.LogError("Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            if (playerCamera != null)
            {
                _normalFOV = playerCamera.fieldOfView;
            }

            currentStamina = maxStamina;
        }



        private void Update()
        {

            if (_isFalling || isWindowJumping) return;
            if (IsStateRestricted()) return;

            if (IsClimbing)
            {
                Climb();
                return;
            }

            GroundedCheck();
            JumpAndGravity();
            Crouch();
            HandleStamina();
            Move();
            HandleNoiseEmission();

        }
        private void LateUpdate()
        {
            if (_isFalling || isWindowJumping ||
                IsStateRestricted())
                return;

            CameraRotation();
        }
        private bool IsStateRestricted()
        {
            if (PlayerStateManager.Instance == null) return false;
            var state = PlayerStateManager.Instance.currentState;
            return state == PlayerStateManager.PlayerState.Inspecting ||
             state == PlayerStateManager.PlayerState.Hiding;
        }




        private Vector3 GetGroundCheckPosition()
        {
            CharacterController controller = _controller != null ? _controller : GetComponent<CharacterController>();
            Vector3 worldCenter = transform.TransformPoint(controller.center);
            Vector3 FeetPoint = new Vector3(worldCenter.x, worldCenter.y - (controller.height / 2f), worldCenter.z);

            return new Vector3(FeetPoint.x, FeetPoint.y + GroundedOffset, FeetPoint.z);

        }

        private void GroundedCheck()
        {
            Grounded = Physics.CheckSphere(GetGroundCheckPosition(), GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
        }


        private void CameraRotation()
        {

            if (_input.look.sqrMagnitude >= _threshold)
            {
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
                _rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

                transform.Rotate(Vector3.up * _rotationVelocity);
            }
        }
        public void SetRidingChair(RollingChair chair)
        {
            ridingChair = chair;
        }

        public Vector2 GetMoveInput()
        {
            return _input.move;
        }

        private float targetSpeed;


        private void Move()
        {

            bool canSprint = currentStamina > 0 && !isExhausted;

            if (ridingChair != null)
                return;


            if (!_controller.enabled)
                return;


            if (_isCrouching)
            {
                targetSpeed = CrouchSpeed;
            }
            else if (_input.sprint && canSprint)
            {

                targetSpeed = SprintSpeed;
            }
            else
            {
                //targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
                targetSpeed = MoveSpeed;
            }



            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            if (_input.move != Vector2.zero)
            {
                inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
            }

            _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }


        private void HandleStamina()
        {
            bool isMoving = _input.move.sqrMagnitude > 00.01f;
            bool canSprint = _input.sprint && isMoving && !_isCrouching && !isExhausted;

            //Debug.Log($"Stamina: {currentStamina:F2} | Regen: {staminaRegenRate} | Exhausted: {isExhausted}");

            if (canSprint)
            {
                targetSpeed = SprintSpeed;
                currentStamina -= staminaDrainRate * Time.deltaTime;

                if (currentStamina <= 0)
                {
                    staminaRegenTimer = staminaRegenDelay;
                    isExhausted = true;
                    currentStamina = 0f;
                }

            }
            else
            {
                targetSpeed = MoveSpeed;
                currentStamina += staminaRegenRate * Time.deltaTime;
                if (staminaRegenTimer > 0f)
                {
                    staminaRegenTimer -= staminaRegenRate * Time.deltaTime;
                    Debug.Log(staminaRegenTimer);
                }

                if (currentStamina >= maxStamina)
                {
                    currentStamina = maxStamina;

                }

                if (isExhausted && currentStamina >= maxStamina * 25 / 100)
                {
                    isExhausted = false;
                }

            }

        }



        private void JumpAndGravity()
        {
            if (isWindowJumping || _isFalling)
                return;

            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    _input.jump = false;
                    _jumpTimeoutDelta = JumpTimeout;


                    if (CheckWindow())
                    {
                        Debug.Log(CheckWindow());
                        _input.jump = false;

                        StartCoroutine(WindowJumpRoutine());
                        return;
                    }

                    Debug.Log($"JUMP SOUND CALL - Frame: {Time.frameCount}");


                    AddressablesAudioManager.Instance.PlaySfx("JumpLanding");

                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                }

                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;

                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }

                _input.jump = false;
            }

            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }



        public void TriggerFall()
        {
            if (!_isFalling)
            {
                StartCoroutine(FallRoutine());
            }
        }

        private IEnumerator FallRoutine()
        {
            _isFalling = true;

            Quaternion startRotation = CinemachineCameraTarget.transform.localRotation;

            Quaternion fallRotation = Quaternion.Euler(
                -FallTargetRoll,
                _cinemachineTargetPitch,
                0f
            );

            float t = 0f;

            while (t < FallTiltDuration)
            {
                t += Time.deltaTime;

                CinemachineCameraTarget.transform.localRotation =
                    Quaternion.Slerp(startRotation, fallRotation, t / FallTiltDuration);

                yield return null;
            }

            CinemachineCameraTarget.transform.localRotation = fallRotation;

            transform.DOShakePosition(0.5f, new Vector3(0.5f, 0.3f, 0), 12, 90f, true).SetRelative(true);
            transform.DOShakeRotation(0.5f, new Vector3(0.5f, 0.3f, 0), 12, 90f, true).SetRelative(true);

            yield return new WaitForSeconds(FallHoldDuration);

            t = 0f;

            while (t < FallRecoverDuration)
            {
                t += Time.deltaTime;

                CinemachineCameraTarget.transform.localRotation =
                    Quaternion.Slerp(fallRotation, startRotation, t / FallRecoverDuration);

                yield return null;
            }

            CinemachineCameraTarget.transform.localRotation = startRotation;

            _verticalVelocity = -2f;
            _isFalling = false;
        }

        public void SetForcedCrouch(bool crouch)
        {
            _forcedCrouch = crouch;
        }

        private void Crouch()
        { Debug.Log($"Crouching: {_isCrouching} | Camera Y: {CinemachineCameraTarget.transform.localPosition.y} | Target Y: {_standingCameraHeight}");
            //_isCrouching = _input.crouch;
            _isCrouching = _input.crouch || _forcedCrouch;

            float targetControllerHeight = _isCrouching ? _playerCrouchHeight : _playerStandingHeight;

            float crouchDrop = (_playerStandingHeight - _playerCrouchHeight) * CrouchCameraHeightMultiplier;
            float targetCameraHeight = _isCrouching ? (_standingCameraHeight - crouchDrop) : _standingCameraHeight;

            float smoothFactor = 1f - Mathf.Exp(-CrouchTransitionSpeed * Time.deltaTime);

            _controller.height = Mathf.Lerp(_controller.height, targetControllerHeight, smoothFactor);

            Vector3 center = _controller.center;
            center.y = _standingCenterY.y - (_playerStandingHeight - _controller.height) / 2f;
            _controller.center = center;

            Vector3 cameraPos = CinemachineCameraTarget.transform.localPosition;
            cameraPos.y = Mathf.Lerp(cameraPos.y, targetCameraHeight, smoothFactor);
            CinemachineCameraTarget.transform.localPosition = cameraPos;
        }

        private void HandleNoiseEmission()
        {
            if (SoundManager.Instance == null || _isCrouching) return;

            bool isMoving = _input.move.sqrMagnitude > 0.001f;
            if (isMoving)
            {
                float noiseRadius = _input.sprint ? sprintNoiseRadius : walkNoiseRadius;
                SoundManager.Instance.MakeNoise(transform.position, noiseRadius, enableSoundVisualizer);

            }
        }

        private void Climb()
        {
            float input = _input.move.y;
            if (input < 0f && Grounded)
            {
                IsClimbing = false;
                _verticalVelocity = -2;
                return;
            }
            Vector3 climbDirection = Vector3.up * input * climbSpeed;
            _controller.Move(climbDirection * Time.deltaTime);
            _verticalVelocity = 0f;
        }



        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ladder"))
            {
                IsClimbing = true;
                _verticalVelocity = 0f;

            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ladder"))
            {
                IsClimbing = false;
                _verticalVelocity = -2f;
            }

        }


        private bool CheckWindow()
        {
            Vector3 origin = transform.position + Vector3.up * 1.0f;

            if (Physics.Raycast(origin,
                               transform.forward,
                               out RaycastHit hit,
                               windowCheakdistance,
                               windowLayer
                               ))
            {
                Window window = hit.collider.GetComponentInParent<Window>();



                if (window != null && window.isWindowBroken)
                {
                    return true;
                }
            }
            return false;
        }




        private IEnumerator WindowJumpRoutine()
        {
            AddressablesAudioManager.Instance.PlaySfx("WindowJumping");

            isWindowJumping = true;

            Vector3 start = transform.position;
            Vector3 end = start
                + transform.forward * 2.5f
                + Vector3.up * 0.5f;

            float duration = Mathf.Max(jumpDuration, 0.1f);
            float elapsed = 0f;

            _controller.enabled = false;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                float t = Mathf.Clamp01(elapsed / duration);

                // Smooth forward movement with an arc.
                float arc = Mathf.Sin(t * Mathf.PI) * 1f;

                transform.position = Vector3.Lerp(start, end, t)
                                     + Vector3.up * arc;

                yield return null;
            }

            transform.position = end + Vector3.up * 0.05f;

            _controller.enabled = true;
            _verticalVelocity = -2f;
            isWindowJumping = false;
        }



        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = Color.green;
            Color transparentRed = Color.red;

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;


            Gizmos.DrawSphere(GetGroundCheckPosition(), GroundedRadius);

        }



    }
}