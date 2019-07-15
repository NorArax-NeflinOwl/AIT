package com.hbm;

import com.hbm.contexts.AitDbContext;

public class AppLocalDb {
    public static void main(String[] args) {
        AitDbContext.getInstance().getConnection();
    }
}
