package com.pkr.models.poker;

import com.pkr.models.interfaces.IPlayer;
import com.pkr.models.interfaces.ISetsCard;

import java.util.Comparator;

public class PokerPlayer implements IPlayer, Comparable<PokerPlayer> {
    private int coins;
    private final String name;
    private final ISetsCard cardsInHand;
    private final ISetsCard cards2Count;
    private boolean active;

    public PokerPlayer(String name) {
        this.name = name;

        coins = 0;
        active = false;
        cardsInHand = new PokerSetsCard();
        cards2Count = new PokerSetsCard();
    }

    public PokerPlayer(PokerPlayer player) {
        this.coins = player.coins;
        this.name = player.name;
        this.cardsInHand = new PokerSetsCard(player.cardsInHand);
        this.cards2Count = new PokerSetsCard(player.cards2Count);
        this.active = player.active;
    }

    public String getName() {
        return name;
    }
    public void setCoins(int coins) {
        this.coins = coins;
        if(this.coins < 0)
            this.coins = 0;
    }
    public int getCoins() {
        return coins;
    }
    public boolean getActive() {
        return active;
    }
    public void setActive(boolean active) {
        this.active = active;
    }
    public ISetsCard getCardsInHand() {
        return cardsInHand;
    }
    public ISetsCard getCards2Count() {
        return cards2Count;
    }
    @Override
    public String toString() {
        return "[NAME: " + name + "; COINS: " + coins + "]";
    }

    @Override
    public int compareTo(PokerPlayer player) {
        return Comparator.comparing(PokerPlayer::getCards2Count)
                .thenComparing(PokerPlayer::getCardsInHand)
                .thenComparing(PokerPlayer::getName)
                .thenComparingInt(PokerPlayer::getCoins)
                .thenComparing(PokerPlayer::getActive)
                .compare(this, player);
    }
}
