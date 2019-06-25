package com.arno.daos.modeldao;

import com.arno.daos.GenericDAO;
import com.arno.daos.imodeldao.IUserDataDAO;
import com.arno.datamodels.UserData;
import com.arno.entities.UserDataEntity;
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
