package com.hbm.models.entitiecovers;

import com.hbm.daos.AitDAOFactory;
import com.hbm.generics.AitGenericModel;
import com.hbm.models.entities.AitAccountEntity;
import org.hibernate.Session;

import java.io.Serializable;
import java.util.Date;
import java.util.List;

public class AitAccount extends AitGenericModel<AitAccountEntity> implements Serializable {

    private AitUserData userData;
    private List<AitNote> notes;
    private List<AitUserHost> userHosts;

    public AitAccount(Session session) {
        super(session);
        entity = new AitAccountEntity();
    }

    public AitAccount(Session session, AitAccountEntity entity) {
        super(session);
        this.entity = entity;
    }

    public int getID() {
        return entity.getId();
    }

    public void setID(long id) {
        entity.setId((int) id);
    }

    public AitUserData getUserData() {
        if(userData == null) {
            userData =  new AitDAOFactory(getSession()).getUserDataDAO().findUserDataByAccountId(entity.getId());
        }
        return userData;
    }

    public void setUserData(AitUserData data) {
        userData = data;
    }

    public List<AitNote> getNotes() {
        if(notes == null) {
            notes =  new AitDAOFactory(getSession()).getNotesDAO().findNotesByAccountId(entity.getId());
        }
        return notes;
    }

    public void setNotes(List<AitNote> data) {
        notes = data;
    }

    public List<AitUserHost> getUserHosts() {
        if(userHosts == null) {
            userHosts =  new AitDAOFactory(getSession()).getUserHostDAO().findUserHostsByAccountId(entity.getId());
        }
        return userHosts;
    }

    public void setUserHosts(List<AitUserHost> data) {
        userHosts = data;
    }

    public String getLogin() {
        return entity.getLogin();
    }

    public void setLogin(String login) {
        entity.setLogin(login);
    }

    public String getPassword() {
        return entity.getPassword();
    }

    public void setPassword(String password) {
        entity.setPassword(password);
    }

    public String getEmail() {
        return entity.getEmail();
    }

    public void setEmail(String email) {
        entity.setEmail(email);
    }

    public boolean IsActive() { return entity.isActive(); }

    public void setActive(boolean isActive) {
        entity.setActive(isActive);
    }

    public Date getCreateDate() {
        return entity.getCreateDate();
    }

    public void setCreateDate(Date createDate) {
        entity.setCreateDate(createDate);
    }

    public Date getLastUpdate() {
        return entity.getLastUpdate();
    }

    public void setLastUpdate(Date lastUpdate) {
        entity.setLastUpdate(lastUpdate);
    }

}
