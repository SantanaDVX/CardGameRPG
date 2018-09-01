using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventure : MonoBehaviour {
    public string adventureName;
    public List<Encounter> encounters;
    public Cutscene preAdventureCutscene;

    public int encounterIndex = -1;

    public int permenantPreAdventureFocusGain;
    public List<GameObject> freeSkills;

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
