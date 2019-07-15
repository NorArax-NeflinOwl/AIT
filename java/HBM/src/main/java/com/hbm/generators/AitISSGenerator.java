package com.hbm.generators;

import com.hbm.managers.AitIdManager;
import com.hbm.resources.AitInerfix;
import com.hbm.resources.AitPrefix;

public class AitISSGenerator {
    public static void main(String[] args) {
        System.out.println(createSimpleISS());
    }

    public static String createSimpleISS() {
        return AitIdManager.getInstance().generateId(AitPrefix.ISS);
    }

    public static String createISS2CK() {
        return AitIdManager.getInstance().generateId(AitPrefix.ISS, AitInerfix.ANL);
    }

    public static String createISS2FIX() {
        return AitIdManager.getInstance().generateId(AitPrefix.ISS, AitInerfix.FIX);
    }
}
