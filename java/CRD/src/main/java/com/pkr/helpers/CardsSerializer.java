package com.pkr.helpers;

import com.google.gson.Gson;
import com.pkr.models.interfaces.ISetsCard;
import com.pkr.models.poker.PokerSetsCard;

public class CardsSerializer {
    private Gson serializer;

    public CardsSerializer() {
        serializer = new Gson();
    }

    public String serialize(ISetsCard set) {
        return serializer.toJson(set);
    }

    public PokerSetsCard deserialize(String json) {
        return serializer.fromJson(json, PokerSetsCard.class);
    }
}
