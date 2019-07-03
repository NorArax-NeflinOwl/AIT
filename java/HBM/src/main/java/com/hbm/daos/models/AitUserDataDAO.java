package com.hbm.daos.models;

import com.hbm.entities.AitUserDataEntity;
import com.hbm.generics.AitGenericDAO;
import com.hbm.interfaces.AitUserDataDAOInterface;
import com.hbm.models.AitUserData;
import org.hibernate.Session;
import org.hibernate.query.Query;

import java.util.ArrayList;
import java.util.List;
public class AitUserDataDAO extends AitGenericDAO<AitUserDataEntity, Integer> implements AitUserDataDAOInterface {

    public AitUserDataDAO(Session session) {
        super(session);
    }

    @Override
    public AitUserData findUserDataById(int id) {
        logger.info("opening: AitUserDataDAO.findUserDataById(int)");

        logger.info("exiting: AitUserDataDAO.findUserDataById(int)");
        return new AitUserData(getSession(), findById(id));
    }

    @Override
    public List<AitUserData> findUserDataByNick(String nick) {
        logger.info("opening: AitUserDataDAO.findUserDataByNick(String)");

        Query query = getSession().createQuery("from usersdata where udt_nick = :nick", AitUserDataEntity.class);
        query.setParameter("nick", nick);

        List<AitUserData> result = new ArrayList<>();
        List<AitUserDataEntity> usersdata = query.list();
        if(usersdata != null && usersdata.size() > 0)
        {
            for (AitUserDataEntity entity : usersdata) {
                result.add(new AitUserData(getSession(), entity));
            }
        }

        logger.info("exiting: AitUserDataDAO.findUserDataByNick(String)");
        return result;
    }

    @Override
    public List<AitUserData> findUserDataByFirstName(String firstName) {
        logger.info("opening: AitUserDataDAO.findUserDataByFirstName(String)");

        Query query = getSession().createQuery("from usersdata where udt_firstname = :firstName", AitUserDataEntity.class);
        query.setParameter("firstName", firstName);

        List<AitUserData> result = new ArrayList<>();
        List<AitUserDataEntity> usersdata = query.list();
        if(usersdata != null && usersdata.size() > 0)
        {
            for (AitUserDataEntity entity : usersdata) {
                result.add(new AitUserData(getSession(), entity));
            }
        }

        logger.info("exiting: AitUserDataDAO.findUserDataByFirstName(String)");
        return result;
    }

    @Override
    public List<AitUserData> findUserDataByMiddleName(String middleName) {
        logger.info("opening: AitUserDataDAO.findUserDataByMiddleName(String)");

        Query query = getSession().createQuery("from usersdata where udt_middlename = :middleName", AitUserDataEntity.class);
        query.setParameter("middleName", middleName);

        List<AitUserData> result = new ArrayList<>();
        List<AitUserDataEntity> usersdata = query.list();
        if(usersdata != null && usersdata.size() > 0)
        {
            for (AitUserDataEntity entity : usersdata) {
                result.add(new AitUserData(getSession(), entity));
            }
        }

        logger.info("exiting: AitUserDataDAO.findUserDataByMiddleName(String)");
        return result;
    }

    @Override
    public List<AitUserData> findUserDataByLastName(String lastName) {
        logger.info("opening: AitUserDataDAO.findUserDataByLastName(String)");

        Query query = getSession().createQuery("from usersdata where udt_lastname = :lastName", AitUserDataEntity.class);
        query.setParameter("lastName", lastName);

        List<AitUserData> result = new ArrayList<>();
        List<AitUserDataEntity> usersdata = query.list();
        if(usersdata != null && usersdata.size() > 0)
        {
            for (AitUserDataEntity entity : usersdata) {
                result.add(new AitUserData(getSession(), entity));
            }
        }

        logger.info("exiting: AitUserDataDAO.findUserDataByLastName(String)");
        return result;
    }

    @Override
    public List<AitUserData> findAllUserData() {
        logger.info("opening: AitUserDataDAO.findAllUserData()");

        Query query = getSession().createQuery("from usersdata", AitUserDataEntity.class);

        List<AitUserData> result = new ArrayList<>();
        List<AitUserDataEntity> usersdata = query.list();
        if(usersdata != null && usersdata.size() > 0)
        {
            for (AitUserDataEntity entity : usersdata) {
                result.add(new AitUserData(getSession(), entity));
            }
        }

        logger.info("exiting: AitUserDataDAO.findAllUserData()");
        return result;
    }

    @Override
    public AitUserData findUserDataByAccountId(int id) {
        logger.info("opening: AitUserDataDAO.findUserDataByAccountId(int)");

        Query query = getSession().createQuery("from usersdata where udt_accid = :id", AitUserDataEntity.class);
        query.setParameter("id", id);

        List<AitUserData> result = new ArrayList<>();
        List<AitUserDataEntity> usersdata = query.list();
        if(usersdata != null && usersdata.size() > 0)
        {
            for (AitUserDataEntity entity : usersdata) {
                result.add(new AitUserData(getSession(), entity));
            }
        }

        logger.info("exiting: AitUserDataDAO.findUserDataByAccountId(int)");
        return result.isEmpty() ?  null : result.get(0);
    }
}
