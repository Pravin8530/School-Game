using UnityEngine;
using System.Collections;
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
        public float MoveSpeed = 4.0f;
        public float SprintSpeed = 6.0f;
        public float RotationSpeed = 1.0f;
        public float SpeedChangeRate = 10.0f;

        [Space(10)]
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;

        [Space(10)]
        public float JumpTimeout = 0.1f;
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.5f;
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

        private bool _isFalling = false;
        private float _pitchBeforeFall;
        private Vector3 _cameraTargetOriginalLocalPos;
        // ---------------------------------------

        // cinemachine
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        // private float _fallTimeoutDelta;

        [Tooltip("Absolute pitch angle to look at during the fall, e.g. -60 = looking mostly down")]
        public float FallTargetPitch = -60f;

        [Header("Fall FOV")]
        public Camera playerCamera; // drag your actual Camera component
        public float FallFOVPunch = 15f;
        public float FOVPunchDuration = 0.3f;

        private float _normalFOV;

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

        private void Awake()
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {


            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;


            _normalFOV = playerCamera.fieldOfView;
            // NEW: remember the camera target's resting position
            _cameraTargetOriginalLocalPos = CinemachineCameraTarget.transform.localPosition;

        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                TriggerFall();
            }

            // NEW: while falling, skip normal jump/gravity/move logic entirely
            if (_isFalling) return;

            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        private void LateUpdate()
        {
            // NEW: skip normal look-rotation while falling so the fall coroutine owns the camera
            if (_isFalling) return;

            CameraRotation();
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
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

        private void Move()
        {
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

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

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
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

        // ---------- NEW: Fall Effect ----------

        /// <summary>
        /// Call this to make the player "fall" — camera drops toward the ground,
        /// holds briefly, then rises back up and control resumes automatically.
        /// </summary>
        public void TriggerFall()
        {
            if (!_isFalling)
            {
                StartCoroutine(FallRoutine());
            }
        }

        private IEnumerator FOVPunch()
        {
            float t = 0f;
            while (t < FOVPunchDuration)
            {
                t += Time.deltaTime;
                float lerpT = Mathf.Clamp01(t / FOVPunchDuration);
                float curve = Mathf.Sin(lerpT * Mathf.PI); // punch out then back
                playerCamera.fieldOfView = _normalFOV + FallFOVPunch * curve;
                yield return null;
            }
            playerCamera.fieldOfView = _normalFOV;
        }

        private IEnumerator FallRoutine()
        {
            _isFalling = true;

            float rollSign = (Random.value > 0.5f) ? 1f : -1f;
            float targetRoll = FallTargetRoll * rollSign;

            // 1. Tip over into the fall
            float t = 0f;
            while (t < FallTiltDuration)
            {
                t += Time.deltaTime;
                float lerpT = Mathf.Clamp01(t / FallTiltDuration);
                float easedT = lerpT * lerpT * (3f - 2f * lerpT);

                float roll = Mathf.Lerp(0f, targetRoll, easedT);

                // pitch stays exactly as the player's current look, only roll changes
                //***************//
                // CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0f, roll);
                 CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(roll, 0f,_cinemachineTargetPitch);

                yield return null;
               
            }

            StartCoroutine(FOVPunch());
            // 2. Impact shake, fading out
            float shakeTimer = 0f;
            while (shakeTimer < ShakeDurationAfterDrop)
            {
                shakeTimer += Time.deltaTime;
                float fade = 1f - Mathf.Clamp01(shakeTimer / ShakeDurationAfterDrop);
                float shake = GetShakeOffset(ShakeAmount * fade);
               //*****//
              // CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0f, targetRoll + shake);
                 CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(targetRoll + shake, 0f, _cinemachineTargetPitch);
                yield return null;
            }
            //*****//
           // CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0f, targetRoll);

           CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(targetRoll, 0f, _cinemachineTargetPitch);
            // 3. Hold (settled, no shake)
            float holdRemaining = FallHoldDuration - ShakeDurationAfterDrop;
            if (holdRemaining > 0f)
            {
                yield return new WaitForSeconds(holdRemaining);
            }

            // 4. Recover back to upright
            t = 0f;
            float rollAtRecoverStart = targetRoll;
            while (t < FallRecoverDuration)
            {
                t += Time.deltaTime;
                float lerpT = Mathf.Clamp01(t / FallRecoverDuration);
                float easedT = lerpT * lerpT * (3f - 2f * lerpT);

                float roll = Mathf.Lerp(rollAtRecoverStart, 0f, easedT);
                //*****//
               // CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0f, roll);
                CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(roll, 0f, _cinemachineTargetPitch);
                yield return null;
            }
            //*****// 
           // CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0f, 0f);
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(0f, 0f, _cinemachineTargetPitch);


            _verticalVelocity = -2f;
            _isFalling = false;
        }

        private float GetShakeOffset(float amount)
        {
            return (Mathf.PerlinNoise(Time.time * ShakeSpeed, 0f) - 0.5f) * 2f * amount;
        }
        // ---------------------------------------

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
        }
    }
}