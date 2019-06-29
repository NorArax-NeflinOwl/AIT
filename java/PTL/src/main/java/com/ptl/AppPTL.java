package com.ptl;

import com.ptl.server.AitServer;

public class AppPTL {
    public static void main(String args[]) {
        try {
            AitServer server = new AitServer();
            server.run();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
