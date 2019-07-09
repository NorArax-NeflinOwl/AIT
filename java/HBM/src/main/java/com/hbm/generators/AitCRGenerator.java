package com.hbm.generators;

import com.hbm.managers.AitIdManager;
import com.ptl.managers.AitLogger;
import com.ptl.resources.Prefix;

public class AitCRGenerator {
    public static void main(String[] args) {
        AitLogger.getInstance().logToConsole(createCR());
    }

    public static String createCR() {
        return AitIdManager.getInstance().generateId(Prefix.CR);
    }
}
