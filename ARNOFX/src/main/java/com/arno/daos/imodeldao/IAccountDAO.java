package com.arno.daos.imodeldao;

import com.arno.datamodels.Account;

public interface IAccountDAO {
    Account findAccountById(int id);
}
