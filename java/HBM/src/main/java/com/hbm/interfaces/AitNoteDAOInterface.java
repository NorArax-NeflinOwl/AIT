package com.hbm.interfaces;

import com.hbm.models.AitNote;

import java.util.List;

public interface AitNoteDAOInterface {
    List<AitNote> findNotesByAccountId(int id);
}
