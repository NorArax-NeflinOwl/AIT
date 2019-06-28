package com.hbm.daos.modeldao;

import com.hbm.daos.GenericDAO;
import com.hbm.daos.imodeldao.IUserDataDAO;
import com.hbm.datamodels.models.UserData;
import com.hbm.entities.UserDataEntity;
import org.hibernate.Session;
import org.hibernate.query.Query;

import java.util.ArrayList;
import java.util.List;
public class UserDataDAO extends GenericDAO<UserDataEntity, Integer> implements IUserDataDAO {

    public UserDataDAO(Session session) {
        super(session);
    }

    @Override
    public UserData findUserDataById(int id) {
        logger.info("opening: UserDataDAO.findUserDataById(int)");

        logger.info("exiting: UserDataDAO.findUserDataById(int)");
        return new UserData(findById(id));
    }

    @Override
    public List<UserData> findUserDataByNick(String nick) {
        logger.info("opening: UserDataDAO.findUserDataByNick(String)");

        Query query = getSession().createQuery("from usersdata where udt_nick = :nick", UserDataEntity.class);
        query.setParameter("nick", nick);

        List<UserData> result = new ArrayList<>();
        List<UserDataEntity> usersdata = query.list();
        if(usersdata != null && usersdata.size() > 0)
        {
            for (UserDataEntity entity : usersdata) {
                result.add(new UserData(entity));
            }
        }

        logger.info("exiting: UserDataDAO.findUserDataByNick(String)");
        return result;
    }

    @Override
    public List<UserData> findUserDataByFirstName(String firstName) {
        logger.info("opening: UserDataDAO.findUserDataByFirstName(String)");

        Query query = getSession().createQuery("from usersdata where udt_firstname = :firstName", UserDataEntity.class);
        query.setParameter("firstName", firstName);

        List<UserData> result = new ArrayList<>();
        List<UserDataEntity> usersdata = query.list();
        if(usersdata != null && usersdata.size() > 0)
        {
            for (UserDataEntity entity : usersdata) {
                result.add(new UserData(entity));
            }
        }

        logger.info("exiting: UserDataDAO.findUserDataByFirstName(String)");
        return result;
    }

    @Override
    public List<UserData> findUserDataByMiddleName(String middleName) {
        logger.info("opening: UserDataDAO.findUserDataByMiddleName(String)");

        Query query = getSession().createQuery("from usersdata where udt_middlename = :middleName", UserDataEntity.class);
        query.setParameter("middleName", middleName);

        List<UserData> result = new ArrayList<>();
        List<UserDataEntity> usersdata = query.list();
        if(usersdata != null && usersdata.size() > 0)
        {
            for (UserDataEntity entity : usersdata) {
                result.add(new UserData(entity));
            }
        }

        logger.info("exiting: UserDataDAO.findUserDataByMiddleName(String)");
        return result;
    }

    @Override
    public List<UserData> findUserDataByLastName(String lastName) {
        logger.info("opening: UserDataDAO.findUserDataByLastName(String)");

        Query query = getSession().createQuery("from usersdata where udt_lastname = :lastName", UserDataEntity.class);
        query.setParameter("lastName", lastName);

        List<UserData> result = new ArrayList<>();
        List<UserDataEntity> usersdata = query.list();
        if(usersdata != null && usersdata.size() > 0)
        {
            for (UserDataEntity entity : usersdata) {
                result.add(new UserData(entity));
            }
        }

        logger.info("exiting: UserDataDAO.findUserDataByLastName(String)");
        return result;
    }

    @Override
    public List<UserData> findAllUserData() {
        logger.info("opening: UserDataDAO.findAllUserData()");

        Query query = getSession().createQuery("from usersdata", UserDataEntity.class);

        List<UserData> result = new ArrayList<>();
        List<UserDataEntity> usersdata = query.list();
        if(usersdata != null && usersdata.size() > 0)
        {
            for (UserDataEntity entity : usersdata) {
                result.add(new UserData(entity));
            }
        }

        logger.info("exiting: UserDataDAO.findAllUserData()");
        return result;
    }
}
