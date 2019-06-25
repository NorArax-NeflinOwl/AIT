package com.arno.structures;

import com.arno.resources.AitCommandTypes;

import java.util.Map;

public class AitPackageData {
    private AitClientData client;
    private AitCommandTypes type;
    private Map<String, String> params;

    public AitPackageData(AitClientData client, AitCommandTypes type, Map<String, String> params) {
        this.client = client;
        this.type = type;
        this.params = params;
    }

    public AitClientData getClient() { return client; }
    public AitCommandTypes getType() { return type; }
    public Map<String, String> getParams() { return params; }
}
