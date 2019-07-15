module WMS {
    requires mail;

    opens com.wms to mail;

    exports com.wms;
}