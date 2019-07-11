package com.hbm.models;

import com.hbm.generics.AitGenericModel;
import com.hbm.models.entities.AitUserHostEntity;
import org.hibernate.Session;

import java.io.Serializable;
import java.sql.Date;

public class AitUserHost extends AitGenericModel<AitUserHostEntity> implements Serializable {
    public AitUserHost(Session session) {
        super(session);
    }
    public AitUserHost(Session session, AitUserHostEntity entity) {
        super(session);
        this.entity = entity;
    }

    public int getId() { return entity.getId(); }

    public void setId(int id) { entity.setId(id); }

    public AitAccount getAccount() { return new AitAccount(getSession(), entity.getAccount()); }

    public void setAccount(AitAccount account) { entity.setAccount(account.getEntity());}

    public String getHostName() { return entity.getHostName(); }

    public void setHostName(String hostName) { entity.setHostName(hostName); }

    public boolean isActive() { return entity.isActive(); }

    public void setActive(boolean active) { entity.setActive(active); }

    public boolean isActualLoggedIn() { return entity.isActualLoggedIn(); }

    public void setActualLoggedIn(boolean acctualLoggedIn) { entity.setActualLoggedIn(acctualLoggedIn); }

    public Date getCreateDate() { return entity.getCreateDate(); }

    public void setCreateDate(Date createDate) { entity.setCreateDate(createDate); }
}
