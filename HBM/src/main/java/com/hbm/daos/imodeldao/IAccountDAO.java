package com.hbm.daos.imodeldao;

import com.hbm.datamodels.models.Account;

import java.util.List;

public interface IAccountDAO {
    Account findAccountById(int id);
    List<Account> findAccountByLogin(String login);
    Account findAccountByPass(String pass);
    List<Account> findAccountByEmail(String email);
    List<Account> findActiveAccounts();
    List<Account> findNotActiveAccounts();
    List<Account> findAllAccount();
}
