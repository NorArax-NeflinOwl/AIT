package com.hbm;

import com.hbm.daos.AitDAOFactory;
import com.hbm.daos.models.AitAccountDAO;
import com.hbm.models.AitAccount;
import com.hbm.models.AitUserData;
import com.hbm.hibernate.AitHibernateUtil;
import com.ptl.managers.AitCrypter;
import org.hibernate.Session;

import java.util.Date;
import java.util.List;
import java.util.Scanner;

public class AppHBM {

    private static Session sessionObj;

    private static Session getSession(boolean createIfNotExists) {
        if(sessionObj == null && createIfNotExists || sessionObj != null && !sessionObj.isOpen()) {
            sessionObj = AitHibernateUtil.getInstance().getSessionFactory().openSession();
        }
        return sessionObj;
    }

    public static void main(String[] args) {
        while(true) {
            main();
        }
    }

    private static void main() {
        System.out.println(".......Hibernate Maven Example.......\n.......Choose example medhod:........" +
                "\n[Insert] - insert 5 examples users to db" +
                "\n[SELECT] - select all users from db and print" +
                "\n[DELETE] - delete one row from db by id" +
                "\n[UPDATE] - udpate one row from db by id and new values" +
                "\nSelect [i] or [s] or [d] or [u] or [c]...");

        String id;
        Scanner scn = new Scanner(System.in);
        switch (scn.nextLine().toLowerCase()) {
            case "i":
                insert5ExampleRowToDB();
                break;
            case "s":
                printAllUsersFromDB();
                break;
            case "d":
                System.out.print("[id] = ");
                id = scn.nextLine();
                deleteOneRowFromDB(id);
                break;
            case "u":
                System.out.print("[id] = ");
                id = scn.nextLine();
                System.out.println("Choose row: 'login' by [l] or 'pass' by [p] or 'email' by [e] or 'isActive' by [a] set 't'\n");
                String row = scn.nextLine();

                System.out.print("[new value] = ");
                String newValue = scn.nextLine();
                updateOneRowInDB(id, row, newValue);
                break;
            default:
                System.out.println("You not select [i] or [s] or [d] or [u] or [e]...\n RELOAD!");
                break;
        }
    }

    private static void insert5ExampleRowToDB() {
        try {
            getSession(true).beginTransaction();

            for(int i = 101; i <= 105; i++) {
                AitAccount userObj = new AitAccount(getSession(true));
                userObj.setLogin("Test acccount " + i);
                userObj.setPassword(AitCrypter.generateMD5Hash(userObj.getLogin()));
                userObj.setCreateDate(new Date());

                userObj.saveOrUpdate();
            }
            System.out.println("\n.......Records Saved Successfully To The Database.......\n");

            // Committing The Transactions To The Database
            getSession(true).getTransaction().commit();
        } catch(Exception sqlException) {
            if(null != getSession(false).getTransaction()) {
                System.out.println("\n.......Transaction Is Being Rolled Back.......");
                getSession(false).getTransaction().rollback();
            }
            sqlException.printStackTrace();
        } finally {
            if(getSession(false) != null) {
                getSession(false).close();
            }
        }
    }

    private static void printAllUsersFromDB() {
        try {
            List<AitAccount> users = new AitDAOFactory(getSession(true)).getAccountDAO().findAllAccount();
            List<AitUserData> data = new AitDAOFactory(getSession(true)).getUserDataDAO().findAllUserData();

            System.out.println();
            if(users.isEmpty())
                System.out.println("TABLE IS EMPTY!!!");
            else
                users.forEach(System.out::println);

        }catch (Exception sqlException) {
            sqlException.printStackTrace();
        } finally {
            if(getSession(false) != null) {
                getSession(false).close();
            }
        }
    }

    private static void deleteOneRowFromDB(String id) {
        try {
            getSession(true).beginTransaction();

            Integer lid = Integer.parseInt(id);
            AitAccountDAO doa = new AitDAOFactory(getSession(true)).getAccountDAO();

            if(doa.deleteById(lid))
                System.out.println(".......Delete successfully.......");
            else
                System.out.println(".......No deleted!!!.......");

            getSession(true).getTransaction().commit();
        }catch (Exception sqlException) {
            if(getSession(false) != null)
                getSession(false).getTransaction().rollback();
            sqlException.printStackTrace();
        } finally {
            if(getSession(false) != null) {
                getSession(false).close();
            }
        }
    }

    private static void updateOneRowInDB(String id, String row, String newValue) {
        try {
            getSession(true).beginTransaction();

            AitAccountDAO dao = new AitDAOFactory(getSession(true)).getAccountDAO();
            AitAccount acc = dao.findAccountById(Integer.parseInt(id));

            switch (row.toLowerCase()) {
                case "l":
                    acc.setLogin(newValue);
                    break;
                case "p":
                    acc.setPassword(AitCrypter.generateMD5Hash(newValue));
                    break;
                case "e":
                    acc.setEmail(newValue);
                    break;
                case "a":
                    acc.setActive("t".equals(newValue.toLowerCase()));
                    break;
            }
            acc.saveOrUpdate();
            System.out.println(".......Update successfully.......");

            getSession(true).getTransaction().commit();
        }catch (Exception sqlException) {
            System.out.println(".......No updated!!!.......");
            if(getSession(false) != null)
                getSession(false).getTransaction().rollback();
            sqlException.printStackTrace();
        } finally {
            if(getSession(false) != null) {
                getSession(false).close();
            }
        }
    }
}