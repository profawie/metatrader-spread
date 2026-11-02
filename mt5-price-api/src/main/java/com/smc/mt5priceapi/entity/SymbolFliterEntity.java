package com.smc.mt5priceapi.entity;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

@Data
@Table(name = "symbol_fliter")
@Entity
public class SymbolFliterEntity {

    @Id
    @Column(name = "symbol_id")
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer symbolId;

    @Column(name = "symbol_code")
    private String symbolCode;

    @Column(name = "symbol_name")
    private String symbolName;

    @Column(name = "is_active")
    private Boolean active;

}
