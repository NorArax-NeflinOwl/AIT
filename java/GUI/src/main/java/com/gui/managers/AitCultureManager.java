package com.gui.managers;

import com.gui.interfaces.AitLanguageInterface;
import com.gui.strings.AitEnglishStrings;
import com.gui.strings.AitPolishStrings;

import java.util.Locale;

public class AitCultureManager {
    private static volatile AitCultureManager instance;

    private AitLanguageInterface language;

    private AitCultureManager() {
        language = null;
    }

    public static AitCultureManager getInstance() {
        if(instance != null) return instance;

        synchronized (AitCultureManager.class){
            if(instance == null) {
                instance = new AitCultureManager();
            }
        }
        return instance;
    }


    public AitLanguageInterface getLanguage() {
        return language;
    }

    public void init() {
        String locale = Locale.getDefault().toString();
        setLanguage(locale);
    }

    public void setLanguage(String locale) {
        if (locale.equals(AitPolishStrings.locale)) {
            language = new AitPolishStrings();
        }
        else{
            language = new AitEnglishStrings();
        }
    }
}
