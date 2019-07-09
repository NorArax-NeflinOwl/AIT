package com.ptl.generators;

import com.ptl.managers.AitIdManager;
import com.ptl.managers.AitLogger;
import com.ptl.resources.Inerfix;
import com.ptl.resources.Postfix;
import com.ptl.resources.Prefix;

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
        return AitIdManager.getInstance().generateId(Prefix.ISS, Postfix.LCL);
    }

    public static String createISS2CK() {
        return AitIdManager.getInstance().generateId(Prefix.ISS, Inerfix.CK, Postfix.LCL);
    }

    public static String createISS2FIX() {
        return AitIdManager.getInstance().generateId(Prefix.ISS, Inerfix.FIX, Postfix.LCL);
    }
}
