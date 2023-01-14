using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 attached to onClick animation of each character.
 It starts the audio when the animatio finish
*/
public class OnTapBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioSource audioSource = animator.GetComponent<AudioSource>();
        audioSource.enabled = true;
        audioSource.Play();
    }
}
