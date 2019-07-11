package com.hbm.daos.models;

import com.hbm.generics.AitGenericDAO;
import com.hbm.interfaces.AitNoteDAOInterface;
import com.hbm.models.AitNote;
import com.hbm.models.entities.AitNoteEntity;
import org.hibernate.Session;

import java.util.List;

public class AitNoteDAO extends AitGenericDAO<AitNoteEntity, Integer> implements AitNoteDAOInterface {

    public AitNoteDAO(Session session) {
        super(session);
    }

    @Override
    public List<AitNote> findNotesByAccountId(int id) {
        return null;
    }
}
