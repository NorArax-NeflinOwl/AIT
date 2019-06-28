import java.util.*;
import javax.mail.*;
import javax.mail.internet.*;

public class MailSender {

    private static final String aitMail = "ait.wms.nano@gmail.com";
    private static final String aitPassword = "#_@rnn0I";

    public static void main(String [] args) {
        Properties props = System.getProperties();
        props.put("mail.smtp.user", aitMail);
        props.put("mail.smtp.host", "smtp.gmail.com");
        props.put("mail.smtp.port", "465");
        props.put("mail.smtp.auth", "true");
        props.put("mail.smtp.socketFactory.port", "465");
        props.put("mail.smtp.socketFactory.class", "javax.net.ssl.SSLSocketFactory");
        try{
            Session session = Session.getInstance(props,
                    new javax.mail.Authenticator() {
                        protected PasswordAuthentication getPasswordAuthentication() {
                            return new PasswordAuthentication(aitMail, aitPassword);
                        }
                    });

            Message msg = new MimeMessage(session);
            msg.setFrom(new InternetAddress(aitMail));
            msg.addRecipient(Message.RecipientType.TO, new InternetAddress("ppudi7368@gmail.com"));

            msg.setSubject("Hello");
            msg.setText("How are you");

            msg.setSentDate(new Date());
            Transport.send(msg);

            System.out.println("Message sent.");
        }catch (MessagingException e){
            e.printStackTrace();
        }
    }
}
