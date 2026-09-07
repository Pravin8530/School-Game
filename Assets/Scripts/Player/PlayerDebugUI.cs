using UnityEngine;
using TMPro;

namespace StarterAssets
{
    public class PlayerDebugUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FirstPersonController playerController;

        [Header("TMP Debug Elements")]
        [SerializeField] private TextMeshProUGUI stateText;
        [SerializeField] private TextMeshProUGUI groundedText;
        [SerializeField] private TextMeshProUGUI velocityText;      // Vector3 speed
        [SerializeField] private TextMeshProUGUI inputVectorText;    // Vector2 move input
        [SerializeField] private TextMeshProUGUI noiseText;          // Active noise radius
        [SerializeField] private TextMeshProUGUI crouchText;         // Crouch state & height
        [SerializeField] private TextMeshProUGUI cameraPitchText;    // Cinemachine target pitch
    //    [SerializeField] private TextMeshProUGUI fallEffectText;     // Is fall effect active

        private CharacterController _characterController;

        private void Start()
        {
            if (playerController != null)
            {
                _characterController = playerController.GetComponent<CharacterController>();
            }
        }

        private void Update()
        {
            if (playerController == null) return;

            // 1. Player State Manager (Inspecting, Hiding, BookInteract, etc.)
            if (stateText != null)
            {
                string stateName = PlayerStateManager.Instance != null 
                    ? PlayerStateManager.Instance.currentState.ToString() 
                    : "No StateManager";
                stateText.text = $"State: {stateName}";
            }

            // 2. Grounded Check
            if (groundedText != null)
            {
                groundedText.text = $"Grounded: {playerController.Grounded}";
                groundedText.color = playerController.Grounded ? Color.green : Color.red;
            }

            // 3. Raw Vector3 Velocity & Speed
            if (velocityText != null && _characterController != null)
            {
                Vector3 vel = _characterController.velocity;
                float speed = new Vector3(vel.x, 0f, vel.z).magnitude;
                velocityText.text = $"Vel: ({vel.x:F1}, {vel.y:F1}, {vel.z:F1}) | Speed: {speed:F2} m/s";
            }

            // 4. Input Direction Vector
            if (inputVectorText != null)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                inputVectorText.text = $"Input Raw: ({h:F1}, {v:F1})";
            }

            // 5. Noise Radius (Walking vs Running vs Idle)
            if (noiseText != null)
            {
                bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
                bool isRunning = Input.GetKey(KeyCode.LeftShift);

                float currentNoise = 0f;
                if (isMoving)
                {
                    currentNoise = isRunning ? playerController.maxNoiseRadius : playerController.minNoiseRadius;
                }

                noiseText.text = $"Noise Radius: {currentNoise}m";
            }

            // 6. Crouch State
            if (crouchText != null)
            {
                crouchText.text = $"Crouching: {playerController._isCrouching}";
            }

            // 7. Camera Transform Angle (Pitch / Camera Target Local Rotation)
            if (cameraPitchText != null && playerController.CinemachineCameraTarget != null)
            {
                Vector3 rot = playerController.CinemachineCameraTarget.transform.localEulerAngles;
                cameraPitchText.text = $"Cam Rot: ({rot.x:F1}°, {rot.y:F1}°, {rot.z:F1}°)";
            }

            
        }
    }
}