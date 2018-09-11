using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryRandomizer : LocationContent {
    public string storyName;
    public List<Story> stories;

    public override string getLocationName() {
        return storyName;
    }

    public override LocationType getLocationType() {
        return LocationType.Story;
    }

    public Story getStory() {
        return stories[Random.Range(0, stories.Count)];
    }

}
