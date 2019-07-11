package com.gui.context;

import com.gui.AppGUI;
import com.gui.managers.AitCultureManager;
import com.gui.strings.AitPolishStrings;
import com.hbm.daos.AitDAOFactory;
import com.hbm.daos.models.AitAccountDAO;
import com.hbm.models.AitAccount;
import javafx.scene.control.Alert;
import javafx.scene.control.ButtonType;
import org.apache.log4j.Logger;

public class AitInitializer {
    private static Logger logger = Logger.getLogger(AitMainContext.class);

    private static AitInitializer ourInstance = new AitInitializer();

    public static AitInitializer getInstance() {
        return ourInstance;
    }

    private AitInitializer() {
    }

    public void registerAppSettings() {
        logger.info("opening: AitInitializer.registerAppSettings()");
        AitCultureManager.getInstance().setLanguage(AitPolishStrings.locale);

        logger.info("exiting: AitInitializer.registerAppSettings()");
    }

    public boolean testDBConnection() {
        logger.info("opening: AitInitializer.testDBConnection()");
        try {
            AitMainContext.getSession(true);
        } catch (Exception e){
            Alert alert = new Alert(Alert.AlertType.ERROR, "Error durling db connection!", ButtonType.OK);
            alert.show();
            return false;
        }
        logger.info("exiting: AitInitializer.testDBConnection()");
        return true;
    }
}
