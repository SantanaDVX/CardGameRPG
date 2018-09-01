using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardProgressButtonListener : MonoBehaviour {

    public void toggleCardProgressPanel() {
        CardProgressController.Instance().toggleCardProgressPanel();
    }
}
