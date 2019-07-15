package com.hbm.daos.models;

import com.hbm.generics.AitGenericDAO;
import com.hbm.interfaces.AitSettingDAOInterface;
import com.hbm.managers.AitLogger;
import com.hbm.models.entitiecovers.AitSetting;
import com.hbm.models.entities.AitSettingEntity;
import org.hibernate.Session;
import org.hibernate.query.Query;

import java.util.List;

public class AitSettingsDAO extends AitGenericDAO<AitSettingEntity, Integer> implements AitSettingDAOInterface {

    public AitSettingsDAO(Session session) {
        super(session);
    }

    @Override
    public AitSetting findSettingById(int id) {
        AitLogger.getInstance().logInfoToFile("opening: AitAccountDAO.findAccountById(Integer)");
        AitLogger.getInstance().logInfoToFile("exiting: AitAccountDAO.findAccountById(Integer)");
        return new AitSetting(getSession(), findById(id));
    }

    @Override
    public AitSetting findSettingByName(String name) {
        AitLogger.getInstance().logInfoToFile("opening: AitAccountDAO.findAccountByPass(String)");

        Query query = getSession().createQuery("from settings where set_name = :name", AitSettingEntity.class);
        query.setParameter("name", name);

        List<AitSettingEntity> accounts = query.list();
        if(accounts != null && accounts.size() > 0)
        {
            AitLogger.getInstance().logInfoToFile("exiting: AitAccountDAO.findAccountByPass(String)");
            return new AitSetting(getSession(), accounts.get(0));
        }

        AitLogger.getInstance().logInfoToFile("exiting: AitAccountDAO.findAccountByPass(String)");
        return null;
    }
}
