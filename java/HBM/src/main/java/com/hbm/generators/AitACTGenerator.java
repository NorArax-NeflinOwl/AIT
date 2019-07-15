package com.hbm.generators;

import com.hbm.managers.AitIdManager;
import com.hbm.resources.AitInerfix;
import com.hbm.resources.AitPrefix;

public class AitACTGenerator {
    public static void main(String[] args) {
        System.out.println(createSimpleACK());
    }

    public static String createSimpleACK() {
        return AitIdManager.getInstance().generateId(AitPrefix.ACK);
    }

    public static String createACK2CR() {
        return AitIdManager.getInstance().generateId(AitPrefix.ACK, AitInerfix.CR);
    }

    public static String createACK2CK() {
        return AitIdManager.getInstance().generateId(AitPrefix.ACK, AitInerfix.ANL);
    }

    public static String createACK2FIX() {
        return AitIdManager.getInstance().generateId(AitPrefix.ACK, AitInerfix.FIX);
    }
}
