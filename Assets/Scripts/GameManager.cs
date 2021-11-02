using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueKiller
{
    public sealed class GameManager : MonoBehaviour
    {
        public static GameManager Current { get; private set; }
        public PlayerController PlayerController { get; private set; }
        public CameraController CameraController { get; private set; }

        private void Awake()
        {
            Current = this;
            //
            PlayerController = FindObjectOfType<PlayerController>();
            if (PlayerController == null)
            {
                Debug.Log("error - no player controller found");
                return;
            }
            //
            CameraController = FindObjectOfType<CameraController>();
            if (CameraController == null)
            {
                Debug.Log("error - no camera controller found");
                return;
            }
            CameraController.Prepare(PlayerController);
            PlayerController.Prepare(CameraController);            
            //

        }
    }
}
