package com.hbm.interfaces;

import com.hbm.models.AitSetting;

public interface AitSettingDAOInterface {
    AitSetting findSettingById(int id);
    AitSetting findSettingByName(String name);
}
