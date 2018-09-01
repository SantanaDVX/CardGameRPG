using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextVictoryAnimation : MonoBehaviour {
    public Animator animator;

    void OnEnable() {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        float waitTime = clipInfo[0].clip.length;
        StartCoroutine(finishEncounter(waitTime));
        Destroy(gameObject, waitTime + 1.0f);
    }

    IEnumerator finishEncounter(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        AdventureController.Instance().finishEncounter();
    }
}
