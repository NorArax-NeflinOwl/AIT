package com.pkr.models.interfaces;

public interface IPlayer {
    String getName();
    void setCoins(int coins);
    int getCoins();
    boolean getActive();
    void setActive(boolean active);
    ISetsCard getCardsInHand();
}
