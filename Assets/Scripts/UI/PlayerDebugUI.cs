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
        [SerializeField] private TextMeshProUGUI velocityText;     
        [SerializeField] private TextMeshProUGUI inputVectorText;    
        [SerializeField] private TextMeshProUGUI noiseText;      
        [SerializeField] private TextMeshProUGUI crouchText;      
        [SerializeField] private TextMeshProUGUI cameraPitchText;   

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

            if (stateText != null)
            {
                string stateName = PlayerStateManager.Instance != null 
                    ? PlayerStateManager.Instance.currentState.ToString() 
                    : "No StateManager";
                stateText.text = $"State: {stateName}";
            }

            if (groundedText != null)
            {
                groundedText.text = $"Grounded: {playerController.Grounded}";
                groundedText.color = playerController.Grounded ? Color.green : Color.red;
            }

            if (velocityText != null && _characterController != null)
            {
                Vector3 vel = _characterController.velocity;
                float speed = new Vector3(vel.x, 0f, vel.z).magnitude;
                velocityText.text = $"Vel: ({vel.x:F1}, {vel.y:F1}, {vel.z:F1}) | Speed: {speed:F2} m/s";
            }

            if (inputVectorText != null)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                inputVectorText.text = $"Input Raw: ({h:F1}, {v:F1})";
            }

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

            if (crouchText != null)
            {
                crouchText.text = $"Crouching: {playerController._isCrouching}";
            }

            if (cameraPitchText != null && playerController.CinemachineCameraTarget != null)
            {
                Vector3 rot = playerController.CinemachineCameraTarget.transform.localEulerAngles;
                cameraPitchText.text = $"Cam Rot: ({rot.x:F1}°, {rot.y:F1}°, {rot.z:F1}°)";
            }

            
        }
    }
}