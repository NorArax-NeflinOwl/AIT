package com.hbm.interfaces;

import com.hbm.models.entitiecovers.AitSetting;

public interface AitSettingDAOInterface {
    AitSetting findSettingById(int id);
    AitSetting findSettingByName(String name);
}
