using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceAnimgusTrack : BaseAbility {

    public AnimagusTrack trackToMove;
    public int movementAmount;

    public override string getTextBoxText() {
        return textBoxText.Replace("(trackToMove?)", trackToMove.ToString()).Replace("(movementAmount?)", movementAmount.ToString());
    }
    public override void activateAbility(CardDetails details) {
        if (details.character is AnimagusCharacter) {
            AnimagusCharacter character = details.character as AnimagusCharacter;
            if (trackToMove == AnimagusTrack.Agility) {
                character.actionEnergySlot = Mathf.Clamp(character.actionEnergySlot + movementAmount, 0, AnimagusCharacter.actionAlignmentValues.Length - 1);
            } else if (trackToMove == AnimagusTrack.Might) {
                character.actionEnergySlot = Mathf.Clamp(character.actionEnergySlot - movementAmount, 0, AnimagusCharacter.energyAlignmentValues.Length - 1);
            } else if (trackToMove == AnimagusTrack.Reflex) {
                character.cardDrawFocusSlot = Mathf.Clamp(character.cardDrawFocusSlot - movementAmount, 0, AnimagusCharacter.cardDrawAlignmentValues.Length - 1);
            } else if (trackToMove == AnimagusTrack.Concentration) {
                character.cardDrawFocusSlot = Mathf.Clamp(character.cardDrawFocusSlot + movementAmount, 0, AnimagusCharacter.focusAlignmentValues.Length - 1);
            }
            character.refreshUI();
        } else {
            Debug.Log("Not aNIMAGUs");
        }
    }
}