using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventure : LocationContentWithResolution {
    public string adventureName;
    public List<Encounter> encounters;
    public Cutscene preAdventureCutscene;

    public int encounterIndex = -1;

    public int permenantPreAdventureFocusGain;
    public int permenantPreAdventureHealthGain;
    public int goldGain;
    public List<GameObject> freeSkills;
    public List<BaseItem> itemsGained;
    public List<AdventureFlag> completionFlags;

    public override string getLocationName() {
        return adventureName;
    }

    public override LocationType getLocationType() {
        return LocationType.Adventure;
    }

    public override int getFocusGained() {
        return permenantPreAdventureFocusGain;
    }

    public override int getHealthGained() {
        return permenantPreAdventureHealthGain;
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


    public Encounter getNextEncounter() {
        encounterIndex++;
        if (encounterIndex < encounters.Count) {
            GameObject encGO = Instantiate(encounters[encounterIndex].gameObject, transform);
            return encGO.GetComponent<Encounter>();
        } else {
            return null;
        }
    }
}


