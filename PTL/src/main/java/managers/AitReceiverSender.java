package managers;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.Socket;

public class AitReceiverSender {
    private DataInputStream dis;
    private DataOutputStream dos;

    public AitReceiverSender(Socket s) {
        try {
            this.dis = new DataInputStream(s.getInputStream());
            this.dos = new DataOutputStream(s.getOutputStream());
        } catch (IOException e) {
            try {
                AitLogger.getInstance().logToFile(e.getStackTrace(), e.toString());
            } catch (Exception exc) {
                e.printStackTrace();
                exc.printStackTrace();
            }
        }
    }

    public AitReceiverSender(DataInputStream dis, DataOutputStream dos){
        this.dis = dis;
        this.dos = dos;
    }

    public void cryptMsgAndSend(String msg) throws Exception {
        String cryptMsg = AitCrypter.encrypt(msg);
        dos.writeUTF(cryptMsg);
    }

    public String decryptMsg() throws Exception {
        String cryptMsg = dis.readUTF();
        System.out.println(cryptMsg);

        return AitCrypter.decrypt(cryptMsg);
    }

    public void close() throws IOException {
        dis.close();
        dos.close();
    }
}
