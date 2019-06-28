package com.hbm.entities;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.OneToOne;
import java.util.Date;

@Entity(name = "usersdata")
public class UserDataEntity {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "udt_id")
    private int id;

    @OneToOne(cascade = CascadeType.ALL)
    @JoinColumn(name="udt_accid")
    private AccountEntity account;

    @Column(name = "udt_nick")
    private String nick;

    @Column(name = "udt_firstname")
    private String firstName;

    @Column(name = "udt_middlename")
    private String middleName;

    @Column(name = "udt_lastname")
    private String lastName;

    @Column(name = "udt_birthdate")
    private Date birthdate;

    @Column(name = "udt_lastupdate")
    private Date lastUpdate;

    public UserDataEntity() {}

    public UserDataEntity(int id, AccountEntity account, String nick, String firstName, String middleName, String lastName, Date birthdate, Date lastUpdate) {
        this.id = id;
        this.account = account;
        this.nick = nick;
        this.firstName = firstName;
        this.middleName = middleName;
        this.lastName = lastName;
        this.birthdate = birthdate;
        this.lastUpdate = lastUpdate;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public AccountEntity getAccount() {
        return account;
    }

    public void setAccount(AccountEntity account) {
        this.account = account;
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

    public Date getBirthdate() {
        return birthdate;
    }

    public void setBirthdate(Date birthdate) {
        this.birthdate = birthdate;
    }

    public Date getLastUpdate() {
        return lastUpdate;
    }

    public void setLastUpdate(Date lastUpdate) {
        this.lastUpdate = lastUpdate;
    }

}
