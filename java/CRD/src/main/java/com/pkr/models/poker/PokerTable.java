package com.pkr.models.poker;

import com.pkr.helpers.Randomizer;
import com.pkr.helpers.SetsCardCreator;
import com.pkr.helpers.poker.PokerResultComparer;
import com.pkr.models.GeneralCard;
import com.pkr.models.interfaces.ISetsCard;
import com.pkr.models.poker.enums.PokerSetResult;

import java.util.ArrayList;

public class PokerTable {
    private final ArrayList<PokerPlayer> pokerPlayers;
    private final ISetsCard cardsOnTable;
    private ISetsCard allSet;
    private int coinsOnTable;

    public PokerTable() {
        pokerPlayers = new ArrayList<>();
        allSet = new PokerSetsCard();
        cardsOnTable = new PokerSetsCard();
        coinsOnTable = 0;
    }

    public void join(PokerPlayer pokerPlayer) {
        if(!pokerPlayers.contains(pokerPlayer)) {
            pokerPlayers.add(pokerPlayer);
        }
        pokerPlayer.setActive(true);
    }
    public void quit(PokerPlayer pokerPlayer) {
        pokerPlayer.setActive(false);
    }
    public void createSetsCard() {
        SetsCardCreator creator = new SetsCardCreator();
        creator.createSet();
        creator.shuffle();
        allSet = creator.getSetsCard();
    }
    public void preFlop() {
        giveCards();
    }

    public void flop() {
        putCard(3);
    }

    public void turn() {
        putCard(1);
    }

    public void river() {
        putCard(1);
    }

    public void playersMove() {
        int priceUp = 0;
        for(PokerPlayer player : pokerPlayers) {
            if(player.getActive()) {
                if (Randomizer.makeMove()) {
                    if (priceUp == 0) {
                        priceUp = Randomizer.priceUp(player.getCoins());
                    }
                    player.setCoins(player.getCoins() - priceUp);
                    coinsOnTable += priceUp;
                } else if (priceUp > 0) {
                    quit(player);
                }
            }
        }
    }

    public PokerPlayer[] getPlayersRanking() {
        PokerPlayer[] result = null;
        ArrayList<PokerPlayer> players = new ArrayList<>();
        for(PokerPlayer player : pokerPlayers) {
            if(player.getActive()) {
                players.add(player);
            }
        }
        for (PokerPlayer player : players) {
            player.getCards2Count().getCardSet().addAll(player.getCardsInHand().getCardSet());
            player.getCards2Count().getCardSet().addAll(cardsOnTable.getCardSet());
        }
        players.sort(PokerPlayer::compareTo);

        result = players.toArray(new PokerPlayer[0]);
        result[players.size()-1].setCoins(coinsOnTable);
        coinsOnTable = 0;
        return result;
    }

    public void clearTable() {
        ISetsCard result = new PokerSetsCard();
        for(PokerPlayer player : pokerPlayers) {
            result.getCardSet().addAll(player.getCardsInHand().getCardSet());
            player.getCardsInHand().clear();
            player.setActive(player.getCoins() > 0);
        }
        result.getCardSet().addAll(cardsOnTable.getCardSet());
        cardsOnTable.clear();

        allSet.getCardSet().addAll(result.getCardSet());
    }

    public boolean keepPlayingAvailable() {
        int playersAvailable = 0;
        for(PokerPlayer player : pokerPlayers) {
            if(player.getActive()) {
                playersAvailable++;
            }
        }
        boolean result = playersAvailable >= 2;
        if(result) {
            return result;
        }

        for(PokerPlayer player : pokerPlayers) {
            System.out.println(player);
        }

        return result;
    }

    private void giveCards() {
        for(int i = 0; i < 2; i++) {
            for(PokerPlayer player : pokerPlayers) {
                if(player.getActive()) {
                    GeneralCard card = allSet.getTopCard();
                    player.getCardsInHand().getCardSet().add(card);
                }
            }
        }
    }

    private void putCard(int amount) {
        for(int i = 0; i < amount; i++) {
            GeneralCard card = allSet.getTopCard();
            cardsOnTable.getCardSet().add(card);
        }
    }
}
