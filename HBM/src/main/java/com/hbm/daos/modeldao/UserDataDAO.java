package com.hbm.daos.modeldao;

import com.hbm.daos.GenericDAO;
import com.hbm.daos.imodeldao.IUserDataDAO;
import com.hbm.datamodels.UserData;
import com.hbm.entities.UserDataEntity;
import org.hibernate.Session;

import java.util.List;

public class UserDataDAO extends GenericDAO<UserDataEntity, Long> implements IUserDataDAO {

    public UserDataDAO(Session session) {
        super(session);
    }

    @Override
    public UserData findUserDataById(int id) {

        List<UserDataEntity> usersdata = getSession().createQuery("from usersdata", UserDataEntity.class).list();
        if(usersdata != null && usersdata.size() > 0)
        {
            return new UserData(usersdata.get(0));
        }

        return null;
    }
}
