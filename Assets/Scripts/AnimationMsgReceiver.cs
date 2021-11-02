using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueKiller
{
    public sealed class AnimationMsgReceiver : MonoBehaviour
    {
        public void KillMoment()
        {
            PlayerController.Current.StopExecution();
        }
    }
}
