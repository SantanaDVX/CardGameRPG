using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XAbilityController : MonoBehaviour {

    public InputField xField;
    
    private BaseAbility lastAbility;

    private void Start() {
        xField = GetComponentInChildren<InputField>();
        Instance();
        gameObject.SetActive(false);
    }

    public void activateXField(BaseAbility referenceAbility) {
        xField.text = "";
        gameObject.SetActive(true);
        lastAbility = referenceAbility;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)
         && lastAbility != null) {
            int varX = Int32.Parse(xField.text);
            if (lastAbility.varXResource == ResourceType.Energy) {
                if (CombatCharacter.Player().energy >= varX) {
                    CombatCharacter.Player().energy -= varX;
                    lastAbility.setVarX(varX);
                    successfulInput(varX);
                }
            } else if (lastAbility.varXResource == ResourceType.Focus) {
                if (CombatCharacter.Player().focus >= varX) {
                    CombatCharacter.Player().focus -= varX;
                    successfulInput(varX);
                }
            }

            xField.text = "";
        }
    }

    private void successfulInput(int varX) {
        lastAbility.setVarX(varX);
        gameObject.SetActive(false);
        lastAbility = null;
    }

    private static XAbilityController xAbilityController;
    public static XAbilityController Instance() {
        if (!xAbilityController) {
            xAbilityController = FindObjectOfType<XAbilityController>();
        }
        return xAbilityController;
    }
}
