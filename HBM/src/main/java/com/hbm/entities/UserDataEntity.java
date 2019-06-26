package com.hbm.entities;

import javax.persistence.*;
import java.util.Date;

@Entity(name = "usersdata")
public class UserDataEntity {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "ust_id")
    private int id;

    @Column(name = "ust_accid")
    private int accountId;

    @Column(name = "ust_nick")
    private String nick;

    @Column(name = "ust_firstname")
    private String firstName;

    @Column(name = "ust_middlename")
    private String middleName;

    @Column(name = "ust_lastname")
    private String lastName;

    @Column(name = "ust_lastupdate")
    private Date lastUpdate;


    public UserDataEntity() {}

    public UserDataEntity(int id, int accountId, String nick, String firstName, String middleName, String lastName, Date lastUpdate) {
        this.id = id;
        this.accountId = accountId;
        this.nick = nick;
        this.firstName = firstName;
        this.middleName = middleName;
        this.lastName = lastName;
        this.lastUpdate = lastUpdate;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getAccountId() {
        return accountId;
    }

    public void setAccountId(int accountId) {
        this.accountId = accountId;
    }

    public String getNick() {
        return nick;
    }

    public void setNick(String nick) {
        this.nick = nick;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getMiddleName() {
        return middleName;
    }

    public void setMiddleName(String middleName) {
        this.middleName = middleName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public Date getLastUpdate() {
        return lastUpdate;
    }

    public void setLastUpdate(Date lastUpdate) {
        this.lastUpdate = lastUpdate;
    }
}
