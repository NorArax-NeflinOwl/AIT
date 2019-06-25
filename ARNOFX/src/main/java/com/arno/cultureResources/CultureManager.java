package com.arno.cultureResources;

import com.arno.strings.English;
import com.arno.strings.Polish;

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


    public Language getLanguage() throws Exception {
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
