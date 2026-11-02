package com.smc.mt5priceapi.service;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.annotation.PostConstruct;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.common.collect.Lists;
import com.google.common.collect.Maps;
import com.smc.mt5priceapi.entity.SymbolFliterEntity;
import com.smc.mt5priceapi.repository.SymbolFliterRepository;

import org.springframework.amqp.core.Message;
import org.springframework.amqp.core.MessageListener;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.ResponseEntity;
import org.springframework.messaging.simp.SimpMessagingTemplate;
import org.springframework.stereotype.Service;

import lombok.extern.slf4j.Slf4j;

@Service
@Slf4j
public class Mt5PriceService implements MessageListener {

    @Autowired
    private SimpMessagingTemplate broker;

    @Autowired
    private SymbolFliterRepository symbolFliterRepository;

    @Value("${cache.directory}")
    private String cacheDirectory;

    private File fCacheDirectory = null;

    @PostConstruct
    public void init() {
        fCacheDirectory = new File(cacheDirectory);
    }

     /**
     * message listener
     */
    @Override
    public void onMessage(Message message) {
        if(message == null) {
            log.debug("rawData is NULL");
            return;
        }
        if(message.getBody() == null) {
            log.debug("getBody is NULL");
            return;
        }
        String rawData = new String(message.getBody());
        //log.debug(rawData);
        send(rawData);
    }
    
   

     /**
     * send
     * @param content
     */
    public void send(String rawData) {
        //log.debug(rawData);
        ObjectMapper objectMapper = new ObjectMapper();
        try {
            Map<String,Object> mapData = objectMapper.readValue(rawData, new TypeReference<HashMap<String,Object>>() {} );
            String accountCode = (String)mapData.get("accountCode");
            if(accountCode == null) {
                return;
            }
            String symbol = (String)mapData.get("symbol");
            if(symbol == null) {
                return;
            }
            String platform = (String)mapData.get("platform");
            if(platform == null) {
                return;
            }
            if(!platform.equalsIgnoreCase("MT5")) {
                return;
            }
            
            String subscribe = "/topic/mt5price/" + accountCode;
            broker.convertAndSend(subscribe, mapData);
            
            //save to cache file
            File file = new File(cacheDirectory, symbol);
            if(!file.exists()) {
                file.createNewFile();
            }
            try(ObjectOutputStream  oi = new ObjectOutputStream(new FileOutputStream(file))) {
                oi.writeObject(mapData);
            }catch(Exception e) {
                log.error(e.getMessage(), e);
            }

        }catch(Exception e) {
            log.error(e.getMessage(), e);
        }        
    }

    /**
     * start
     * 
     * @return
     */
    public ResponseEntity<?> start(String accountCode) {
        List<Map<String,Object>> prices = Lists.newLinkedList();
        List<SymbolFliterEntity> symbolFliters = symbolFliterRepository.findAllByAccountCodeAndPlatform(accountCode, "MT5");
        for(SymbolFliterEntity symbolFliter : symbolFliters) {
            File file = new File(fCacheDirectory, symbolFliter.getSymbolCode());
            if(file.exists()) {
                try(ObjectInputStream oi = new ObjectInputStream(new FileInputStream(file))) {
                    Map<String,Object> price = (Map<String,Object>)oi.readObject();
                    prices.add(price);
               }catch(Exception e) {
                   log.error(e.getMessage(), e);
               }
            }
        }
        return ResponseEntity.ok(prices);
    }

    /**
     * price
     * 
     * @return
     */
    public ResponseEntity<?> price(String symbolCode) {
        File file = new File(fCacheDirectory, symbolCode);
        Map<String,Object> price = Maps.newHashMap();
        if(file.exists()) {
            try(ObjectInputStream oi = new ObjectInputStream(new FileInputStream(file))) {
                price = (Map<String,Object>)oi.readObject();
            }catch(Exception e) {
                log.error(e.getMessage(), e);
            }
        }
        return ResponseEntity.ok(price);
    }


    /**
     * list
     * 
     * @return
     */
    public ResponseEntity<?> list() {
        List<String> symbols = Lists.newLinkedList();
        File[] files = fCacheDirectory.listFiles();
        if(files != null) {
            for(File file : files) {
                symbols.add(file.getName());
            }
        }
        return ResponseEntity.ok(symbols);
    }
}
