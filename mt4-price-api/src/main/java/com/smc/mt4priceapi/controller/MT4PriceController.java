package com.smc.mt4priceapi.controller;

import com.smc.mt4priceapi.service.Mt4PriceService;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;

@Controller
public class MT4PriceController {

    @Autowired
    private Mt4PriceService mt4PriceService;

    @CrossOrigin(origins = "*")
    @GetMapping("/start/{accountCode}")
    public ResponseEntity<?> start(@PathVariable("accountCode") String accountCode) {
        return mt4PriceService.start(accountCode);
    }

    @CrossOrigin(origins = "*")
    @GetMapping("/price/{symbolCode}")
    public ResponseEntity<?> price(@PathVariable("symbolCode") String symbolCode) {
        return mt4PriceService.price(symbolCode);
    }

    @CrossOrigin(origins = "*")
    @GetMapping("/price/list")
    public ResponseEntity<?> list() {
        return mt4PriceService.list();
    }
    
}
