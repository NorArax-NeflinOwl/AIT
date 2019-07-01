package com.gui.cultureResources;

import com.gui.strings.English;
import com.gui.strings.Polish;

import java.util.Locale;

public class CultureManager {
    private static volatile CultureManager instance;

    private Language language;

    private CultureManager() {
        language = null;
    }

    public static CultureManager getInstance() {
        if(instance != null) return instance;

        synchronized (CultureManager.class){
            if(instance == null) {
                instance = new CultureManager();
            }
        }
        return instance;
    }


    public Language getLanguage() {
        return language;
    }

    public void init() {
        String locale = Locale.getDefault().toString();
        setLanguage(locale);
    }

    public void setLanguage(String locale) {
        if (locale.equals(Polish.locale)) {
            language = new Polish();
        }
        else{
            language = new English();
        }
    }
}
