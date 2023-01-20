package com.pkr.models;

import com.google.gson.annotations.SerializedName;
import com.pkr.models.enums.CardColorLevel;
import com.pkr.models.enums.CardRangLevel;
import com.pkr.models.strings.CardColor;
import com.pkr.models.strings.CardRang;

import java.util.Comparator;

public class GeneralCard implements Comparable<GeneralCard> {
    @SerializedName("rang")
    private String rang;
    @SerializedName("color")
    private String color;
    private final CardRangLevel constRangLevel;
    private final CardColorLevel constColorLevel;

    public GeneralCard(CardRangLevel rang, CardColorLevel color) {
        this.constRangLevel = rang;
        this.constColorLevel = color;
        setCartConst();
    }

    public String getRang() {
        return rang;
    }

    public CardColorLevel getColor() {
        return constColorLevel;
    }

    public int getRangindex() {
        return constRangLevel.getIndex();
    }

    @Override
    public String toString() {
        return "[RANG: " + rang + "; COLOR: " + color + "]";
    }

    @Override
    public int compareTo(GeneralCard card) {
        return Comparator.comparing(GeneralCard::getColor)
                .thenComparing(GeneralCard::getRang)
                .compare(this, card);
    }

    private void setCartConst() {
        switch (constColorLevel) {
            case GREEN_CLUB:
                color = CardColor.GREEN_CLUB;
                break;
            case BLUE_DIAMOND:
                color = CardColor.BLUE_DIAMOND;
                break;
            case RED_HEART:
                color = CardColor.RED_HEART;
                break;
            case BLACK_SPADE:
                color = CardColor.BLACK_SPADE;
                break;
        }
        switch (constRangLevel) {
            case TWO:
                rang = CardRang.TWO;
                break;
            case THREE:
                rang = CardRang.THREE;
                break;
            case FOUR:
                rang = CardRang.FOUR;
                break;
            case FIVE:
                rang = CardRang.FIVE;
                break;
            case SIX:
                rang = CardRang.SIX;
                break;
            case SEVEN:
                rang = CardRang.SEVEN;
                break;
            case EIGHT:
                rang = CardRang.EIGHT;
                break;
            case NINE:
                rang = CardRang.NINE;
                break;
            case TEN:
                rang = CardRang.TEN;
                break;
            case JACK:
                rang = CardRang.JACK;
                break;
            case QUEEN:
                rang = CardRang.QUEEN;
                break;
            case KING:
                rang = CardRang.KING;
                break;
            case ACE:
                rang = CardRang.ACE;
                break;
        }
    }
}