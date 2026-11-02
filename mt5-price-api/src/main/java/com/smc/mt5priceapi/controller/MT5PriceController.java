package com.smc.mt5priceapi.controller;

import com.smc.mt5priceapi.service.Mt5PriceService;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;

@Controller
public class MT5PriceController {

    @Autowired
    private Mt5PriceService mt5PriceService;

    @CrossOrigin(origins = "*")
    @GetMapping("/start/{accountCode}")
    public ResponseEntity<?> start(@PathVariable("accountCode") String accountCode) {
        return mt5PriceService.start(accountCode);
    }


    @CrossOrigin(origins = "*")
    @GetMapping("/price/{symbolCode}")
    public ResponseEntity<?> price(@PathVariable("symbolCode") String symbolCode) {
        return mt5PriceService.price(symbolCode);
    }

    @CrossOrigin(origins = "*")
    @GetMapping("/price/list")
    public ResponseEntity<?> list() {
        return mt5PriceService.list();
    }
    
}
