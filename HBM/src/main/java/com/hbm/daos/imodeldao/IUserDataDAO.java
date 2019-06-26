package com.hbm.daos.imodeldao;

import com.hbm.datamodels.UserData;

public interface IUserDataDAO {
    UserData findUserDataById(int id);
}
