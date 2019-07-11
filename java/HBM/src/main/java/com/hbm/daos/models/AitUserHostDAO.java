package com.hbm.daos.models;

import com.hbm.generics.AitGenericDAO;
import com.hbm.interfaces.AitUserHostDAOInterface;
import com.hbm.models.AitUserHost;
import com.hbm.models.entities.AitUserHostEntity;
import org.hibernate.Session;
import org.hibernate.query.Query;

import java.util.ArrayList;
import java.util.List;

public class AitUserHostDAO extends AitGenericDAO<AitUserHostEntity, Integer> implements AitUserHostDAOInterface {

    public AitUserHostDAO(Session session) {
        super(session);
    }

    @Override
    public List<AitUserHost> findUserHostsByAccountId(int id) {
        logger.info("opening: AitUserHostDAO.findUserHostsByAccountId(int)");

        Query query = getSession().createQuery("from usershosts where uhs_accid = :id", AitUserHostEntity.class);
        query.setParameter("id", id);

        List<AitUserHost> result = new ArrayList<>();
        List<AitUserHostEntity> usersdata = query.list();
        if(usersdata != null && usersdata.size() > 0)
        {
            for (AitUserHostEntity entity : usersdata) {
                result.add(new AitUserHost(getSession(), entity));
            }
        }

        logger.info("exiting: AitUserHostDAO.findUserHostsByAccountId(int)");
        return result;
    }

    @Override
    public List<AitUserHost> findUserHostsByHostName(String hostName) {
        logger.info("opening: AitUserHostDAO.findUserHostsByHostName(String)");

        Query query = getSession().createQuery("from usershosts where uhs_hostname = :hostname", AitUserHostEntity.class);
        query.setParameter("hostname", hostName);

        List<AitUserHost> result = new ArrayList<>();
        List<AitUserHostEntity> usersdata = query.list();
        if(usersdata != null && usersdata.size() > 0)
        {
            for (AitUserHostEntity entity : usersdata) {
                result.add(new AitUserHost(getSession(), entity));
            }
        }

        logger.info("exiting: AitUserHostDAO.findUserHostsByHostName(String)");
        return result;
    }
}
