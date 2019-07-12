package com.ptl;

import com.ptl.managers.AitMailSender;
import com.ptl.server.AitServer;

public class AppPTL {
    public static void main(String[] args) {
        try {
            AitMailSender sender = new AitMailSender();
            sender.sendTest();
            AitServer server = new AitServer();
            //server.run();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
