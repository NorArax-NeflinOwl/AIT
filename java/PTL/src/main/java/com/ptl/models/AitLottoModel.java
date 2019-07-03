package com.ptl.models;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Date;
import java.util.List;

public class AitLottoModel {
    private Date date;
    private List<Integer> luckySix;

    public AitLottoModel(String date, String six) throws ParseException {
        luckySix = new ArrayList<>();

        if(!date.isEmpty()) {
            this.date = new SimpleDateFormat("dd.MM.yyyy").parse(date);
        }

        List<String> r = new ArrayList<>(Arrays.asList(six.split(",")));
        for (String s : r) {
            luckySix.add(Integer.parseInt(s));
        }

        Collections.sort(luckySix);
    }

    public Date getDate() {
        return date;
    }

    public List<Integer> getLuckySix() {
        return luckySix;
    }

    public boolean bingo(AitLottoModel model) {
        return this.luckySix.equals(model.luckySix);
    }
}
