using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingDamageText : FloatingText {
    public Text rawDamageText;
    public Text armorText;
    public Text blockText;
    public Text finalDamageText;

    public void SetText(int rawDamage, int armor, int block, int finalDamage) {
        rawDamageText.text = rawDamage.ToString();
        armorText.text = armor.ToString();
        blockText.text = block.ToString();
        finalDamageText.text = "-" + finalDamage.ToString();
    }
}
