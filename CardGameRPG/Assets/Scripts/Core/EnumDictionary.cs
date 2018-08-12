using System;

[Serializable]
public enum Phase {
    Upkeep,
    Draw,
    Action,
    End
};

[Serializable]
public enum AnimagusTrack {
    Agility,
    Might,
    Reflex,
    Concentration
};

[Serializable]
public enum TargetCategory {
    Enemy,
    CardInOwnHand
};

[Serializable]
public enum SubphaseAction {
    WaitingForPlayerInput,
    Targeting,
    Animating,
    WaitingForDefenseResponse
}

[Serializable]
public enum CardType {
    Action,
    Item    
};

[Serializable]
public enum CardSubType {
    Attack,
    Defend,
    Skill,
    Fast,
    Magic
};

[Serializable]
public enum BuffDuration {
    UntilEndOfTurn,
    UntilEndOfTargetsTurn,
    UntilStartOfNextTurn
};

[Serializable]
public enum ResourceType {
    Energy,
    Focus
};



