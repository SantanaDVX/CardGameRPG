using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCharacter : Character {
    public bool playerCharacter;
    public string characterName;
    public Transform arrowPoint;
    public Hand hand;
    public Deck deck;
    public Discard discard;
    public CharacterUI ui;
    public TabPanelController tabPanel;
    public List<BaseBuff> buffs;
    
    public virtual int actionTurnAmount { get; protected set; }
    public virtual int cardDrawTurnAmount { get; protected set; }
    public virtual int energyTurnAmount { get; protected set; }
    public virtual int focusTurnAmount { get; protected set; }

    public int otherSourcesActionTurnAmountValue;
    public int otherSourcesEnergyTurnAmountValue;
    public int otherSourcesCardDrawTurnAmountValue;
    public int otherSourcesFocusTurnAmountValue;

    public int maxHealth;

    public int actions;
    public int energy;
    public int focus;
    public int health;

    public float avoidance;
    public float accuracy;
    public int armor;
    public int weaponDamageMod;

    public Block activeBlock;
    
    public Weapon equippedWeapon;
    public Armor equippedArmor;
    public BasicAI ai;

    public bool passedPriority = false;

    private void Awake() {
        shuffleDeck();
        activeBlock = null;
        EventManager.StartListening(TurnMaster.getGenericStartPhaseTrigger(), refreshUI);
        EventManager.StartListening(TurnMaster.getStartPhaseTrigger(Phase.Draw, gameObject.GetInstanceID()), drawStep);
        if (!playerCharacter) {
            EventManager.StartListening(TurnMaster.getEndTurnTrigger(gameObject.GetInstanceID()), turnOffUI);
        }

        if (equippedWeapon != null) {
            equippedWeapon.equip(this);
        }

        if (equippedArmor != null) {
            equippedArmor.equip(this);
        }
    }

    private void OnMouseDown() {
        if (!PauseController.paused) {
            TurnMaster.Instance().checkTargetListener(this);
        }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(2)) {
            refreshUI();
        }
    }

    private void Start() {
        drawStep();
        if (ui == null
         && !playerCharacter) {
            EnemyUIController.Instance().setUI(this);
        }
    }

    public bool hasPlay() {
        bool hasPlay = false;
        if (!passedPriority) {
            foreach (BaseCard card in hand.cards) {
                if (card.details.isCardPlayable()) {
                    hasPlay = true;

                }
            }

            if (!hasPlay
             && playerCharacter
             && isSideboardAllowedNow()
             && atleastOneSideboardLearnable()) {
                hasPlay = true;
            }
        }
        if (hasPlay) {
            if (playerCharacter) {
                TurnMaster.Instance().setContinueButton(true);
            } else {
                ai.nudgeAIForDecision();
            }
            return true;
        } else {
            return false;
        }
    }

    public bool atleastOneSideboardLearnable() {
        foreach (GameObject card in PlayerInfo.Instance().sideboard) {
            if (card.GetComponent<CardDetails>().getFocusLearnCost() <= focus) {
                return true;
            }
        }
        return false;
    }

    public bool equipItem(BaseEquippable item) {
        if (item is Weapon) {
            if (equippedWeapon != null) {
                equippedWeapon.unequip(this);
            }

            equippedWeapon = item as Weapon;
            equippedWeapon.equip(this);
            
            return true;
        } else if (item is Armor) {
            if (equippedArmor != null) {
                equippedArmor.unequip(this);
            }

            equippedArmor = item as Armor;
            equippedArmor.equip(this);

            return true;
        }

        return false;
    }

    public int getWeaponDamage() {
        return (equippedWeapon == null ? 0 : equippedWeapon.weaponDamage) + weaponDamageMod;
    }

    public bool getIfFreeHand() {
        if (equippedWeapon == null) {
            return true;
        } else {
            return equippedWeapon.freeHand;
        }
    }

    public bool checkDodge(CombatCharacter source, float accuracyMod) {
        float rng = Random.Range(0, 100);

        float hitChance = rng + (accuracyMod * source.accuracy) - this.avoidance;

        return hitChance < 100;
    }

    public void getAttacked(int rawDamage, bool ignoreArmor) {
        int dmg = rawDamage;
        if (!ignoreArmor) {
            dmg -= armor;
        }
        int blockValue = 0;
        if (activeBlock != null) {
            blockValue = activeBlock.blockValue;
        }
        dmg -= blockValue;
        dmg = Mathf.Max(0, dmg);
        health -= dmg;
        activeBlock = null;
        refreshUI();
        FloatingDamageTextController.Instance().createFloatingDamageText(transform, rawDamage, armor, blockValue, dmg);

        if (health <= 0) {
            if (playerCharacter) {
                GameAlertUI.Instance().setText("You died!");
            } else {
                TurnMaster.Instance().handleEnemyDeath(this);
                Destroy(gameObject);
            }
        }
    }

    public int getBuffCategoryCount(BuffCategory cat) {
        int cnt = 0;
        foreach (BaseBuff buff in buffs) {
            if (buff.categories.Contains(cat)) {
                cnt++;
            }
        }
        return cnt;
    }

    public bool hasDefendPossibility() {
        return checkIfHasTypeInHand(new CardSubType[] { CardSubType.Fast, CardSubType.Defend });
    }

    public bool hasFastCard() {
        return checkIfHasTypeInHand(new CardSubType[] { CardSubType.Fast });
    }

    public bool checkIfHasTypeInHand(CardSubType[] matchTypes) {
        foreach (BaseCard card in hand.cards) {
            if (card.details.isCardPlayable()) {
                foreach (CardSubType subType in card.details.subTypes) {
                    foreach (CardSubType matchType in matchTypes) {
                        if (subType == matchType) {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    float calculateEnergyPercentage() {
        return ((float)energy) / ((float)energyTurnAmount);
    }

    float calculateFocusPercentage() {
        return 1.0f;
    }

    float calculateHealthPercentage() {
        return ((float)health) / ((float)maxHealth);
    }

    float calculateActionPercentage() {
        return ((float)actions) / ((float)actionTurnAmount);
    }

    private bool isSideboardAllowedNow() {
        return this == TurnMaster.Instance().activeCharacter()
                    && TurnMaster.currentPhase == Phase.Action
                    && StackController.Instance().theStack.Count == 0;
    }

    public virtual void refreshUI() {
        if (ui != null) {
            ui.transform.GetChild(0).gameObject.SetActive(true);
            ui.characterNameText.text = characterName;
            ui.weaponDmgText.text = getWeaponDamage().ToString();
            ui.armorText.text = armor.ToString();
            ui.avoidanceText.text = avoidance.ToString();
            ui.accuracyText.text = accuracy.ToString();
            ui.energyBar.value = calculateEnergyPercentage();
            ui.energyText.text = energy.ToString() + "/" + energyTurnAmount.ToString();
            ui.focusBar.value = calculateFocusPercentage();
            ui.focusText.text = focus.ToString();
            ui.healthBar.value = calculateHealthPercentage();
            ui.healthText.text = health.ToString() + "/" + maxHealth.ToString();
            ui.actionBar.value = calculateActionPercentage();
            ui.actionText.text = actions.ToString() + "/" + actionTurnAmount.ToString();
            if (!playerCharacter) {
                ui.enemyArrow.position = Camera.main.WorldToScreenPoint(arrowPoint.transform.position) + ui.arrowDisplacement;
            }
        }
        if (tabPanel != null) {
            tabPanel.updateTabs(this);
        }

        if (playerCharacter) {
            SideboardController.Instance().addToDeckButton.interactable =
                PlayerInfo.Instance().sideboard.Count > 0
             && SideboardController.Instance().currentPreviewedCard.GetComponent<CardDetails>().getFocusLearnCost() <= focus
             && isSideboardAllowedNow();
        } 
    }

    public void turnOffUI() {
        ui.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void drawStep() {
        discardHand();
        actions = actionTurnAmount;
        energy = energyTurnAmount;
        focus += focusTurnAmount;
        refreshUI();
        drawXFromDeck(cardDrawTurnAmount);
        if (!playerCharacter && hand.cards.Count > 0) {
            hand.cards[0].hiddenCard = false;
        }
        hand.organizeCards();
    }

    public void discardHand() {
        for (int i = hand.cards.Count - 1; i >= 0; i--) {
            hand.discardCard(i);
        }
    }

    public void drawXFromDeck(int X) {
        for (int i = 0; i < X; i++) {
            drawFromDeck();
        }
    }

    public void drawFromDeck() {
        if (deck.deckContents.Count > 0) {
            doDraw();
        } else {
            shuffleDiscardIntoDeck();
            if (deck.deckContents.Count > 0) {
                doDraw();
            }
        }
    }

    public void doDraw() {
        GameObject card = deck.deckContents[0];
        deck.deckContents.RemoveAt(0);
        hand.addNewCard(card);
    }

    public void shuffleDiscardIntoDeck() {
        deck.deckContents.AddRange(discard.discardContents);
        discard.discardContents.Clear();
        shuffleDeck();
    }
    
    public void shuffleDeck() {
        for (int i = 0; i < deck.deckContents.Count; i++) {
            GameObject temp = deck.deckContents[i];
            int randomIndex = Random.Range(i, deck.deckContents.Count);
            deck.deckContents[i] = deck.deckContents[randomIndex];
            deck.deckContents[randomIndex] = temp;
        }
    }

    private static CombatCharacter playerChar;
    public static CombatCharacter Player() {
        if (!playerChar) {
            CombatCharacter[] characters = FindObjectsOfType<CombatCharacter>();
            foreach (CombatCharacter character in characters) {
                if (character.playerCharacter) {
                    playerChar = character;
                    break;
                }
            }
        }
        return playerChar;
    }
}
