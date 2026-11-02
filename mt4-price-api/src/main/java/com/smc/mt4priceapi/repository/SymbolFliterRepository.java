package com.smc.mt4priceapi.repository;

import java.util.List;

import com.smc.mt4priceapi.entity.SymbolFliterEntity;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

public interface SymbolFliterRepository  extends JpaRepository<SymbolFliterEntity, Integer> {

    @Query(value = "SELECT * FROM symbol_fliter a INNER JOIN acc_account b ON a.account_id = b.account_id WHERE a.is_active = 1 AND b.account_code = :accountCode", nativeQuery = true)
    public List<SymbolFliterEntity> findAllByAccountCode(String accountCode);
    
}
