using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStoryProgress : MonoBehaviour {

    public AdventureFlag debugFlagGain;
    public bool gainDebug;

    public Dictionary<AdventureFlag, int> storyFlags = new Dictionary<AdventureFlag, int>();
    public HashSet<int> storiesCompleted = new HashSet<int>();

    private void Update() {
        if (gainDebug) {
            gainDebug = false;
            incrementStoryFlag(debugFlagGain);
        }
    }

    public void incrementStoryFlag(AdventureFlag flag) {
        changeStoryFlag(flag, 1);
    }

    public void decrementStoryFlag(AdventureFlag flag) {
        changeStoryFlag(flag, -1);
    }

    public void changeStoryFlag(AdventureFlag flag, int amt) {
        if (storyFlags.ContainsKey(flag)) {
            storyFlags[flag] += amt;
        } else {
            storyFlags.Add(flag, amt);
        }
        storyFlags[flag] = Mathf.Max(0, storyFlags[flag]);
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
