package com.hbm;

import com.hbm.entities.AccountEntity;
import com.hbm.hibernate.HibernateUtil;
import org.hibernate.Session;
import org.hibernate.query.Query;

import java.util.Date;
import java.util.List;
import java.util.Scanner;

public class AppHBM {

    private static Session sessionObj;

    private static Session getSession(boolean createIfNotExists) {
        if(sessionObj == null && createIfNotExists || sessionObj != null && !sessionObj.isOpen()) {
            sessionObj = HibernateUtil.getInstance().getSessionFactory().openSession();
        }
        return sessionObj;
    }

    public static void main(String[] args) {
        while(true) {
            main();
        }
    }

    private static void main() {
        System.out.println("\n\n.......Hibernate Maven Example.......\n.......Choose example medhod:........" +
                "\n[Insert] - insert 5 examples users to db" +
                "\n[SELECT] - select all users from db and print" +
                "\n[DELETE] - delete one row from db by id" +
                "\n[UPDATE] - udpate one row from db by id and new values" +
                "\n[CLEAR] - clear table" +
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
                System.out.println("Choose row: 'user_name' by [n] or 'created_by' by [c] or 'created_date' by [d]");
                String row = scn.nextLine();

                System.out.print("[new value] = ");
                String newValue = scn.nextLine();
                updateOneRowInDB(id, row, newValue);
                break;
            case "c":
                clearTable();
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
                AccountEntity userObj = new AccountEntity();
                userObj.setLogin("Test acccount " + i);
                userObj.setMail("Administrator@gov.com");
                userObj.setCreateDate(new Date());

                getSession(true).save(userObj);
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
            List<AccountEntity> users = getSession(true).createQuery("from accounts", AccountEntity.class).list();

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

            Query query = getSession(true).createQuery("delete accounts users where acc_id = :id");
            query.setParameter("id", id);
            int result = query.executeUpdate();

            if(result != Integer.parseInt(id))
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

            String q = "";
            switch (row.toLowerCase()) {
                case "n":
                    q = "update accounts set user_name = :newValue where acc_id = :id";
                    break;
                case "c":
                    q = "update accounts set created_by = :newValue where acc_id = :id";
                    break;
                case "d":
                    q = "update accounts set created_date = :newValue where acc_id = :id";
                    break;
            }

            Query query = getSession(true).createQuery(q);

            query.setParameter("id", id);
            query.setParameter("newValue", newValue);

            int result = query.executeUpdate();
            if (result != Integer.parseInt(id))
                System.out.println(".......Update successfully.......");
            else
                System.out.println(".......No updated!!!.......");

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

    private static void clearTable() {
        try {
            getSession(true).beginTransaction();
            Query query = getSession(true).createQuery("delete from users");
            query.executeUpdate();
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
}