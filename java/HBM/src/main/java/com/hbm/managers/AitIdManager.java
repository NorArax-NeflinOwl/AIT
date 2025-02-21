package com.hbm.managers;

import com.hbm.daos.AitDAOFactory;
import com.hbm.daos.models.AitSettingsDAO;
import com.hbm.hibernate.AitHibernateUtil;
import com.hbm.models.entitiecovers.AitSetting;
import com.hbm.models.entities.AitSettingEntity;
import com.hbm.resources.AitInerfix;
import com.hbm.resources.AitPostfix;
import com.hbm.resources.AitPrefix;
import org.apache.maven.model.Model;
import org.apache.maven.model.io.xpp3.MavenXpp3Reader;
import org.hibernate.Session;

import java.io.FileReader;
import java.util.Arrays;
import java.util.List;
import java.util.Properties;

public class AitIdManager {
    private AitIdManager(){}
    private static Session sessionObj;
    private static AitIdManager instance = new AitIdManager();

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

            boolean canCommit = true;
            if (set != null) {
                i = set.getValue() + 1;
                canCommit = checkVersionNumber(i, prefix);
            } else {
                AitSettingEntity entity = new AitSettingEntity();
                entity.setName(key);
                set = new AitSetting(getSession(true), entity);
            }

            if (canCommit) {
                set.setValue(i);
                set.saveOrUpdate();
                return key + separator + i + postfix.toString();
            }
        } catch (Exception ex) {
            ex.printStackTrace();
        }
        return null;
    }

    private boolean checkVersionNumber(int v, AitPrefix prefix) {
        boolean error = false;
        try {
            MavenXpp3Reader reader = new MavenXpp3Reader();
            Model model = reader.read(new FileReader("pom.xml"));
            Properties properties = model.getProperties();
            String verStr = properties.getProperty("global.version");
            List<String> versionTab = Arrays.asList(verStr.replace('.',',').split(","));

            int version = Integer.parseInt(versionTab.get(0));
            int reqVer = Integer.parseInt(versionTab.get(1));
            int crVer = Integer.parseInt(versionTab.get(2));

            if(AitPrefix.REQ.equals(prefix) && reqVer != v) {
                reqVer = v;
                error = true;
            }
            if(AitPrefix.CR.equals(prefix) && crVer != v){
                crVer = v;
                error = true;
            }

            if(error) {
                throw new Exception("You must increase version mumber to " + version + "." + reqVer + "." + crVer + " and try again!");
            }
        } catch (Exception ex) {
            AitLogger.getInstance().logErrorToFile(ex);
        }
        return !error;
    }

    private static Session getSession(boolean createIfNotExists) {
        if(sessionObj == null && createIfNotExists || sessionObj != null && !sessionObj.isOpen()) {
            sessionObj = AitHibernateUtil.getInstance().getSessionFactory().openSession();
        }
        return sessionObj;
    }
}
