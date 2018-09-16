using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryRandomizer : LocationContent {
    public string storyName;
    public List<Story> stories;
    public Story defaultStory;

    public override string getLocationName() {
        return storyName;
    }

    public override LocationType getLocationType() {
        return LocationType.Story;
    }

    public Story getStory() {
        Story story = defaultStory;

        List<Story> eligbleStories = new List<Story>();

        foreach (Story storyTmp in stories) {
            if (!PlayerStoryProgress.Instance().storiesCompleted.Contains(storyTmp.GetInstanceID())) {
                eligbleStories.Add(storyTmp);
            }
        }

        if (eligbleStories.Count > 0) {
            int randomIndex = Random.Range(0, eligbleStories.Count);
            story = eligbleStories[randomIndex];
            PlayerStoryProgress.Instance().storiesCompleted.Add(story.GetInstanceID());
        }

        return story;
    }

}
