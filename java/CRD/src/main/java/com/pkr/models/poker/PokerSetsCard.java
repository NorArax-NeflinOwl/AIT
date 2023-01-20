package com.pkr.models.poker;

import com.google.gson.annotations.SerializedName;
import com.pkr.helpers.poker.PokerResultComparer;
import com.pkr.models.GeneralCard;
import com.pkr.models.interfaces.ISetsCard;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.EmptyStackException;

public class PokerSetsCard implements ISetsCard {
    @SerializedName("setsCard")
    private ArrayList<GeneralCard> cardSet;

    public PokerSetsCard() {
        cardSet = new ArrayList<GeneralCard>();
    }
    public PokerSetsCard(GeneralCard[] cards) {
        cardSet = new ArrayList<>(Arrays.asList(cards));
    }
    public PokerSetsCard(ISetsCard cardsInHand) {
        this.cardSet = new ArrayList<>(cardsInHand.getCardSet());
    }

    public ArrayList<GeneralCard> getCardSet() {
        return cardSet;
    }

    @Override
    public String toString() {
        return "CARD'S_SET: " + cardSet.toString() + "]";
    }

    public GeneralCard getTopCard() throws EmptyStackException {
        if(cardSet.isEmpty()) {
            throw new EmptyStackException();
        }
        GeneralCard result = cardSet.get(0);
        cardSet.remove(0);
        return result;
    }

    @Override
    public void clear() {
        cardSet.clear();
    }

    @Override
    public int compareTo(ISetsCard par) {
        return PokerResultComparer.compare2SetsCards(this, par);
    }
}
