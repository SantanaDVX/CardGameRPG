using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {
    public Weapon weapon;
    public Armor armor;

    public Deck deck;

    public int startingHealth;
    public int preadventureFocus;
    public int gold;
    
    public List<GameObject> sideboard;

    private void Awake() {
        deck = GetComponent<Deck>();

    }

    public void loadPlayerInfo() {
        CombatCharacter player = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatCharacter>();

        player.maxHealth = startingHealth;
        player.health = startingHealth;
        player.equipItem(weapon);
        player.equipItem(armor);
        player.deck = deck;
    }

    private static PlayerInfo playerInfo;
    public static PlayerInfo Instance() {
        if (!playerInfo) {
            playerInfo = FindObjectOfType<PlayerInfo>();
        }
        return playerInfo;
    }
}

