using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Location : MonoBehaviour {
    public GameObject locationNode;
    public LocationContent content;
    public RectTransform ribbon;
    public Text locationNameText;
    public Button locationButton;

    public List<AdventureFlagChart> appearRequirements;
    public Dictionary<AdventureFlag, int> appearMap;
    public List<AdventureFlagChart> unlockableRequirements;
    public Dictionary<AdventureFlag, int> unlockableMap;
    public string preUnlockText;
    
    
    void Update () {
        if (content == null) {
            locationNameText.text = "???";
        } else {
            string text = content.getLocationName();
            if (text != locationNameText.text) {
                locationNameText.text = text;
            }
            Image bob = locationButton.GetComponent<Image>();
            MaterialDictionary dict = MaterialDictionary.Instance();
            bob.sprite = dict.locationButtonsDictionary[content.getLocationType()];
            if (!Application.isEditor || Application.isPlaying) {
                checkIfShouldAppearInWorld();
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(ribbon);
    }

    public void checkIfShouldAppearInWorld() {
        bool shouldAppear = true;
        foreach (AdventureFlagChart afc in appearRequirements) {
            if (!PlayerStoryProgress.Instance().flagMet(afc.flag, afc.cnt, afc.comp)) {
                shouldAppear = false;
                break;
            }
        }

        if (shouldAppear) {
            locationNode.SetActive(true);
            bool locationAvailable = true;
            foreach (AdventureFlagChart afc in unlockableRequirements) {
                if (!PlayerStoryProgress.Instance().flagMet(afc.flag, afc.cnt, afc.comp)) {
                    locationAvailable = false;
                    break;
                }
            }

            locationButton.interactable = locationAvailable;

        } else {
            locationNode.SetActive(false);
        }
    }

    public void locationActivate() {
        if (content is Adventure) {
            GameController.Instance().startPreadventure(content as Adventure);
        } else if (content is StoryRandomizer) {
            GameController.Instance().startStory((content as StoryRandomizer).getStory());
        } else if (content is Story) {
            GameController.Instance().startStory(content as Story);
        }
    }

    [Serializable]
    public struct AdventureFlagChart {
        public AdventureFlag flag;
        public Comparison comp;
        public int cnt;
    }
}
