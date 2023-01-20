package com.pkr.helpers;

import java.security.SecureRandom;

public class Randomizer {
    public static void shuffle(Object[] list) {
        SecureRandom random = new SecureRandom();
        int n = list.length;
        while (n > 1) {
            byte[] box = new byte[1];
            do {
                random.nextBytes(box);
            }
            while (!(box[0] < n * (Byte.MAX_VALUE / n)));
            int k = Math.abs(box[0] % n);
            n--;
            Object value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static boolean makeMove() {
        SecureRandom random = new SecureRandom();
        return random.nextBoolean();
    }

    public static int priceUp(int amount) {
        int result = amount;
        SecureRandom random = new SecureRandom();
        if(random.nextBoolean()) {
            result -= random.nextInt(10);
        } else if (random.nextBoolean()) {
            result -= random.nextInt(100);
        } else {
            result -= random.nextInt(amount);
        }

        return result;
    }
}
