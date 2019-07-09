package com.ptl.context;

import com.google.gson.Gson;

import java.util.HashMap;
import java.util.Map;
import java.util.prefs.Preferences;

public class AitMainContext {
    private static Preferences settings = Preferences.systemNodeForPackage(AitMainContext.class);

    private static String DEFAULT = "";
    private static String ID_DIC  = "ID_DIC";

    public static Map<String, Double> getIdDic()  throws SecurityException {
        Gson gson = new Gson();
        String d = settings.get(ID_DIC, DEFAULT);
        Map<String, Double> dic = gson.fromJson(d, HashMap.class);
        return DEFAULT.equals(d) || dic == null ? new HashMap<>() : dic;
    }

    public static void setIdDic(Map<String, Double> dic)  throws SecurityException {
        Gson gson = new Gson();
        settings.put(ID_DIC, gson.toJson(dic));
    }
}
