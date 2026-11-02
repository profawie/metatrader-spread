import { MDBBtn, MDBCol, MDBContainer, MDBRow, MDBSpinner, MDBTypography } from 'mdb-react-ui-kit';
import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Select from 'react-select';
import { v4 as uuidv4 } from 'uuid';
import { GET_SPREAD } from './constant/spread';

function App() {
  
  const options = [
    {value: 'topTraded', label: 'Top Traded'},
    {value: 'stock', label: 'Stocks'},
    {value: 'indices', label: 'Indices'},
    {value: 'forex', label: 'Forex'},
    {value: 'comm', label: 'Commodities'},
    {value: 'crypto', label: 'Cryptocurrencies'},
  ];

  const dispatch = useDispatch();
  const { data, loading } = useSelector((state) => state.spread);
  const [category, setCategory] = React.useState('topTraded');
  const page = localStorage.getItem('page');
  console.log("page: "+page);


  //structure
  const TOP_TRADED = [
    {i: 1, code: "EURUSD"},
    {i: 2, code: "GBPUSD"},
    {i: 3, code: "XAUUSD"},
    {i: 4, code: "XAGUSD"},
    {i: 5, code: "TESLA"},
    {i: 6, code: "APPLE"},
    {i: 7, code: "MICROSOFT"},
    {i: 8, code: "FACEBOOK"},
    {i: 9, code: "USOUSD"},
    {i: 10, code: "UKOUSD"},
    {i: 11, code: "BCHUSD"},
    {i: 12, code: "BTCUSD"},
    {i: 13, code: "ETHUSD"},
    {i: 14, code: "LTCUSD"},
    {i: 15, code: "RPLUSD"}
  ];

  const STOCK = [
    {i: 1, code: "TESLA"},
    {i: 2, code: "APPLE"},
    {i: 3, code: "AMD"},
    {i: 4, code: "NIO"},
    {i: 5, code: "MICROSOFT"},
    {i: 6, code: "FACEBOOK"},
    {i: 7, code: "NVIDIA"},
    {i: 8, code: "INTEL"},
    {i: 9, code: "ZOOM"},
    {i: 10, code: "BOEING"},
    {i: 11, code: "ALIBABA"},
    {i: 12, code: "3M"},
    {i: 13, code: "Regneneron"}
  ];

  const INDICES = [
    {i: 1, code: "100GBP"},
    {i: 2, code: "200AUD"},
    {i: 3, code: "225JPY"},
    {i: 4, code: "D30EUR"},
    {i: 5, code: "E35EUR"},
    {i: 6, code: "E50EUR"},
    {i: 7, code: "F40EUR"},
    {i: 8, code: "H33HKD"},
    {i: 9, code: "NASUSD"},
    {i: 10, code: "SPXUSD"},
    {i: 11, code: "U30USD"}
  ];

  const FOREX = [
    {i: 0, code: "GBPUSD"},
    {i: 1, code: "XAUUSD"},
    {i: 2, code: "EURUSD"},
    {i: 3, code: "GBPJPY"},
    {i: 4, code: "USDCAD"},
    {i: 5, code: "GBPAUD"},
    {i: 6, code: "EURJPY"},
    {i: 7, code: "GBPCHF"},
    {i: 8, code: "NZDUSD"},
    {i: 9, code: "AUDUSD"},
    {i: 10, code: "USDCHF"},
    {i: 11, code: "EURAUD"},
    {i: 12, code: "USDJPY"},
    {i: 13, code: "GBPCAD"},
    {i: 14, code: "GBPNZD"}
  ];

  const COMM_METALS = [
    {i: 0, code: "USOUSD"},
    {i: 1, code: "UKOUSD"},
    {i: 2, code: "XAUUSD"},
    {i: 3, code: "XAGUSD"}
  ];

  const CRYPTO = [
    {i: 1, code: "BCHUSD"},
    {i: 3, code: "BTCUSD"},
    {i: 5, code: "ETHUSD"},
    {i: 2, code: "LTCUSD"},
    {i: 4, code: "RPLUSD"}
  ];


  const round = (value, decimals) => {
    var digits = Math.pow(10, decimals);
      return addZeroes(Math.round(value * digits) / digits, decimals);
  };

  const addZeroes = (num, decimals) => {
    const snum = ''+num;	
    const dec = snum.split('.')[1];
    const len = dec && dec.length > decimals ? dec.length : decimals;
    return Number(snum).toFixed(len);
  }
  


  useEffect(() => {
    if(page != "") {
      setCategory(page);
    }
    dispatch({
      type: GET_SPREAD
    });
  }, []);

  const categoryFilter = (data) => {
    if(category == 'topTraded') {
      return TOP_TRADED.map((a) => { return a.code; }).indexOf(data.symbol) != -1;
    }
    else if(category == 'stock') {
      return STOCK.map((a) => { return a.code; }).indexOf(data.symbol) != -1;
    }
    else if(category == 'indices') {
      return INDICES.map((a) => { return a.code; }).indexOf(data.symbol) != -1;
    }
    else if(category == 'forex') {
      return FOREX.map((a) => { return a.code; }).indexOf(data.symbol) != -1;
    }
    else if(category == 'comm') {
      return COMM_METALS.map((a) => { return a.code; }).indexOf(data.symbol) != -1;
    }
    else if(category == 'crypto') {
      return CRYPTO.map((a) => { return a.code; }).indexOf(data.symbol) != -1;
    }
    return false;
  };

  const fData = data
  //category filter
  .filter(categoryFilter)
  
  //append i (sorting)
  .map((d) => {
    if(category == 'topTraded') {
      TOP_TRADED.map((a) => { 
        if(a.code == d.symbol) {
          d.i = a.i;
        }
        return a;
      });
    }
    else if(category == 'stock') {
      STOCK.map((a) => { 
        if(a.code == d.symbol) {
          d.i = a.i;
        }
        return a;
      });
    }
    else if(category == 'indices') {
      INDICES.map((a) => { 
        if(a.code == d.symbol) {
          d.i = a.i;
        }
        return a;
      });
    }
    else if(category == 'forex') {
      FOREX.map((a) => { 
        if(a.code == d.symbol) {
          d.i = a.i;
        }
        return a;
      });
    }
    else if(category == 'comm') {
      COMM_METALS.map((a) => { 
        if(a.code == d.symbol) {
          d.i = a.i;
        }
        return a;
      });
    }
    else if(category == 'crypto') {
      CRYPTO.map((a) => { 
        if(a.code == d.symbol) {
          d.i = a.i;
        }
        return a;
      });
    }
    return d;

  //sorting i  
  }).sort((a, b) => {
    if(a.i == b.i) {
      return 0;
    }
    if(a.i < b.i) {
      return -1;
    }
    if(a.i > b.i) {
      return 1;
    }
    
  });
  if(fData.length % 2  !== 0)  {
    fData.push({symbol: 'dummy'});
  }

  return<MDBContainer fluid>
  <MDBRow  className="gy-2">
    &nbsp;
  </MDBRow>
  
  {/* Mobile - Dropdown */}
  {page == "" && <MDBRow center className="d-lg-none">
    <Select
      value={options.filter(function(option) {
        return option.value === category;
      })}
      onChange={(select) => {
        setCategory(select.value);
      } }
      options={options} />
  </MDBRow>}  

  {/* Web - Buttons */}
  {page == "" && <MDBRow between className="g-1 spHiddenInMobile">
      <MDBCol size="2">
        <MDBBtn 
        onClick={() => setCategory('topTraded') }
        className={`spBtnTopTraded ${category == 'topTraded' ? "spBtnSelected" : ""}`}>Top Traded</MDBBtn>
      </MDBCol>
      <MDBCol size="2">
        <MDBBtn 
        onClick={() => setCategory('stock') }
        className={`spBtnStocks ${category == 'stock' ? "spBtnSelected" : ""}`}>Stocks</MDBBtn>
      </MDBCol>
      <MDBCol size="2">
        <MDBBtn 
        onClick={() => setCategory('indices') }
        className={`spBtnIndices ${category == 'indices' ? "spBtnSelected" : ""}`}>Indices</MDBBtn>
      </MDBCol>
      <MDBCol size="2">
        <MDBBtn 
        onClick={() => setCategory('forex') }
        className={`spBtnForex ${category == 'forex' ? "spBtnSelected" : ""}`}>Forex</MDBBtn>
      </MDBCol>
      <MDBCol size="2">
        <MDBBtn 
        onClick={() => setCategory('comm') }
        className={`spBtnComm ${category == 'comm' ? "spBtnSelected" : ""}`}>Commodities</MDBBtn>
      </MDBCol>
      <MDBCol size="2">
        <MDBBtn 
        onClick={() => setCategory('crypto') }
        className={`spBtnCrypto ${category == 'crypto' ? "spBtnSelected" : ""}`}>Cryptocurrencies</MDBBtn>
      </MDBCol>
  </MDBRow>}

  {/* Mobile - Header  */}
  {!loading && <MDBRow className="d-lg-none">
    <MDBCol size="12">
      <MDBRow className="ps-2 pe-1">
        <MDBCol size="3" className="spMobileHeader">
          <MDBTypography tag='h6'>Markets</MDBTypography>
        </MDBCol>
        <MDBCol size="3" className="spMobileHeader">
          <MDBTypography tag='h6'>Sell</MDBTypography>
        </MDBCol>
        <MDBCol size="3" className="spMobileHeader">
          <MDBTypography tag='h6'>Buy</MDBTypography>
        </MDBCol>
        <MDBCol size="3" className="spMobileHeader">
          <MDBTypography tag='h6'>Spreads</MDBTypography>
        </MDBCol>
      </MDBRow>
    </MDBCol>
  </MDBRow>}


  {/* Web - Header  */}
  {!loading && <MDBRow className="spHiddenInMobile">
    <MDBCol size="6" className="ps-5 pe-5">
      <MDBRow>
        <MDBCol size="2" className="spWebHeader">
          <MDBTypography tag='h6'>Markets</MDBTypography>
        </MDBCol>
        <MDBCol size="4" className="spWebHeader">
          &nbsp;
        </MDBCol>
        <MDBCol size="2" className="spWebHeader">
          <MDBTypography tag='h6'>Sell</MDBTypography>
        </MDBCol>
        <MDBCol size="2" className="spWebHeader">
          <MDBTypography tag='h6'>Buy</MDBTypography>
        </MDBCol>
        <MDBCol size="2" className="spWebHeader">
          <MDBTypography tag='h6'>Spreads</MDBTypography>
        </MDBCol>
      </MDBRow>
    </MDBCol>
    <MDBCol size="6" className="ps-5 pe-5">
      <MDBRow>
        <MDBCol size="2" className="spWebHeader">
          <MDBTypography tag='h6'>Markets</MDBTypography>
        </MDBCol>
        <MDBCol size="4" className="spWebHeader">
          &nbsp;
        </MDBCol>
        <MDBCol size="2" className="spWebHeader">
          <MDBTypography tag='h6'>Sell</MDBTypography>
        </MDBCol>
        <MDBCol size="2" className="spWebHeader">
          <MDBTypography tag='h6'>Buy</MDBTypography>
        </MDBCol>
        <MDBCol size="2" className="spWebHeader">
          <MDBTypography tag='h6'>Spreads</MDBTypography>
        </MDBCol>
      </MDBRow>
    </MDBCol>
  </MDBRow>}

  {loading && <>
  <div className="p-5"></div>
  <div className='d-flex justify-content-center'>
    <MDBSpinner role='status'>
      <span className='visually-hidden'>Loading...</span>
    </MDBSpinner>
  </div>
  </>}
  
  {/* Mobile - Body  */}
  {!loading && <MDBRow center className="d-lg-none">
    {fData.map((d) => {
      var  spread = round(Math.abs(d.bid - d.ask) * Math.pow(10, d.digits), 0);
      var  ask = round(d.ask, d.digits);
      var  bid = round(d.bid, d.digits);
      var id = d.symbol;
      id = id.replace('.', '_');
      var idBidM = id +"M_bid";
      var idAskM = id +"M_ask";
      var idSpreadM = id +"M_spread";
      
      return <MDBCol key={uuidv4()} size="12" className="ps-2 pe-1">
      <MDBRow className="">
        <hr className="spDivider" />
        <MDBCol size="3">
          <div className="spMImgSymbol">
            {d.symbol === 'dummy' ? <></> : <img src={`https://mt5price.s3.eu-west-2.amazonaws.com/${d.symbol}.png`}></img>}
          </div>  
          <MDBTypography  className="spMRowName" tag='h6'>{d.symbol === 'dummy' ? "" : d.name}</MDBTypography>
        </MDBCol>
        <MDBCol size="3">
          <MDBTypography  className="spMRowSell" id={idBidM} tag='h6'>{d.symbol === 'dummy' ? '' : bid}</MDBTypography>
        </MDBCol>
        <MDBCol size="3">
          <MDBTypography  className="spMRowBuy" id={idAskM} tag='h6'>{d.symbol === 'dummy' ? '' : ask}</MDBTypography>
        </MDBCol>
        <MDBCol size="3">
          <MDBTypography  className="spMRowSpreads" id={idSpreadM} tag='h6'>{d.symbol === 'dummy' ? '' : spread}</MDBTypography>
        </MDBCol>
      </MDBRow>
    </MDBCol>
    })}
  </MDBRow>}

  {/* Web - Body  */}
  {!loading && <MDBRow center className="spHiddenInMobile">
    {fData.map((d) => {
      var  spread = round(Math.abs(d.bid - d.ask) * Math.pow(10, d.digits), 0);
      var  ask = round(d.ask, d.digits);
      var  bid = round(d.bid, d.digits);
      var id = d.symbol;
      id = id.replace('.', '_');
      var idBidW = id +"W_bid";
      var idAskW = id +"W_ask";
      var idSpreadW = id +"W_spread";
      return <MDBCol key={uuidv4()} size="6" className="ps-5 pe-5">
      <MDBRow className="">
        <hr className="spDivider" />
        <MDBCol size="6">
            <div className="spWImgSymbol">
              {d.symbol === 'dummy' ? <></> : <img src={`https://mt5price.s3.eu-west-2.amazonaws.com/${d.symbol}.png`}></img>}
            </div>
            <MDBTypography  className="spWRowName" tag='h6'>{d.symbol === 'dummy' ? '' : d.name}</MDBTypography>
        </MDBCol>
        <MDBCol size="2">
          <MDBTypography  className="spWRowSell" id={idBidW} tag='h6'>{d.symbol === 'dummy' ? '' : bid}</MDBTypography>
        </MDBCol>
        <MDBCol size="2">
          <MDBTypography  className="spWRowBuy" id={idAskW} tag='h6'>{d.symbol === 'dummy' ? '' : ask}</MDBTypography>
        </MDBCol>
        <MDBCol size="2">
          <MDBTypography  className="spWRowSpreads" id={idSpreadW} tag='h6'>{d.symbol === 'dummy' ? '' : spread}</MDBTypography>
        </MDBCol>
      </MDBRow>
    </MDBCol>
    })}
  </MDBRow>}
</MDBContainer>;
}

export default App;
