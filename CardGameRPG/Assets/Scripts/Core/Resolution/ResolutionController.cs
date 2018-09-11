using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionController : MonoBehaviour {

    public Text locationNameText;
    public Text focusGainedText;
    public Text healthGainedText;
    public Text goldGainedText;
    public Transform itemHolder;
    public List<GameObject> previewCards;


    public void exitResolutionPage() {
        GameController.Instance().finishResolution();
    }


    public void setResolutionPage(LocationContentWithResolution content) {
        locationNameText.text = content.getLocationName();
        focusGainedText.text = content.getFocusGained().ToString();
        healthGainedText.text = content.getHealthGained().ToString();
        goldGainedText.text = content.getGoldGained().ToString();

        foreach (Transform child in itemHolder) {
            GameObject.Destroy(child.gameObject);
        }

        foreach (BaseItem item in content.getItemsGained()) {
            // DO SOMETHING?!??!
        }

        foreach (GameObject card in previewCards) {
            card.SetActive(false);
        }

        for (int i = 0; i < content.getCardsGained().Count; i++) {
            previewCards[i].SetActive(true);
            previewCards[i].GetComponent<BaseCard>().loadCardDetails(content.getCardsGained()[i]);
        }

        PlayerInfo player = PlayerInfo.Instance();
        player.preadventureFocus += content.getFocusGained();
        player.startingHealth += content.getHealthGained();
        player.gold += content.getGoldGained();
        // DO SOMETHING WITH ITEMS
        foreach (GameObject card in content.getCardsGained()) {
            PlayerCardProgress.Instance().createNewCardAtTrained(card);
        }
    }



    private static ResolutionController resolutionController;
    public static ResolutionController Instance() {
        if (!resolutionController) {
            resolutionController = FindObjectOfType<ResolutionController>();
        }
        return resolutionController;
    }
}
