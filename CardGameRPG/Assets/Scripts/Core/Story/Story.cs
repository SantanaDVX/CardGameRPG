using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : LocationContentWithResolution {
    public string storyName;
    public Cutscene storyCutscene;

    public int focusGain;
    public int healthGain;
    public int goldGain;
    public List<GameObject> freeSkills;
    public List<BaseItem> itemsGained;
    public List<AdventureFlag> completionFlags;

    public override string getLocationName() {
        return storyName;
    }

    public override LocationType getLocationType() {
        return LocationType.Story;
    }

    public override int getFocusGained() {
        return focusGain;
    }

    public override int getHealthGained() {
        return healthGain;
    }

    public override int getGoldGained() {
        return goldGain;
    }

    public override List<GameObject> getCardsGained() {
        return freeSkills;
    }

    public override List<BaseItem> getItemsGained() {
        return itemsGained;
    }
}
