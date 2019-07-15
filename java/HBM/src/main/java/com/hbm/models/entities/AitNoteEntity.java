package com.hbm.models.entities;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.OneToOne;
import java.io.Serializable;
import java.util.Date;

@Entity(name = "notes")
public class AitNoteEntity implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "nts_id")
    private int id;

    @OneToOne(cascade = CascadeType.ALL)
    @JoinColumn(name="nts_accid")
    private AitAccountEntity account;

    @Column(name = "nts_title")
    private String name;

    @Column(name = "nts_content")
    private boolean isActive;

    @Column(name = "uhs_create")
    private Date createDate;

    @Column(name = "nts_lastupdate")
    private Date lastUpdate;

    public AitNoteEntity() {}

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

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public boolean isActive() {
        return isActive;
    }

    public void setActive(boolean active) {
        isActive = active;
    }

    public Date getCreateDate() {
        return createDate;
    }

    public void setCreateDate(Date createDate) {
        this.createDate = createDate;
    }

    public Date getLastUpdate() {
        return lastUpdate;
    }

    public void setLastUpdate(Date lastUpdate) {
        this.lastUpdate = lastUpdate;
    }
}
