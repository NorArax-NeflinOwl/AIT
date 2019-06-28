package com.hbm.daos.imodeldao;

import com.hbm.datamodels.Account;

import java.util.List;

public interface IAccountDAO {
    Account findAccountById(int id);
    Account findAccountByLogin(String login);
    Account findAccountByPass(String pass);
    Account findAccountByEmail(String email);
    List<Account> findActiveAccounts();
    List<Account> findNotActiveAccounts();
    List<Account> findAllAccount();
}
