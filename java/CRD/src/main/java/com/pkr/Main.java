package com.pkr;

import com.pkr.helpers.CardsSerializer;
import com.pkr.helpers.SetsCardCreator;
import com.pkr.models.poker.PokerMain;

public class Main {
    public static void main(String[] args) {
        for(int i = 0; i < 1; i++) {
            //System.out.print(i + 1 + " ");
            test2();
        }
    }

    private static void test1() {
        SetsCardCreator creator = new SetsCardCreator();
        creator.createSet();

        CardsSerializer serializer = new CardsSerializer();
        String json = serializer.serialize(creator.getSetsCard());

        System.out.println(json);
        System.out.println(creator.getSetsCard());
    }

    private static void test2() {
        PokerMain main = new PokerMain();
        main.gameTest();
    }
}