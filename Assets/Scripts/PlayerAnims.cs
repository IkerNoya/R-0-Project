using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnims : MonoBehaviour {
    [SerializeField] Animator animator;
    public enum Parameters {
        MoveSide,
        Idle,
        Dash,
        Dead
    }
    Parameters p;
    public void StartAnimMoveSide() {
        animator.SetBool("MoveSide", true);
        animator.SetBool("Idle", false);
        animator.SetBool("Dead", false);
    }
    public void StartIdleAnim() {
        animator.SetBool("MoveSide", false);
        animator.SetBool("Idle", true);
        animator.SetBool("Dead", false);
    }
    public void StartDeadAnim() {
        animator.SetBool("MoveSide", false);
        animator.SetBool("Idle", false);
        animator.SetBool("Dead", true);
    }
    public void StartDashAnim()
    {
        animator.SetBool("MoveSide", false);
        animator.SetBool("Idle", false);
        animator.SetTrigger("Dash");
    }
}
