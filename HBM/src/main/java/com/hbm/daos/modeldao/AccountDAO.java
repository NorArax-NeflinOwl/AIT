package com.hbm.daos.modeldao;

import com.hbm.daos.GenericDAO;
import com.hbm.daos.imodeldao.IAccountDAO;
import com.hbm.datamodels.Account;
import com.hbm.entities.AccountEntity;
import org.hibernate.Session;

public class AccountDAO extends GenericDAO<AccountEntity, Long> implements IAccountDAO {
    public AccountDAO(Session session) {
        super(session);
    }

    @Override
    public Account findAccountById(int id) {
        return null;
    }
}
