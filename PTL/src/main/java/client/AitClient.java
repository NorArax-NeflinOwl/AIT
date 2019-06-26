package client;

import managers.AitCrypter;
import managers.AitLogger;
import managers.AitReceiverSender;
import resources.AitConsoleColor;
import resources.AitLoggerPriority;
import structures.AitClientData;
import structures.AitPackageData;
import com.google.gson.Gson;

import java.io.IOException;
import java.net.InetAddress;
import java.net.Socket;
import java.util.ArrayList;
import java.util.Map;
import java.util.Scanner;

import static resources.AitCommandTypes.*;

public class AitClient {
    private final static int ServerPort = 1234;

    private AitClientData clientData;
    private AitReceiverSender aitReceiverSender;
    private Thread sendMessage;
    private Scanner scn;
    private Socket s;

    public AitClient() {
        this.scn = new Scanner(System.in);
    }

    public void run() throws IOException {
        try {
            InetAddress ip = InetAddress.getByName("localhost");
            this.s = new Socket(ip, ServerPort);
            this.aitReceiverSender = new AitReceiverSender(s);
        } catch (IOException e) {
            try {
                AitLogger.getInstance().logToFile(e.getStackTrace(), e.toString());
            } catch (Exception exc) {
                e.printStackTrace();
                exc.printStackTrace();
            }
            if (this.s != null) {
                this.s.close();
            }
        }

        sendMessage = new Thread(() -> {
            while(true) {
                if(clientData.getIsLoggedIn()) {

                    String msg = scn.nextLine();

                    try {
                        aitReceiverSender.cryptMsgAndSend(msg);
                    } catch (Exception e) {
                        try {
                            aitReceiverSender.close();
                        } catch (IOException ex) {
                            try {
                                AitLogger.getInstance().logToFile(e.getStackTrace(), e.toString());
                            } catch (Exception exc) {
                                ex.printStackTrace();
                                exc.printStackTrace();
                            }
                        }
                    }
                }
            }
        });

        Thread readMessage = new Thread(() -> {
            while(true) {
                try {
                    String msg = aitReceiverSender.decryptMsg();
                    msgProccessing(msg);
                }catch (Exception e) {
                    try {
                        aitReceiverSender.close();
                        try {
                            AitLogger.getInstance().logToFile(e.getStackTrace(), e.toString());
                        } catch (Exception exc) {
                            e.printStackTrace();
                            exc.printStackTrace();
                        }
                    } catch (IOException ex) {
                        try {
                            AitLogger.getInstance().logToFile(e.getStackTrace(), e.toString());
                        } catch (Exception exc) {
                            ex.printStackTrace();
                            exc.printStackTrace();
                        }
                    }
                }
            }
        });
        readMessage.start();
    }

    private void msgProccessing(String msg) throws Exception {
        Gson gson = new Gson();
        AitPackageData data = gson.fromJson(msg, AitPackageData.class);
        if(data != null) {

            if(clientData != null && clientData.getIsWaiting()
                    && data.getType() == StopWaiting) {

                msg = data.getParams().get(StopWaiting.toString());
                if(clientData.getIsWaiting()) {

                    clientData.setIsWaiting();
                    AitLogger.getInstance().logToConsole(new Object[] { msg });
                }
            }
            else {
                switch (data.getType()){
                    case ClientInitConnection:
                        AitLogger.getInstance().logToConsole(new Object[] {
                                "Connected... " });
                        login(data);
                        break;
                    case SendFrom:
                        ArrayList<String> msgs = new ArrayList<>();
                        for (Map.Entry<String, String> entry : data.getParams().entrySet()) {
                            msgs.add("From " + entry.getKey() + ": " + entry.getValue());
                        }

                        AitLogger.getInstance().logToConsole(msgs.toArray());
                        break;
                    case LoginSuc:
                        msg = data.getParams().get(LoginSuc.toString());
                        if(!clientData.getIsLoggedIn()) {

                            clientData.setIsLoggedIn();
                            AitLogger.getInstance().logToConsole(new Object[] { msg });

                            sendMessage.start();
                        }
                        break;
                    case LogoutSuc:
                        msg = data.getParams().get(LogoutSuc.toString());
                        if(clientData.getIsLoggedIn()) {

                            clientData.setIsLoggedIn();
                            AitLogger.getInstance().logToConsole(new Object[] { msg });
                            showCommand();
                        }
                        break;
                    case Wait:
                        msg = data.getParams().get(Wait.toString());
                        if(!clientData.getIsWaiting()) {

                            clientData.setIsWaiting();
                            AitLogger.getInstance().logToConsole(new Object[] { msg });
                        }
                        break;
                    case RejestractionRequest:
                        msg = data.getParams().get(RejestractionRequest.toString());
                        AitLogger.getInstance().logToConsole(new Object[] { msg }, AitLoggerPriority.Warting);
                        createClient();
                        break;
                    case RejestractionSuc:
                        msg = data.getParams().get(RejestractionSuc.toString());
                        AitLogger.getInstance().logToConsole(new Object[] { msg });
                        login(data);
                        break;
                    case RejestractionInvalid:
                        msg = data.getParams().get(RejestractionInvalid.toString());
                        AitLogger.getInstance().logToConsole(new Object[] { msg });
                        login(data);
                        break;
                    case LoginInvalid:
                        msg = data.getParams().get(LoginInvalid.toString());
                        AitLogger.getInstance().logToConsole(new Object[] { msg });
                        login(data);
                        break;
                    default:
                        msg = data.getParams().get(data.getType().toString());
                        AitLogger.getInstance().logToConsole(new Object[] { msg });
                        break;
                }
            }

        }
    }

    private void login(AitPackageData data) {
        Gson gson = new Gson();
        clientData = new AitClientData(data.getClient().getId());
        System.out.print(String.format(AitConsoleColor.ANSI_BLUE_LINE, "Login: "));
        String login = scn.nextLine();
        System.out.print(String.format(AitConsoleColor.ANSI_BLUE_LINE, "Password: "));
        String password = scn.nextLine();
        if(!login.isEmpty() && !password.isEmpty()) {

            try {
                clientData.setLogin(login);
                clientData.setPassword(AitCrypter.generateMD5Hash(password));

                AitPackageData pack = new AitPackageData(clientData, Login, null);
                aitReceiverSender.cryptMsgAndSend(gson.toJson(pack));

                AitLogger.getInstance().logToConsole(new Object[] {
                        "Login send... " });
            } catch (Exception e) {
                try {
                    AitLogger.getInstance().logToFile(e.getStackTrace(), e.toString());
                }catch (Exception ex) {
                    e.printStackTrace();
                    ex.printStackTrace();
                }
            }
        }
    }

    private void createClient() throws Exception {
        AitLogger.getInstance().logToConsole(new Object[] { "Please fill below field" });

        System.out.print(String.format(AitConsoleColor.ANSI_CYAN_LINE, "Password: "));
        String pass = scn.nextLine();
        System.out.print(String.format(AitConsoleColor.ANSI_CYAN_LINE, "Repeat password: "));
        String rpass = scn.nextLine();

        if(!pass.equals(rpass)) {
            AitLogger.getInstance().logToConsole(new Object[] { "Passwords is not equals! Try again" }, AitLoggerPriority.Warting);
            createClient();
        } else {
            pass = AitCrypter.generateMD5Hash(pass);
            clientData.setPassword(pass);
            AitLogger.getInstance().logToConsole(new Object[] { "Accout is creating..." });

            Gson gson = new Gson();
            AitPackageData pack = new AitPackageData(clientData, RejestractionRespons, null);
            aitReceiverSender.cryptMsgAndSend(gson.toJson(pack));
        }
    }

    private void showCommand() {
        //TODO display in console list of command to use by gui.client to communicate with another gui.client or account managment
    }
}
