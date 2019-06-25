package com.arno.daos.imodeldao;

import com.arno.datamodels.UserData;

public interface IUserDataDAO {
    UserData findUserDataById(int id);
}
