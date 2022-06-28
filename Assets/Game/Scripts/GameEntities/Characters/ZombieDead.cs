using UnityEngine;

public class ZombieDead : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Destroy object after animation completes
        Destroy(animator.gameObject,stateInfo.length);
    }

}
