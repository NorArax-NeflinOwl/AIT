package com.hbm.generators;

import com.hbm.managers.AitIdManager;
import com.ptl.managers.AitLogger;
import com.ptl.resources.AitInerfix;
import com.ptl.resources.AitPrefix;

public class AitISSGenerator {
    public static void main(String[] args) {
        if(args.length == 0) {
            AitLogger.getInstance().logToConsole(createSimpleISS());
        } else if (args.length == 1) {
            AitLogger.getInstance().logToConsole(createISS2CK());
        } else if (args.length == 2) {
            AitLogger.getInstance().logToConsole(createISS2FIX());
        }
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
