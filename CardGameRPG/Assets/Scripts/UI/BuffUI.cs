using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour {

    public Transform gridTransform;

    public GameObject buffIconPrefab;

    List<string> buffTooltips = new List<string>();

    public void updateBuffs(CombatCharacter character) {

        bool updateNeeded = false;
        if (buffTooltips.Count == character.buffs.Count) {
            for (int i = 0; i < character.buffs.Count; i++) {
                BaseBuff buff = character.buffs[i];
                if (!buff.tooltip.Equals(buffTooltips[i])) {
                    updateNeeded = true;
                }
            }
        } else {
            updateNeeded = true;
        }

        if (updateNeeded) {
            foreach (Transform child in gridTransform) {
                GameObject.Destroy(child.gameObject);
                buffTooltips.Clear();
            }

            foreach (BaseBuff buff in character.buffs) {
                buffTooltips.Add(buff.tooltip);
                GameObject go = Instantiate(buffIconPrefab, gridTransform);
                go.GetComponent<Image>().sprite = buff.buffIcon;
                go.GetComponent<Tooltip>().tooltipTitle = buff.buffName;
                go.GetComponent<Tooltip>().tooltipContent = buff.tooltip;
            }
        }
    }
}
