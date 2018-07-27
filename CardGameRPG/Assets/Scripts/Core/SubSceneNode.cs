using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSceneNode : MonoBehaviour {

    public string nodeName;
    public bool hideOnAwake;

    private void Awake() {
        subSceneNodes.Add(nodeName, this);
        if (hideOnAwake) {
            setActive(false);
        }
    }

    public void setActive(bool active) {
        gameObject.SetActive(active);
    }

    public bool isActive() {
        return gameObject.activeSelf;
    }


    protected static Dictionary<string, SubSceneNode> subSceneNodes = new Dictionary<string, SubSceneNode>();

    public static SubSceneNode getNode(string key) {
        //Debug.Log("Key: " + key);
        return subSceneNodes[key];
    }

    public static Dictionary<string, SubSceneNode> getDict() {
        return subSceneNodes;
    }

}
