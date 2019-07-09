package com.hbm.models;

import com.hbm.generics.AitGenericModel;
import com.hbm.models.entities.AitSettingEntity;
import org.hibernate.Session;

import java.io.Serializable;
import java.sql.Date;

public class AitSetting extends AitGenericModel<AitSettingEntity> implements Serializable {

    public AitSetting(Session session) {
        super(session);
    }

    public AitSetting(Session session, AitSettingEntity entity) {
        super(session);
        this.entity = entity;
    }

    public int getID() { return entity.getId(); }

    public void setID(long id) { this.entity.setId((int)id);}

    public String getName() {
        return entity.getName();
    }

    public void setName(String login) {
        entity.setName(login);
    }

    public int getValue() {
        return entity.getValue();
    }

    public void setValue(int value) {
        entity.setValue(value);
    }

    public Date getLastUpdate() {
        return entity.getLastUpdate();
    }

    public void setLastUpdate(Date lastUpdate) {
        entity.setLastUpdate(lastUpdate);
    }
}
