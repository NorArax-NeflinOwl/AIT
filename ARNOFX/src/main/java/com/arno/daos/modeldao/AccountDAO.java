package com.arno.daos.modeldao;

import com.arno.daos.GenericDAO;
import com.arno.daos.imodeldao.IAccountDAO;
import com.arno.datamodels.Account;
import com.arno.entities.AccountEntity;
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
