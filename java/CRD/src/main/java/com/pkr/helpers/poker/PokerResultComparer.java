package com.pkr.helpers.poker;

import com.pkr.models.GeneralCard;
import com.pkr.models.enums.CardColorLevel;
import com.pkr.models.poker.PokerPlayer;
import com.pkr.models.poker.enums.PokerSetResult;
import com.pkr.models.interfaces.ISetsCard;

public class PokerResultComparer {
    public static int compare2SetsCards(ISetsCard set1, ISetsCard set2) {
        PokerSetResult set1Resutl = calculateResult(set1);
        PokerSetResult set2Resutl = calculateResult(set2);
        return set1Resutl.compareTo(set2Resutl);
    }

    public static PokerSetResult calculateResult(PokerPlayer player) {
        PokerSetResult result = calculateResult(player.getCards2Count());
        player.getCards2Count().clear();
        return result;
    }

    private static PokerSetResult calculateResult(ISetsCard set) {
        PokerSetResult result = null;
        int order = 0;
        boolean fourKind = false;
        boolean threeKind = false;
        boolean pair = false;
        boolean secondPair = false;
        boolean flush = false;
        boolean ordered = false;
        boolean royal = set.getCardSet().get(0).getColor().equals(CardColorLevel.BLACK_SPADE);
        int[] duplicates = new int[13];

        for (int i = 0; i < set.getCardSet().size(); i++) {
            GeneralCard card = set.getCardSet().get(i);
            CardColorLevel firstColor = set.getCardSet().get(0).getColor();
            if(card.getColor().equals(firstColor)) {
                flush = true;
            }
            duplicates[card.getRangindex()]++;
        }

        for(int i = 0; i < 13; i++) {
            if(duplicates[i] > 0) {
                order++;
            }
            else if (duplicates[i] == 0){
                order = 0;
            }
            if(order == 5) ordered = true;
            if(duplicates[i] == 4) fourKind = true;
            if(duplicates[i] == 3) threeKind = true;
            if(pair & duplicates[i] == 2) secondPair = true;
            if(duplicates[i] == 2) pair = true;
        }

        if(flush && ordered && royal)
            result = PokerSetResult.ROYAL_FLUSH;
        if(flush && ordered && null == result)
            result = PokerSetResult.STRAIGHT_FLUSH;
        if(fourKind && null == result)
            result = PokerSetResult.FOUR_KIND;
        if(threeKind && pair && null == result)
            result = PokerSetResult.FULL;
        if(flush && null == result)
            result = PokerSetResult.FLUSH;
        if(ordered && null == result)
            result = PokerSetResult.STRAIGHT;
        if(threeKind && null == result)
            result = PokerSetResult.THREE_KIND;
        if(secondPair && null == result)
            result = PokerSetResult.TWO_PAIR;
        if(pair && null == result)
            result = PokerSetResult.ONE_PAIR;
        if(null == result)
            result = PokerSetResult.HIGH_CARD;

        //System.out.println(result.getKey());
        return result;
    }
}
