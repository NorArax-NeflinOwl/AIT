package com.hbm.interfaces;

import com.hbm.models.entitiecovers.AitAccount;

import java.util.List;

public interface AitAccountDAOInterface {
    AitAccount findAccountById(int id);
    List<AitAccount> findAccountByLogin(String login);
    AitAccount findAccountByPass(String pass);
    List<AitAccount> findAccountByEmail(String email);
    List<AitAccount> findActiveAccounts();
    List<AitAccount> findNotActiveAccounts();
    List<AitAccount> findAllAccount();
}
