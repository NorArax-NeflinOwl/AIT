package com.hbm.models;

import com.hbm.generics.AitGenericModel;
import com.hbm.models.entities.AitNoteEntity;
import org.hibernate.Session;

import java.io.Serializable;

public class AitNote extends AitGenericModel<AitNoteEntity> implements Serializable {
    public AitNote(Session session) {
        super(session);
    }

    public AitNote(Session session, AitNoteEntity entity) {
        super(session);
        this.entity = entity;
    }

}
