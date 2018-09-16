﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseBuff : MonoBehaviour {
    public string buffName;
    public string tooltip;
    public Sprite buffIcon;
    public int durationCnt = 1;
    public BuffDuration duration;
    public List<BuffCategory> categories;

    string eventName;

    public CombatCharacter target;

    public void createBuff(CombatCharacter source, CombatCharacter target) {
        this.target = target;
        target.buffs.Add(this);
        applyBuff(1);
        target.refreshUI();
        
        switch (duration) {
            case BuffDuration.UntilEndOfTurn: {
                eventName = TurnMaster.getEndTurnTrigger(source.gameObject.GetInstanceID());
                break;
            }
            case BuffDuration.UntilStartOfNextTurn: {
                eventName = TurnMaster.getStartTurnTrigger(source.gameObject.GetInstanceID());
                break;
            }
            case BuffDuration.UntilEndOfTargetsTurn: {
                eventName = TurnMaster.getEndTurnTrigger(target.gameObject.GetInstanceID());
                break;
            }
        }

        EventManager.StartListening(eventName, buffDurationDecrement);
    }

    public void removeDebuff() {
        while (durationCnt > 0) {
            buffDurationDecrement();
        }
    }

    private void buffDurationDecrement() {
        durationCnt--;
        if (durationCnt <= 0) {
            applyBuff(-1);
            target.buffs.Remove(this);
            target.refreshUI();
            EventManager.StopListening(eventName, buffDurationDecrement);
            Destroy(gameObject);
        }
    }

    public abstract void applyBuff(int mult);

}
