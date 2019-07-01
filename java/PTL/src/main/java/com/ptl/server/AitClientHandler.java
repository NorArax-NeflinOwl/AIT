package com.ptl.server;

import com.google.gson.Gson;
import com.ptl.database.AitLocalDB;
import com.ptl.managers.AitLogger;
import com.ptl.managers.AitReceiverSender;
import com.ptl.structures.AitClientData;
import com.ptl.structures.AitPackageData;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.net.Socket;
import java.util.HashMap;
import java.util.Map;
import java.util.Objects;

import static com.ptl.resources.AitCommandTypes.*;

public class AitClientHandler implements Runnable {
    private AitClientData clientData;
    private AitReceiverSender aitReceiverSender;
    private Socket socket;

    AitClientHandler(Socket socket, Integer id, DataInputStream dis, DataOutputStream dos) {
        this.socket = socket;
        this.clientData = new AitClientData(id);
        this.aitReceiverSender = new AitReceiverSender(dis,dos);
    }

    @Override
    public void run() {
        try {
            AitLogger.getInstance().logToConsole(new Object[] {
                    "com/ptl/client " + clientData.getId() + " connecting..." });
        } catch (Exception e) {
            try {
                AitLogger.getInstance().logToFile(e.getStackTrace(), e.toString());
            } catch (Exception exc) {
                e.printStackTrace();
                exc.printStackTrace();
            }
        }

        String received;
        while (true) {
            try {
                received = aitReceiverSender.decryptMsg();
                msgProccessing(received);
            }catch (Exception e) {
                try {
                    AitLogger.getInstance().logToFile(e.getStackTrace(), e.toString());
                } catch (Exception exc) {
                    e.printStackTrace();
                    exc.printStackTrace();
                }

                if(clientData.getIsConnected()) {
                    clientData.setIsConnected();
                }
                close();
                break;
            }
        }
    }

    void connect(){
        try {
            Gson gson = new Gson();
            AitPackageData data = new AitPackageData(clientData, ClientInitConnection, null);

            AitClientHandler mc = AitServer.clientHandlers.elementAt(clientData.getId());
            if(mc.clientData.getIsConnected()) {

                mc.aitReceiverSender.cryptMsgAndSend(gson.toJson(data));
            }
        }
        catch (Exception e) {
            try {
                AitLogger.getInstance().logToFile(e.getStackTrace(), e.toString());
            } catch (Exception exc) {
                e.printStackTrace();
                exc.printStackTrace();
            }
        }
    }

    private boolean canSend(AitClientHandler mc) {
        return canProccessing() && mc.canProccessing();
    }

    private boolean canProccessing() {
        return clientData.canProccessing();
    }

    private void msgProccessing(String msg) throws Exception {
        Gson gson = new Gson();
        AitPackageData data = gson.fromJson(msg, AitPackageData.class);
        if(data != null) {

            if(canProccessing()) {

                Map<String, String> params = data.getParams();
                switch (data.getType()) {
                    case SendTo:
                        if(params != null) {

                            for (Map.Entry<String, String> entry : params.entrySet()) {
                                for(AitClientHandler mc : AitServer.clientHandlers) {

                                    if(mc.clientData.getId().equals(Integer.parseInt(entry.getKey())) && canSend(mc)) {
                                        sendTo(mc, entry);
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    case SendBct:
                        if(params != null) {

                            for(AitClientHandler mc : AitServer.clientHandlers) {
                                if(canSend(mc)) {
                                    send(mc, params);
                                }
                            }
                        }
                        break;
                    case Logout:
                        if(clientData.getIsLoggedIn()){
                            clientData.setIsLoggedIn();

                            params = new HashMap<>();
                            params.put(LoginSuc.toString(), "Log out success :)");

                            data = new AitPackageData(clientData, LogoutSuc, params);
                            aitReceiverSender.cryptMsgAndSend(gson.toJson(data));
                        }
                        break;
                }
            }
            else if(clientData.getIsConnected()) {

                switch (data.getType())
                {
                    case Login:
                        String login = data.getClient().getLogin();
                        String password = data.getClient().getPassword();
                        if(validLogin(login))
                        {
                            Map<String, String> params = new HashMap<>();
                            if(validPassword(login, password)) {

                                if (!clientData.getIsLoggedIn()) {
                                    clientData.setLogin(login);

                                    clientData.setIsLoggedIn();
                                    params.put(LoginSuc.toString(), "Log is success :)");

                                    data = new AitPackageData(clientData, LoginSuc, params);
                                    aitReceiverSender.cryptMsgAndSend(gson.toJson(data));

                                    AitLogger.getInstance().logToConsole(new Object[]{
                                            "com/ptl/client " + clientData.getId() + " by " + clientData.getLogin() + " login success :)"});
                                }
                            }
                            else {
                                params.put(LoginInvalid.toString(), "Wrong password!");
                                data = new AitPackageData(clientData, LoginInvalid, params);
                                aitReceiverSender.cryptMsgAndSend(gson.toJson(data));

                                AitLogger.getInstance().logToConsole(new Object[] { "com/ptl/client " + data.getClient().getLogin() + " give wrong password!" });
                            }
                        }
                        break;
                    case RejestractionRespons:
                        createAccount(data);
                        break;
                }
            }
        }
    }

    private boolean validLogin(String login) throws Exception {
        if(Objects.requireNonNull(AitLocalDB.getInstance()).validLogin(login)) {
            return true;
        } else {
            Gson gson = new Gson();
            Map<String, String> params = new HashMap<>();
            params.put(RejestractionRequest.toString(), "You must reqister!");
            AitPackageData data = new AitPackageData(clientData, RejestractionRequest, params);
            aitReceiverSender.cryptMsgAndSend(gson.toJson(data));

            AitLogger.getInstance().logToConsole(new Object[] { "com/ptl/client " + data.getClient().getLogin() + " must reqister!" });

            return false;
        }
    }

    private boolean validPassword(String login, String password) {
        return Objects.requireNonNull(AitLocalDB.getInstance()).validPass(login, password);
    }

    private void createAccount(AitPackageData data) throws Exception {
        AitClientData client = data.getClient();
        Gson gson = new Gson();
        Map<String, String> params = new HashMap<>();
        if(Objects.requireNonNull(AitLocalDB.getInstance()).canRegister(client.getLogin())){
            AitLocalDB.getInstance().registerClient(client);
            clientData = client;

            params.put(RejestractionSuc.toString(), "Accout was created success!");
            data = new AitPackageData(clientData, RejestractionSuc, params);
        }
        else {
            params.put(RejestractionInvalid.toString(), "Login is already used");
            data = new AitPackageData(clientData, RejestractionInvalid, params);
        }
        aitReceiverSender.cryptMsgAndSend(gson.toJson(data));
    }

    private void sendTo(AitClientHandler mc, Map.Entry<String,String> entry) throws Exception {
        Map<String, String> params = new HashMap<>();
        params.put(entry.getKey(), entry.getValue());
        send(mc, params);
    }

    private void send(AitClientHandler mc, Map<String, String> params) throws Exception {
        AitPackageData data = new AitPackageData(clientData, SendFrom, params);
        Gson gson = new Gson();
        mc.aitReceiverSender.cryptMsgAndSend(gson.toJson(data));
    }

    private void close() {
        try {
            socket.close();
            aitReceiverSender.close();
        }
        catch (Exception e) {
            try {
                AitLogger.getInstance().logToFile(e.getStackTrace(), e.toString());
            } catch (Exception exc) {
                e.printStackTrace();
                exc.printStackTrace();
            }
        }
    }
}
