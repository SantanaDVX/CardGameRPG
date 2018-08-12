using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour {
    
    public Vector3 stackDisplacement;
    
    public Vector3 getNextStackPosition() {
        return transform.position + transform.TransformVector((stackDisplacement * StackController.Instance().theStack.Count));
    }

    private static PlayArea playArea;
    public static PlayArea Instance() {
        if (!playArea) {
            playArea = FindObjectOfType<PlayArea>();
        }
        return playArea;
    }
}
