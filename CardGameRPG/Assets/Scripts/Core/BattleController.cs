using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {

    public void goToCollection() {
        GameController.Instance().switchToCollectionView();
    }
}
