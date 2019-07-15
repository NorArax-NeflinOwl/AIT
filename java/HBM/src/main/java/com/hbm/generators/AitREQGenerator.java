package com.hbm.generators;

import com.hbm.managers.AitIdManager;
import com.hbm.resources.AitInerfix;
import com.hbm.resources.AitPostfix;
import com.hbm.resources.AitPrefix;

public class AitREQGenerator {
    public static void main(String[] args) {
        System.out.println(createJavaREQ());
    }

    public static String createJavaREQ() {
        return AitIdManager.getInstance().generateId(AitPrefix.REQ, AitInerfix.AIT, AitPostfix.JV);
    }

    public static String createCSREQ() {
        return AitIdManager.getInstance().generateId(AitPrefix.REQ, AitInerfix.AIT, AitPostfix.CS);
    }


}
