package com.pkr.models.poker.enums;

public enum PokerSetResult {
    HIGH_CARD(0),
    ONE_PAIR(1),
    TWO_PAIR(2),
    THREE_KIND(3),
    STRAIGHT(4),
    FLUSH(5),
    FULL(6),
    FOUR_KIND(7),
    STRAIGHT_FLUSH(8),
    ROYAL_FLUSH(9);

    private final int value;
    PokerSetResult (int value) {
        this.value = value;
    }
}
