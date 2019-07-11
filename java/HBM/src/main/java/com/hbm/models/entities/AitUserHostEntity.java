package com.hbm.models.entities;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import java.io.Serializable;
import java.sql.Date;

@Entity(name = "usershosts")
public class AitUserHostEntity implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "uhs_id")
    private int id;

    @ManyToOne(cascade = CascadeType.ALL)
    @JoinColumn(name="uhs_accid")
    private AitAccountEntity account;

    @Column(name = "uhs_hostname")
    private String hostName;

    @Column(name = "uhs_active")
    private boolean isActive;

    @Column(name = "uhs_actualloggedin")
    private boolean isActualLoggedIn;

    @Column(name = "uhs_create")
    private Date createDate;

    public AitUserHostEntity() {}

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public AitAccountEntity getAccount() {
        return account;
    }

    public void setAccount(AitAccountEntity account) {
        this.account = account;
    }

    public String getHostName() {
        return hostName;
    }

    public void setHostName(String hostName) {
        this.hostName = hostName;
    }

    public boolean isActive() {
        return isActive;
    }

    public void setActive(boolean active) {
        isActive = active;
    }

    public boolean isActualLoggedIn() {
        return isActualLoggedIn;
    }

    public void setActualLoggedIn(boolean actualLoggedIn) {
        isActualLoggedIn = actualLoggedIn;
    }

    public Date getCreateDate() {
        return createDate;
    }

    public void setCreateDate(Date createDate) {
        this.createDate = createDate;
    }
}
