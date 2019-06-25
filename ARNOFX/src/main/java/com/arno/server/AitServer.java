package com.arno.server;

import com.arno.managers.AitLogger;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.Vector;

public class AitServer {
    static Vector<AitClientHandler> clientHandlers = new Vector<>();
    private static int counter = 0;

    public void run() throws Exception {
        AitLogger.getInstance().logToConsole(new Object[] {
                "arno.server starting..." });
        ServerSocket ss = new ServerSocket(1234);
        Socket s;

        while(true) {
            s = ss.accept();
            AitLogger.getInstance().logToConsole(new Object[] {
                    "New arno.client request received: " + s });

            DataInputStream dis = new DataInputStream(s.getInputStream());
            DataOutputStream dos = new DataOutputStream(s.getOutputStream());

            AitLogger.getInstance().logToConsole(new Object[] {
                    "Creating a new handler for this arno.client..." });

            AitClientHandler mtch = new AitClientHandler(s, counter, dis, dos);

            Thread t = new Thread(mtch);

            AitLogger.getInstance().logToConsole(new Object[] {
                    "Adding this arno.client to active arno.client list" });

            clientHandlers.add(mtch);
            t.start();
            mtch.connect();
            counter++;
        }
    }
}

