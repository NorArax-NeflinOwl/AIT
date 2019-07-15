package com.hbm.generators;

import com.hbm.managers.AitIdManager;
import com.hbm.resources.AitPrefix;

public class AitCRGenerator {
    public static void main(String[] args) {
        System.out.println(createCR());
    }

    public static String createCR() {
        return AitIdManager.getInstance().generateId(AitPrefix.CR);
    }
}
