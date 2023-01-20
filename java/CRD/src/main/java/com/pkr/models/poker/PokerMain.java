package com.pkr.models.poker;

import com.pkr.helpers.poker.PokerResultComparer;

public class PokerMain {
    public void gameTest() {
        int coins = 100;
        boolean keepPlaying = true;
        PokerTable table = new PokerTable();
        table.createSetsCard();

        PokerPlayer player = new PokerPlayer("test");
        player.setCoins(coins);
        table.join(player);

        for(int i = 1; i < 5; i++) {
            PokerPlayer boot = new PokerPlayer("boot" + i);
            boot.setCoins(coins);
            table.join(boot);
        }

        while(keepPlaying) {
            table.preFlop();
            table.playersMove();
            table.flop();
            table.playersMove();
            table.turn();
            table.playersMove();
            table.river();

            PokerPlayer[] players = table.getPlayersRanking();

            System.out.print(players[players.length - 1].getName());
            System.out.print(" took first place with ");
            System.out.print(PokerResultComparer.calculateResult(players[players.length - 1]));
            System.out.println(" and won " + players[players.length - 1].getCoins());

            table.clearTable();
            keepPlaying = table.keepPlayingAvailable();
        }
    }
}
