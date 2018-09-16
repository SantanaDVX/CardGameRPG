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



[Serializable]
public enum BuffCategory {
    Discipline,
    Ferocity,
    Ingenuity,

    Enviroment,
    Fire,
    Ice,
    Magic,

    Debilitating,
    Demoralizing,
    Curse

};

[Serializable]
public enum CardProgression {
    Unlearned,
    Trained,
    Adept,
    Proficient,
    Mastered,
    Perfected
}

[Serializable]
public enum CardRarity {
    Basic,
    Advanced,
    Secret,
    Mythical,
    Legendary,
    Unique
}


[Serializable]
public enum LocationType {
    Adventure,
    Story,
    Shop,
    Training
}


[Serializable]
public enum Comparison {
    GT,
    GTE,
    EQ,
    NEQ,
    LTE,
    LT
}



[Serializable]
public enum DaysOfWeek {
    Apreckaday,
    Breckensday,
    Crugsleday,
    Dulmnaday,
    Eugrophday,
    Fronxenday
}

[Serializable]
public enum WeeksOfMonth {
    Glynweek,
    Huegenweek,
    Ittradunweek,
    Japhenweek
}

[Serializable]
public enum MonthsOfYear {
    Koplenmonth,
    Lungermonth,
    Mejhamonth,
    Nawmermonth,
    Oggratukmonth,
    Pagfinmonth,
    Quirtenmonth,
    Rortmonth,
    Seizenmonth,
    Tordorlmonth,
    Ubagornmonth,
    Vlubtorbmonth
}



