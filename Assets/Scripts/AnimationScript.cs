using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void RollDice()
    {
        animator.SetBool("isRolling", true);
    }
    public void StopRoll()
    {
        animator.SetBool("isRolling", false);
    }
}
