module PTL {
    requires gson;
    requires org.jsoup;
    requires java.prefs;
    requires javax.mail;

    exports com.ptl;
    exports com.ptl.managers;
    exports com.ptl.resources;
}