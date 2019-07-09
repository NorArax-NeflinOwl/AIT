package com.ptl.managers;

import com.ptl.context.AitMainContext;
import com.ptl.resources.AitLoggerPriority;
import com.ptl.resources.Inerfix;
import com.ptl.resources.Postfix;
import com.ptl.resources.Prefix;

import java.util.Map;

public class AitIdManager {
    private AitIdManager(){}
    private static AitIdManager instance = new AitIdManager();

    public static AitIdManager getInstance() {
        return instance;
    }

    /// Generator
    public String generateId(Prefix prefix) {
        return generateId(prefix, Inerfix.AIT);
    }
    public String generateId(Prefix prefix, Postfix postfix) {
        return generateId(prefix, Inerfix.AIT, postfix);
    }

    public String generateId(Prefix prefix, Inerfix inerfix) {
        return generateId(prefix, inerfix, Postfix.IT);
    }

    public String generateId(Prefix prefix, Inerfix inerfix, Postfix postfix) {
        int i = 0;
        char separator = '-';
        String key = prefix.toString() + separator + inerfix.toString();

        try {
            Map<String, Double> dic = AitMainContext.getIdDic();
            if(dic.containsKey(key)) {
                Double d = dic.get(key);
                i = d.intValue() + 1;
            }
            dic.put(key, i * 1.0);
            AitMainContext.setIdDic(dic);
        } catch (SecurityException ex) {
            AitLogger.getInstance().logToConsole(ex.toString(), AitLoggerPriority.ERROR);
        }

        return key + separator + i + postfix.toString();
    }
}
