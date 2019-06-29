package com.ptl.structures;

import java.util.Collection;
import java.util.HashMap;
import java.util.Map;
import java.util.Set;

public class AitMap implements Map<String, AitClientData> {

    private Map<String, AitClientData> _data;

    public AitMap() { _data = new HashMap<>(); }

    @Override
    public int size() {
        return _data.size();
    }

    @Override
    public boolean isEmpty() {
        return _data.isEmpty();
    }

    @Override
    public boolean containsKey(Object key) {
        return _data.containsKey(key);
    }

    @Override
    public boolean containsValue(Object value) {
        return _data.containsValue(value);
    }

    @Override
    public AitClientData get(Object key) {
        return _data.get(key);
    }

    @Override
    public AitClientData put(String key, AitClientData value) {
        return _data.put(key, value);
    }

    @Override
    public AitClientData remove(Object key) {
        return _data.remove(key);
    }

    @Override
    public void putAll(Map<? extends String, ? extends AitClientData> m) {
        _data.putAll(m);
    }

    @Override
    public void clear() {
        _data.clear();
    }

    @Override
    public Set<String> keySet() {
        return _data.keySet();
    }

    @Override
    public Collection<AitClientData> values() {
        return _data.values();
    }

    @Override
    public Set<Entry<String, AitClientData>> entrySet() {
        return _data.entrySet();
    }

    @Override
    public boolean equals(Object o) {
        return _data.equals(0);
    }

    @Override
    public int hashCode() {
        return _data.hashCode();
    }
}
