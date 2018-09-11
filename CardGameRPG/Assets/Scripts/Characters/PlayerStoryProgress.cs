using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStoryProgress : MonoBehaviour {

    public AdventureFlag debugFlagGain;
    public bool gainDebug;

    public Dictionary<AdventureFlag, int> storyFlags = new Dictionary<AdventureFlag, int>();

    private void Update() {
        if (gainDebug) {
            gainDebug = false;
            incrementStoryFlag(debugFlagGain);
        }
    }

    public void incrementStoryFlag(AdventureFlag flag) {
        if (storyFlags.ContainsKey(flag)) {
            storyFlags[flag]++;
        } else {
            storyFlags.Add(flag, 1);
        }
    }

    public bool flagMet(AdventureFlag flag, int requirement, Comparison comp) {
        int val = (storyFlags.ContainsKey(flag) ? storyFlags[flag] : 0);
        if (comp == Comparison.GT) {
            return val > requirement;
        } else if (comp == Comparison.GTE) {
            return val >= requirement;
        } else if (comp == Comparison.EQ) {
            return val == requirement;
        } else if (comp == Comparison.NEQ) {
            return val != requirement;
        } else if (comp == Comparison.LTE) {
            return val <= requirement;
        } else if (comp == Comparison.LT) {
            return val < requirement;
        }
        return false;
    }


    private static PlayerStoryProgress playerStoryProgress;
    public static PlayerStoryProgress Instance() {
        if (!playerStoryProgress) {
            playerStoryProgress = FindObjectOfType<PlayerStoryProgress>();
        }
        return playerStoryProgress;
    }
}
