using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseCard : Targetable {

    public TextMesh cardNameMesh;
    public TextMesh energyPlayCostMesh;
    public TextMesh focusPlayCostMesh;
    public SpriteRenderer artSprite;
    public TextMesh typelineMesh;
    public SmartTextMesh abilitiesMesh;
    public TextMesh focusLearnCostMesh;
    public MeshRenderer cardMeshRenderer;
    public SpriteRenderer rarityGemSprite;

    public CardDetails details;

    public CombatCharacter character;

    public bool hiddenCard = false;

    public float cardMovementSpeed;
    public float startLerpTime;
    public float lerpDistance;
    public Vector3 startLerpPos;
    public Vector3 endLerpPos;
    public bool lerping = false;
    public Vector3 handPosition;
    public Vector3 hoverDisplacement;
    public bool hovered = false;
    public bool hoverLerping = false;
    public bool beingPlayedLerping = false;

    private bool destroyOnLerpEnd = false;
    public GlowObject glowControl;
    public ParticleSystem magicParticles;

    private void Start() {
        glowControl = GetComponent<GlowObject>();
    }

    private void OnMouseDown() {
        if (!PauseController.paused) {
            details.cardClicked();
        }
    }

    private void OnMouseOver() {
        if (character != null) {
            if (character.playerCharacter
             && character.hand.cards.Contains(this)
             && !hovered
             && !hoverLerping) {
                startLerp(handPosition + transform.TransformVector(hoverDisplacement));
                hoverLerping = true;
            }
        }
    }

    public void setGlow() {
        details.isCardPlayable();
    }

    public void setGlow(bool glowOn) {
        if (character != null && character.playerCharacter && glowControl != null) {
            if (glowOn) {
                glowControl.startGlow();
            } else {
                glowControl.endGlow();
            }
        }
    }
    
    public void loadCardDetails(GameObject cardDetails) {
        GameObject go = Instantiate(cardDetails, transform.transform);
        details = go.GetComponent<CardDetails>();
        details.character = character;
        details.cardBase = this;
        details.prefabRef = cardDetails;
        name = details.cardName;
        string subtypesKey = "";
        magicParticles.gameObject.SetActive(false);
        foreach (CardSubType type in details.subTypes) {
            string subType = type.ToString().ToLower();
            if (subType.Equals("magic")) {
                magicParticles.gameObject.SetActive(true);
            } else {
                subtypesKey += subType;
            }
        }
        cardMeshRenderer.material = MaterialDictionary.Instance().materialDictionary[subtypesKey];
        cardNameMesh.text = details.cardName;
        energyPlayCostMesh.text = details.getEnergyPlayCost().ToString();
        focusPlayCostMesh.text = (details.getFocusPlayCost() > 0 ? details.getFocusPlayCost().ToString() : "");
        artSprite.sprite = details.art;
        typelineMesh.text = details.getTypeLine();
        focusLearnCostMesh.text = details.getFocusLearnCost().ToString();
        rarityGemSprite.sprite = MaterialDictionary.Instance().rarityGemsDictionary[details.rarity];
        resetCardInfo();
    }

    protected string getRepeatText() {
        if (details.repeatCount > 0) {
            return "Card Repeat: " + details.repeatCount + "\n";
        } else {
            return "";
        }
    }

    public void resetCardInfo() {
        abilitiesMesh.UnwrappedText = getRepeatText() + details.getAbilitiesTextBox() + details.getRequirements();
        abilitiesMesh.NeedsLayout = true;
    }

    public void startLerp(Vector3 dest) {
        startLerp(dest, cardMovementSpeed);
    }

    public void startLerp(Vector3 dest, float speed) {
        startLerpPos = transform.position;
        endLerpPos = dest;
        startLerpTime = Time.time;
        lerpDistance = Vector3.Distance(startLerpPos, dest);
        lerping = true;
    }

    private void Update() {
        if (hovered && !EventSystem.current.IsPointerOverGameObject()) {
            hovered = false;
            startLerp(handPosition);
        }

        if (lerping) {
            if (lerpDistance == 0) {
                lerping = false;
            } else {
                float distCovered = (Time.time - startLerpTime) * cardMovementSpeed;
                float fracJourney = distCovered / lerpDistance;
                transform.position = Vector3.Lerp(startLerpPos, endLerpPos, fracJourney);

                if (fracJourney >= 1) {
                    lerping = false;
                    transform.position = endLerpPos;
                }
            }

            if (!lerping) {
                if (destroyOnLerpEnd) {
                    Destroy(gameObject);
                } else if (hoverLerping) {
                    hovered = true;
                    hoverLerping = false;
                    if (EventSystem.current.IsPointerOverGameObject()) {
                        startLerp(handPosition);
                    }
                } else if (beingPlayedLerping) {
                    beingPlayedLerping = false;
                    StackController.Instance().setPostCardMove();
                }
            }
        }
    }

    public void putInDiscard() {
        character.discard.discardContents.Add(details.prefabRef);
        character.refreshUI();

        if (character.playerCharacter) {
            startLerp(character.discard.transform.position, cardMovementSpeed * 3);
            destroyOnLerpEnd = true;
        } else {
            Destroy(gameObject);
        }
    }
}

