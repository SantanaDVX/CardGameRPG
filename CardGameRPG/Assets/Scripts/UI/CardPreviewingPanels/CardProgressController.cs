using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardProgressController : MonoBehaviour {

    public Transform cardProgressListTransform;
    public GameObject cardProgressListItemPrefab;
    public BaseCard cardProgressPreviewCard;
    public GameObject currentPreviewedCard;


    public Slider cardProgress;
    public Image progressMeterBackground;
    public Text progressMeterTitleText;
    public Text zeroPercText;
    public Text twentyfivePercText;
    public Text fiftyPercText;
    public Text seventyfivePercText;
    public Text hundredPercText;
    public Button rankUpButton;

    public Sprite untrainedBackground;
    public Sprite trainedBackground;
    public Sprite adeptBackground;
    public Sprite proficientBackground;
    public Sprite masteredBackground;
    public Sprite perfectedBackground;

    public Dictionary<string, GameObject> cardProgListItemMap;

    public void toggleCardProgressPanel() {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void loadCardProgression() {
        loadCardProgress();
        gameObject.SetActive(false);
    }

    public void loadCardProgress() {
        cardProgListItemMap = new Dictionary<string, GameObject>();
        foreach (Transform child in cardProgressListTransform) {
            GameObject.Destroy(child.gameObject);
        }
        
        currentPreviewedCard = PlayerCardProgress.Instance().cards.Values.First().card;
        previewCard(currentPreviewedCard);

        foreach (CardProgress cardProg in PlayerCardProgress.Instance().cards.Values) {
            addCardProg(cardProg, false);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(cardProgressListTransform as RectTransform);        
    }

    public void addCardProg(CardProgress cardProg, bool forceRefresh) {
        GameObject cardProgressListItemGO = Instantiate(cardProgressListItemPrefab, cardProgressListTransform);
        cardProgressListItemGO.GetComponent<CardProgressListItem>().setCard(cardProg);
        if (forceRefresh) {
            LayoutRebuilder.ForceRebuildLayoutImmediate(cardProgressListTransform as RectTransform);
        }
        cardProgListItemMap.Add(cardProg.cardName, cardProgressListItemGO);
        checkIfRankUpTextShouldShow(cardProg.cardName);
    }

    public void checkIfRankUpTextShouldShow(string cardName) {
        GameObject cardProgressListItemGO = cardProgListItemMap[cardName];
        CardProgress prog = PlayerCardProgress.Instance().cards[cardName];

        int maxExp = 0;
        switch (prog.progress) {
            case CardProgression.Unlearned: {
                    maxExp = PlayerCardProgress.Instance().untrainedExpCap;
                    break;
                }
            case CardProgression.Trained: {
                    maxExp = PlayerCardProgress.Instance().trainedExpCap;
                    break;
                }
            case CardProgression.Adept: {
                    maxExp = PlayerCardProgress.Instance().adpetExpCap;
                    break;
                }
            case CardProgression.Proficient: {
                    maxExp = PlayerCardProgress.Instance().proficientExpCap;
                    break;
                }
            case CardProgression.Mastered: {
                    maxExp = PlayerCardProgress.Instance().masteredExpCap;
                    break;
                }
            case CardProgression.Perfected: {
                    return;
                }
        }

        if (prog.expPoints >= maxExp) {
            cardProgressListItemGO.GetComponent<CardProgressListItem>().rankUpText.SetActive(true);
        }
    }

    public void rankUpPreviewCard() {
        CardProgress prog = PlayerCardProgress.Instance().cards[currentPreviewedCard.GetComponent<CardDetails>().cardName];
        switch (prog.progress) {
            case CardProgression.Unlearned: {
                    prog.progress = CardProgression.Trained;
                    PlayerCardCollection.Instance().cardCollection.Add(prog.card);
                    break;
                }
            case CardProgression.Trained: {
                    prog.progress = CardProgression.Adept;
                    PlayerCardCollection.Instance().cardCollection.Add(prog.card);
                    break;
                }
            case CardProgression.Adept: {
                    prog.progress = CardProgression.Proficient;
                    break;
                }
            case CardProgression.Proficient: {
                    prog.progress = CardProgression.Mastered;
                    PlayerCardCollection.Instance().cardCollection.Add(prog.card);
                    break;
                }
            case CardProgression.Mastered: {
                    prog.progress = CardProgression.Perfected;
                    break;
                }
        }
        prog.expPoints = 0;

        loadCardProgress();
        previewCard(prog.card);
    }

    public void previewCard(GameObject card) {
        currentPreviewedCard = card;
        cardProgressPreviewCard.loadCardDetails(currentPreviewedCard);
        CardProgress prog = PlayerCardProgress.Instance().cards[currentPreviewedCard.GetComponent<CardDetails>().cardName];
        int maxExp = 0;
        switch (prog.progress) {
            case CardProgression.Unlearned: {
                    progressMeterBackground.sprite = untrainedBackground;
                    maxExp = PlayerCardProgress.Instance().untrainedExpCap;
                    break;
                }
            case CardProgression.Trained: {
                    progressMeterBackground.sprite = trainedBackground;
                    maxExp = PlayerCardProgress.Instance().trainedExpCap;
                    break;
                }
            case CardProgression.Adept: {
                    progressMeterBackground.sprite = adeptBackground;
                    maxExp = PlayerCardProgress.Instance().adpetExpCap;
                    break;
                }
            case CardProgression.Proficient: {
                    progressMeterBackground.sprite = proficientBackground;
                    maxExp = PlayerCardProgress.Instance().proficientExpCap;
                    break;
                }
            case CardProgression.Mastered: {
                    progressMeterBackground.sprite = masteredBackground;
                    maxExp = PlayerCardProgress.Instance().masteredExpCap;
                    break;
                }
            case CardProgression.Perfected: {
                    progressMeterBackground.sprite = perfectedBackground;
                    cardProgress.value = 1.0f;
                    zeroPercText.gameObject.SetActive(false);
                    twentyfivePercText.gameObject.SetActive(false);
                    fiftyPercText.gameObject.SetActive(false);
                    seventyfivePercText.gameObject.SetActive(false);
                    hundredPercText.gameObject.SetActive(false);
                    break;
                }
        }

        progressMeterTitleText.text = prog.progress.ToString();
        if (prog.progress == CardProgression.Perfected) {
            cardProgress.value = 1;
        } else {
            cardProgress.value = ((float)prog.expPoints) / ((float)maxExp);
        }
        zeroPercText.text = "0";
        twentyfivePercText.text = (maxExp / 4).ToString();
        fiftyPercText.text = (maxExp / 2).ToString();
        seventyfivePercText.text = ((3 * maxExp) / 4).ToString();
        hundredPercText.text = (maxExp).ToString();

        if ((!SubSceneNode.getDict().ContainsKey("Battle") || !SubSceneNode.getDict()["Battle"].isActive())
         && prog.progress != CardProgression.Perfected
         && prog.expPoints >= maxExp) {
            rankUpButton.interactable = true;
        } else {
            rankUpButton.interactable = false;
        }
    }

    private static CardProgressController cardProgressController;
    public static CardProgressController Instance() {
        if (!cardProgressController) {
            cardProgressController = FindObjectOfType<CardProgressController>();
        }
        return cardProgressController;
    }
}
