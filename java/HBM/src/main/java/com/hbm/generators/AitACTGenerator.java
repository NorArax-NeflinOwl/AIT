package com.hbm.generators;

import com.hbm.managers.AitIdManager;
import com.ptl.managers.AitLogger;
import com.ptl.resources.AitInerfix;
import com.ptl.resources.AitPrefix;

public class AitACTGenerator {
    public static void main(String[] args) {
        if(args.length == 0) {
            AitLogger.getInstance().logToConsole(createSimpleACK());
        } else if (args.length == 1) {
            AitLogger.getInstance().logToConsole(createACK2CR());
        } else if (args.length == 2) {
            AitLogger.getInstance().logToConsole(createACK2CK());
        } else if (args.length == 3) {
            AitLogger.getInstance().logToConsole(createACK2FIX());
        }
    }

    public static String createSimpleACK() {
        return AitIdManager.getInstance().generateId(AitPrefix.ACK);
    }

    public static String createACK2CR() {
        return AitIdManager.getInstance().generateId(AitPrefix.ACK, AitInerfix.CR);
    }

    public static String createACK2CK() {
        return AitIdManager.getInstance().generateId(AitPrefix.ACK, AitInerfix.CK);
    }

    public static String createACK2FIX() {
        return AitIdManager.getInstance().generateId(AitPrefix.ACK, AitInerfix.FIX);
    }
}
