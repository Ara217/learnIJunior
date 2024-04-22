using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using NaughtyAttributes;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayAnimatorOnDemand : MonoBehaviour
{


    public Animator animator;

    public string[] animations;

    public void PlayAnimation(string animation) {

        if (animations.Contains(animation) && AnimationExists(animation)) { 
            animator.Play(animation, 0, 0);
        }
    
    }

    private bool AnimationExists(string name)
    {
        if (animator != null)
        {
            AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;

            foreach (AnimationClip clip in animatorController.animationClips)
            {
                if (clip.name == name)
                {
                    return true;
                }
            }
            Debug.Log(animator.gameObject.name + " has no animation: " + name);
            return false;

        }
        else {
            Debug.LogError("No Animator" + animator.gameObject.name);
            return false;
        }

    }

    //[Button]
    public void GetAllAnimations() {


        AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;

        animations = new string[animatorController.animationClips.Length];
        for (int i = 0; i < animatorController.animationClips.Length; i++)
        {
            animations[i] = animatorController.animationClips[i].name;
        }

    }


}
