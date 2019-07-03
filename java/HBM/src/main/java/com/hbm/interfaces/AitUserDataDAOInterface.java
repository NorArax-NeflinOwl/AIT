package com.hbm.interfaces;

import com.hbm.models.AitUserData;

import java.util.List;

public interface AitUserDataDAOInterface {
    AitUserData findUserDataById(int id);
    List<AitUserData> findUserDataByNick(String nick);
    List<AitUserData> findUserDataByFirstName(String firstName);
    List<AitUserData> findUserDataByMiddleName(String middleName);
    List<AitUserData> findUserDataByLastName(String lastName);
    List<AitUserData> findAllUserData();
    AitUserData findUserDataByAccountId(int id);
}
