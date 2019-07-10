package com.hbm.generators;

import com.hbm.managers.AitIdManager;
import com.ptl.managers.AitLogger;
import com.ptl.resources.AitInerfix;
import com.ptl.resources.AitPostfix;
import com.ptl.resources.AitPrefix;

public class AitREQGenerator {
    public static void main(String[] args) {
        AitLogger.getInstance().logToConsole(createJavaREQ());
    }

    public static String createJavaREQ() {
        return AitIdManager.getInstance().generateId(AitPrefix.REQ, AitInerfix.AIT, AitPostfix.JV);
    }

    public static String createCSREQ() {
        return AitIdManager.getInstance().generateId(AitPrefix.REQ, AitInerfix.AIT, AitPostfix.CS);
    }


}
