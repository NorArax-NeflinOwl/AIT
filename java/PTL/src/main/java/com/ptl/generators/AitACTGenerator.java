package com.ptl.generators;

import com.ptl.managers.AitIdManager;
import com.ptl.managers.AitLogger;
import com.ptl.resources.Inerfix;
import com.ptl.resources.Postfix;
import com.ptl.resources.Prefix;

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
        return AitIdManager.getInstance().generateId(Prefix.ACK);
    }

    public static String createACK2CR() {
        return AitIdManager.getInstance().generateId(Prefix.ACK, Inerfix.CR, Postfix.LCL);
    }

    public static String createACK2CK() {
        return AitIdManager.getInstance().generateId(Prefix.ACK, Inerfix.CK, Postfix.LCL);
    }

    public static String createACK2FIX() {
        return AitIdManager.getInstance().generateId(Prefix.ACK, Inerfix.FIX, Postfix.LCL);
    }
}
