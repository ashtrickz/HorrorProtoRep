using Cinemachine;
using UnityEngine;

namespace Player
{
    public class CameraManager : MonoBehaviour
    {
        [Header("Camera Settings")] 
        [SerializeField]                private Vector2 sensitivity;
        [SerializeField, Range(0, 100)] private   float cameraYRange;

        [Header("References")]
        [SerializeField] private CinemachineVirtualCamera cmCamera;

        private CinemachinePOV _pov;

        public Vector2 Sensitivity => sensitivity;
        public float CameraYRange => cameraYRange;

        public CinemachineVirtualCamera CinemachineCam => cmCamera;

        private PlayerManager _player;
    
        public void Init(PlayerManager playerManager)
        {
            _player = playerManager;
        }

    }
}