package com.hbm.interfaces;

import com.hbm.models.entitiecovers.AitNote;

import java.util.List;

public interface AitNoteDAOInterface {
    List<AitNote> findNotesByAccountId(int id);
}
