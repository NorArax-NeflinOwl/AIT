package com.hbm.interfaces;

import com.hbm.models.entitiecovers.AitUserHost;

import java.util.List;

public interface AitUserHostDAOInterface {
    List<AitUserHost> findUserHostsByAccountId(int id);
    List<AitUserHost> findUserHostsByHostName(String hostName);
}
