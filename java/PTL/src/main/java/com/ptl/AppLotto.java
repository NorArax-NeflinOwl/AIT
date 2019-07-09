package com.ptl;

import com.ptl.managers.AitLottoInformator;

public class AppLotto {

    public static void main(String[] args) {
        try {
            if(new AitLottoInformator().checkLottoLastResult()) {
                System.out.println("SUCCESS!!!");
            } else {
                System.out.println("Meaby next time :/");
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
