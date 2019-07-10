package com.hbm.managers;

import com.hbm.daos.AitDAOFactory;
import com.hbm.daos.models.AitSettingsDAO;
import com.hbm.hibernate.AitHibernateUtil;
import com.hbm.models.AitSetting;
import com.hbm.models.entities.AitSettingEntity;
import com.ptl.managers.AitLogger;
import com.ptl.resources.AitInerfix;
import com.ptl.resources.AitLoggerPriority;
import com.ptl.resources.AitPostfix;
import com.ptl.resources.AitPrefix;
import org.apache.maven.model.Model;
import org.apache.maven.model.io.xpp3.MavenXpp3Reader;
import org.hibernate.Session;

import java.io.FileReader;
import java.util.Arrays;
import java.util.List;

public class AitIdManager {
    private AitIdManager(){}
    private static Session sessionObj;
    private static AitIdManager instance = new AitIdManager();

    private static final String OK = "<OK>";
    private static final String ERROR = "<ERROR>";

    public static AitIdManager getInstance() {
        return instance;
    }

    /// Generator
    public String generateId(AitPrefix prefix) {
        return generateId(prefix, AitInerfix.AIT);
    }

    public String generateId(AitPrefix prefix, AitPostfix postfix) {
        return generateId(prefix, AitInerfix.AIT, postfix);
    }

    public String generateId(AitPrefix prefix, AitInerfix inerfix) {
        return generateId(prefix, inerfix, AitPostfix.IT);
    }

    public String generateId(AitPrefix prefix, AitInerfix inerfix, AitPostfix postfix) {
        int i = 0;
        char separator = '-';
        String key = prefix.toString() + separator + inerfix.toString();

        try {
            getSession(true).beginTransaction();
            AitSettingsDAO dao = new AitDAOFactory(getSession(true)).getSettingsDAO();
            AitSetting set = dao.findSettingByName(key);

            if(set != null) {
                i = set.getValue() + 1;

                String version = checkVersionNumber(i);
                if(AitPrefix.CR.equals(prefix) && !OK.equals(version)) {
                    throw new Exception("You must increase version mumber to " + version + " and try again!");
                }
            } else {
                AitSettingEntity entity = new AitSettingEntity();
                entity.setName(key);
                set = new AitSetting(getSession(true), entity);
            }
            set.setValue(i);
            set.saveOrUpdate();
            getSession(true).getTransaction().commit();

            return key + separator + i + postfix.toString();
        } catch (Exception ex) {
            AitLogger.getInstance().logToConsole(new Object[] { ex }, AitLoggerPriority.ERROR);
            if(null != getSession(false).getTransaction()) {
                AitLogger.getInstance().logToConsole(".......Transaction Is Being Rolled Back.......");
                getSession(false).getTransaction().rollback();
            }
        }
        return null;
    }

    private String checkVersionNumber(int v) {
        try {
            MavenXpp3Reader reader = new MavenXpp3Reader();
            Model model = reader.read(new FileReader("pom.xml"));
            String verStr = model.getVersion();
            List<String> version = Arrays.asList(verStr.replace('.',',').split(","));
            int fVer = Integer.parseInt(version.get(0)) * 100 != 100 ? Integer.parseInt(version.get(0)) * 100 : 0;
            int versionNumber = fVer + Integer.parseInt(version.get(1)) * 10 + Integer.parseInt(version.get(2));
            if(versionNumber == v){
                return OK;
            } else {
                return (v/100) > 0 ? String.valueOf(v/100) : "1" + "." + (v/10)%10 + "." + v%10;
            }
        } catch (Exception ex) {
            AitLogger.getInstance().logToConsole(new Object[] { ex }, AitLoggerPriority.ERROR);
        }
        return ERROR;
    }

    private static Session getSession(boolean createIfNotExists) {
        if(sessionObj == null && createIfNotExists || sessionObj != null && !sessionObj.isOpen()) {
            sessionObj = AitHibernateUtil.getInstance().getSessionFactory().openSession();
        }
        return sessionObj;
    }
}
