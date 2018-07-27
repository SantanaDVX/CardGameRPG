using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {
    public int currentHealth;
    public Weapon weapon;
    public Armor armor;

    public Deck deck;
    
    public List<GameObject> sideboard;

    private void Awake() {
        deck = GetComponent<Deck>();
    }

    public void loadPlayerInfo() {
        CombatCharacter player = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatCharacter>();

        player.health = currentHealth;
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

