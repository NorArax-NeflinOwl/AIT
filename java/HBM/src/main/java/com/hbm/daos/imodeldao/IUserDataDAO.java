package com.hbm.daos.imodeldao;

import com.hbm.datamodels.models.UserData;

import java.util.List;

public interface IUserDataDAO {
    UserData findUserDataById(int id);
    List<UserData> findUserDataByNick(String nick);
    List<UserData> findUserDataByFirstName(String firstName);
    List<UserData> findUserDataByMiddleName(String middleName);
    List<UserData> findUserDataByLastName(String lastName);
    List<UserData> findAllUserData();
}
