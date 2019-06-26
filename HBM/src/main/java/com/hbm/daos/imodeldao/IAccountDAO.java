package com.hbm.daos.imodeldao;

import com.hbm.datamodels.Account;

public interface IAccountDAO {
    Account findAccountById(int id);
}
