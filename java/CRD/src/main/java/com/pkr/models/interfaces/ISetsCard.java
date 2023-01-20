package com.pkr.models.interfaces;

import com.pkr.models.GeneralCard;

import java.util.ArrayList;

public interface ISetsCard extends Comparable<ISetsCard> {
    ArrayList<GeneralCard> getCardSet();
    GeneralCard getTopCard();
    void clear();
}
