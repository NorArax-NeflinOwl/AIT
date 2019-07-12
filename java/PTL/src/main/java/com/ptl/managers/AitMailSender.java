package com.ptl.managers;

import javax.mail.Message;
import javax.mail.MessagingException;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import java.util.Date;
import java.util.Properties;

public class AitMailSender {
    private final String aitMail = "ait.wms.nano@gmail.com";
    private final String aitPassword = "NIJdkE+HyTr75zeR9vepuw==";
    private final String aitDefaltSubject = "New User Activation Request";
    private final String aitDefaltContent = String.format("Activation Link: %s", aitMail);

    private Properties props;

    public AitMailSender() {
        props = System.getProperties();
        props.put("mail.smtp.user", aitMail);
        props.put("mail.smtp.host", "smtp.gmail.com");
        props.put("mail.smtp.port", "465");
        props.put("mail.smtp.auth", "true");
        props.put("mail.smtp.socketFactory.port", "465");
        props.put("mail.smtp.socketFactory.class", "javax.net.ssl.SSLSocketFactory");
    }

    public void sendTest() {
        sentMailFromTo(aitMail, aitPassword, aitMail, aitDefaltSubject, aitDefaltContent);
    }

    public void sentActivationLinkTo(String adress) {
        sentMailFromTo(aitMail, aitPassword, adress, aitDefaltSubject, aitDefaltContent);
    }

    public void sentMailTo(String adress, String subject, String content) {
        sentMailFromTo(aitMail, aitPassword, adress, subject, content);
    }

    public void sentMailFromTo(String from, String cryptPass, String to, String subject, String content) {
        try{
            Session session = Session.getInstance(props,
                    new javax.mail.Authenticator() {
                        protected javax.mail.PasswordAuthentication getPasswordAuthentication() {
                            return new javax.mail.PasswordAuthentication(from, AitCrypter.decrypt(cryptPass));
                        }
                    });

            Message msg = new MimeMessage(session);
            msg.setFrom(new InternetAddress(aitMail));
            msg.addRecipient(Message.RecipientType.TO, new InternetAddress(to));

            msg.setSubject(subject);
            msg.setText(content);

            msg.setSentDate(new Date());
            Transport.send(msg);

            System.out.println("Message sent.");
        }catch (MessagingException e){
            e.printStackTrace();
        }
    }
}
