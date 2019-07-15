package com.hbm.managers;

import com.hbm.models.caches.AitLottoModel;
import org.jsoup.Jsoup;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class AitLottoInformator {
    private static String link = "http://www.mbnet.com.pl/dl.txt";
    private static String searchString = "4,6,17,21,24,34";
    private static int intervalDay = 3;

    public void run() {
        Runnable r = () -> {
            try {
                while(true) {
                    if(checkLottoLastResult()) {
                        //FIXME return true;
                        //create notification like alarm, showing on screen and send email
                    }
                    Thread.sleep(3600000 * 24 * intervalDay);
                }
            } catch (Exception e) {
                e.printStackTrace();
            }
        };

        new Thread(r).start();
    }

    public boolean checkLottoLastResult() throws Exception {
        String result = Jsoup.connect(link).get().body().html();
        List<String> results = new ArrayList<>(Arrays.asList(result.split(" ")));

        if(!result.isEmpty()) {
            AitLottoModel lastResult = new AitLottoModel(results.get(results.size() - 2), results.get(results.size() - 1));
            AitLottoModel searchResult = new AitLottoModel("", searchString);

            return lastResult.bingo(searchResult);
        } else {
            throw new Exception("ERROR durling website download and parsing");
        }
    }
}

