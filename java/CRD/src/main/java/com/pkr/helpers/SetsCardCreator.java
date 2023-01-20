package com.pkr.helpers;

import com.pkr.models.GeneralCard;
import com.pkr.models.enums.CardColorLevel;
import com.pkr.models.enums.CardRangLevel;
import com.pkr.models.interfaces.ISetsCard;
import com.pkr.models.poker.PokerSetsCard;

public class SetsCardCreator {
    private ISetsCard set;

    public SetsCardCreator() {
        set = new PokerSetsCard();
    }

    public ISetsCard getSetsCard() {
        return set;
    }

    public void createSet() {
        for (int i = 0; i < CardColorLevel.values().length; i++) {
            for (int j = 0; j < CardRangLevel.values().length; j++) {
                CardColorLevel color = CardColorLevel.values()[i];
                CardRangLevel rang =CardRangLevel.values()[j];
                set.getCardSet().add(new GeneralCard(rang, color));
            }
        }
    }

    public void shuffle() {
        GeneralCard[] cards = set.getCardSet().toArray(new GeneralCard[0]);
        Randomizer.shuffle(cards);
        set = new PokerSetsCard(cards);
    }
}
