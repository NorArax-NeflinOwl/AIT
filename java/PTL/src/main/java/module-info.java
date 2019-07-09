module PTL {
    requires gson;
    requires org.jsoup;
    requires java.prefs;
    requires java.sql;

    exports com.ptl;
    exports com.ptl.managers;
    exports com.ptl.resources;
}