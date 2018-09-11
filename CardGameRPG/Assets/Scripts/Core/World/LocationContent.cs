using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LocationContent : MonoBehaviour {
    public abstract string getLocationName();
    public abstract LocationType getLocationType();
}
