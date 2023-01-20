package com.pkr.models.enums;

public enum CardColorLevel {
    GREEN_CLUB(0),
    BLUE_DIAMOND(1),
    RED_HEART(2),
    BLACK_SPADE(3);
    private final int value;
    CardColorLevel (int value) {
        this.value = value;
    }
}