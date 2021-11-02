using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RogueKiller;

public sealed class ExecutionState : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController.Current.StopExecution();
    }
}
