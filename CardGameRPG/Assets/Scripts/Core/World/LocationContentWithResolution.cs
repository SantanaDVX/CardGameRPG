using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LocationContentWithResolution : LocationContent {
    public abstract int getFocusGained();
    public abstract int getHealthGained();
    public abstract int getGoldGained();
    public abstract List<BaseItem> getItemsGained();
    public abstract List<GameObject> getCardsGained();

}
