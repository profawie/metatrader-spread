import { MDBBtn, MDBCard, MDBCardBody, MDBCardText, MDBCardTitle, MDBCol, MDBContainer, MDBRow, MDBSpinner } from 'mdb-react-ui-kit';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useMediaQuery } from 'react-responsive';
import Accordion from './Accordion';
import { GET_SPREAD } from './constant/spread';

function App() {
  
  const dispatch = useDispatch();
  const { data, loading } = useSelector((state) => state.spread);
  const [account, setAccount] = useState('Micro');
  const [search, setSearch] = useState('');
  const isMobile = useMediaQuery({ maxWidth: 767 });

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

  const PRO = [
    "EURUSD",
    "GBPUSD",
    "XAUUSD",
    "XAGUSD",
    "USOUSD",
    "UKOUSD",
    "GBPJPY",
    "USDCAD",
    "GBPAUD",
    "EURJPY",
    "GBPCHF",
    "NZDUSD",
    "AUDUSD",
    "USDCHF",
    "EURAUD",
    "USDJPY",
    "GBPCAD",
    "GBPNZD"
  ];

  const ECN = [
    "EURUSD.ecn",
    "GBPUSD.ecn",
    "XAUUSD.ecn",
    "XAGUSD.ecn",
    "USOUSD.ecn",
    "UKOUSD.ecn",
    "GBPJPY.ecn",
    "USDCAD.ecn",
    "GBPAUD.ecn",
    "EURJPY.ecn",
    "GBPCHF.ecn",
    "NZDUSD.ecn",
    "AUDUSD.ecn",
    "USDCHF.ecn",
    "EURAUD.ecn",
    "USDJPY.ecn",
    "GBPCAD.ecn",
    "GBPNZD.ecn"
  ];

  const STANDARD = [
    "EURUSD.",
    "GBPUSD.",
    "XAUUSD.",
    "XAGUSD.",
    "USOUSD.",
    "UKOUSD.",
    "GBPJPY.",
    "USDCAD.",
    "GBPAUD.",
    "EURJPY.",
    "GBPCHF.",
    "NZDUSD.",
    "AUDUSD.",
    "USDCHF.",
    "EURAUD.",
    "USDJPY.",
    "GBPCAD.",
    "GBPNZD."
  ];

  const MICRO = [
    "EURUSD_",
    "GBPUSD_",
    "XAUUSD_",
    "XAGUSD_",
    "USOUSD_",
    "UKOUSD_",
    "GBPJPY_",
    "USDCAD_",
    "GBPAUD_",
    "EURJPY_",
    "GBPCHF_",
    "NZDUSD_",
    "AUDUSD_",
    "USDCHF_",
    "EURAUD_",
    "USDJPY_",
    "GBPCAD_",
    "GBPNZD_"
  ];

  const dataPro = [];
  const dataStandard = [];
  const dataEcn = [];
  const dataMirco = [];
  
  const dataFilter = (data) => {
    data.map((d) => {
      for(const dd of PRO) {
        if(dd == d.symbol) {
          dataPro.push({
            code: dd,
            ...d
          });
          continue;
        }
      }
      for(const dd of STANDARD) {
        if(dd == d.symbol) {
          dataStandard.push({
            code: dd.substring(0, dd.length-1),
            ...d
          });
          continue;
        }
      }
      for(const dd of ECN) {
        if(dd == d.symbol) {
          dataEcn.push({
            code: dd.substring(0, dd.length-4),
            ...d
          });
          continue;
        }
      }
      for(const dd of MICRO) {
        if(dd == d.symbol) {
          dataMirco.push({
            code: dd.substring(0, dd.length-1),
            ...d
          });
          continue;
        }
      }
     
      return d;
    });
  }

  const calcPro = (d) => {
    const dd = dataPro.find(p => p.code == d.symbol);
    if(!dd) {
      return 0;
    }
    const spread = Math.pow(10, d.digits - 1);
    const a = (parseFloat(dd.ask) - parseFloat(dd.bid)) * spread;
    return round(a, 1);
  };

  const calcStandard = (d) => {
    const dd = dataStandard.find(p => p.code == d.symbol);
    if(!dd) {
      return 0;
    }
    const spread = Math.pow(10, d.digits - 1);
    const a = (parseFloat(dd.ask) - parseFloat(dd.bid)) * spread;
    return round(a, 1);
  };

  const calcEcn = (d) => {
    const dd = dataEcn.find(p => p.code == d.symbol);
    if(!dd) {
      return 0;
    }
    const spread = Math.pow(10, d.digits - 1);
    const a = (parseFloat(dd.ask) - parseFloat(dd.bid)) * spread;
    return round(a, 1);
  };

  const calcMicro = (d) => {
    const dd = dataMirco.find(p => p.code == d.symbol);
    if(!dd) {
      return 0;
    }
    const spread = Math.pow(10, d.digits - 1);
    const a = (parseFloat(dd.ask) - parseFloat(dd.bid)) * spread;
    return round(a, 1);
  }


  useEffect(() => {
    dispatch({
      type: GET_SPREAD
    });
  }, []);


  dataFilter(data);

  return<MDBContainer fluid>
  <MDBRow  className="gy-2">
    &nbsp;
  </MDBRow>
  

  {loading && <>
  <div className="p-5"></div>
  <div className='d-flex justify-content-center'>
    <MDBSpinner role='status'>
      <span className='visually-hidden'>Loading...</span>
    </MDBSpinner>
  </div>
  </>}
  
  {!loading && <MDBRow center>
    <div className="d-flex justify-content-center pt-2 pb-2">
      <div className="col col-lg-3 col-sm-12">
        <div className="input-group">
          <input type="search" onChange={(e) => setSearch(e.target.value)} className="form-control" />
          <button type="button" className="btn btn-outline-dark btnSearch" data-mdb-ripple-color="dark">
            <i className="fas fa-search"></i>
            </button>
          </div>
      </div>
   </div>
    <div className="d-flex justify-content-center pt-2 pb-2">
          <button onClick={(e) => {
            setAccount('Micro');
          }} type="button" className={account == 'Micro' ? "btn btn-primary btnSelected btnMicro" :  "btn btn-outline-dark btnMicro"} data-mdb-ripple-color="dark">
            Micro
          </button>
          &nbsp;
          <button onClick={(e) => {
            setAccount('Standard');
          }} type="button"  className={account == 'Standard' ? "btn btn-primary btnSelected btnStandard" :  "btn btn-outline-dark btnStandard"} data-mdb-ripple-color="dark">
            Standard
          </button>
          &nbsp;
          <button onClick={(e) => {
            setAccount('Pro');
          }} type="button"  className={account == 'Pro' ? "btn btn-primary btnSelected btnPro" :  "btn btn-outline-dark btnPro"} data-mdb-ripple-color="dark">
            Pro
          </button>
          &nbsp;
          <button onClick={(e) => {
            setAccount('ECN');
          }} type="button"  className={account == 'ECN' ? "btn btn-primary  btnSelected btnEcn" :  "btn btn-outline-dark btnEcn"} data-mdb-ripple-color="dark">
            ECN
          </button>
    </div>

    <div className="accordion">
      {dataPro.filter((d) => {
        return d.symbol.toLowerCase().includes(search);
      }).map((d, i) => {
        var  ask = round(d.ask, d.digits);
        var  bid = round(d.bid, d.digits);
        
        const pro = calcPro(d);
        const standard = calcStandard(d);
        const ecn = calcEcn(d);
        const micro = calcMicro(d);
        const name = d.name != '' ? d.name : d.symbol.substring(0, 3) + '/' + d.symbol.substring(3, d.symbol.length);
        let volMax = d.VolumeMaxExt;
        let volMin = d.VolumeMinExt;
        let volStep = d.VolumeStepExt;

        if(volMax > 0) {
          volMax = volMax / 100000000;
        }
        if(volMin > 0) {
          volMin = volMin / 100000000;
        }
        if(volStep > 0) {
          volStep = volStep / 1000000;
        }

        //
        // mobile
        //
        if(isMobile) {
          return <Accordion
          title={<MDBRow style={{width: '95vw'}} center>
            <MDBRow>
                <MDBCol>
                    <div className="spWRowName">{name}</div>
                </MDBCol>
            </MDBRow>
            <MDBRow>
              <MDBCol size="4">
                <div className="spWRowTBid">Bid</div>
                <div className="spWRowVBid" id={"bid_" + d.symbol}>{bid} <i className='px-2 fas fa-angle-right'></i></div>
              </MDBCol>
              <MDBCol size="4">
                <div className="spWRowTAsk">Ask</div>
                <div className="spWRowVAsk" id={"ask_" + d.symbol}>{ask} <i className='px-2 fas fa-angle-right'></i></div>
              </MDBCol>
              {account == 'Micro' && <MDBCol size="2">
                <div className={account == 'Micro' ? "spWRowTMirco spSelect" : "spWRowTMirco"}>Micro</div>
                <div className="spWRowVMicro" id={"micro_" + d.symbol}>{micro}</div>
              </MDBCol>}
              {account == 'Standard' && <MDBCol size="2">
                <div className={account == 'Standard' ? "spWRowTStandard spSelect" : "spWRowTStandard"}>Standard</div>
                <div className="spWRowVStandard" id={"standard_" + d.symbol}>{standard}</div>
              </MDBCol>}
              {account == 'Pro' && <MDBCol size="2">
                <div className={account == 'Pro' ? "spWRowTPro spSelect" : "spWRowTPro"}>Prime</div>
                <div className="spWRowVPro" id={"pro_" + d.symbol}>{pro}</div>
              </MDBCol>}
              {account == 'ECN' && <MDBCol size="2">
                <div className={account == 'ECN' ? "spWRowTECN spSelect" : "spWRowTECN"}>ECN</div>
                <div className="spWRowVECN" id={"ecn_" + d.symbol}>{ecn}</div>
              </MDBCol>}
            </MDBRow>
          </MDBRow>}
          content={
            <MDBRow center>
              <MDBCol size="12">
                <MDBCard style={{borderRadius: 0}}>
                  <MDBCardBody>
                      <MDBCardTitle  className="spWRowName">{name}</MDBCardTitle>
                      <MDBCardText>
                        <MDBRow center>
                          <MDBCol size="8">
                            <MDBRow center>
                              <MDBCol size="8" className="spRowDetailName">Margin requirement</MDBCol>
                              <MDBCol size="4" className="spRowDetailValue">{volStep}%</MDBCol>
                            </MDBRow>
                            <MDBRow center>
                              <MDBCol size="8" className="spRowDetailName">Min. transaction size</MDBCol>
                              <MDBCol size="4" className="spRowDetailValue">{volMin}</MDBCol>
                            </MDBRow>
                            <MDBRow center>
                              <MDBCol size="8" className="spRowDetailName">Max. transaction size</MDBCol>
                              <MDBCol size="4" className="spRowDetailValue">{volMax}</MDBCol>
                            </MDBRow>
                          </MDBCol>
                          <MDBCol size="4">
                              <MDBBtn className="btnSelected" onClick={() => {
                                window.open('https://www.example.com/login');
                              }}>Trade</MDBBtn>            
                          </MDBCol>
                         </MDBRow> 
                      </MDBCardText>
                    </MDBCardBody>
                </MDBCard>
              </MDBCol>
            </MDBRow>
          }
        />
        }else {

          //
          // web
          //
          return <Accordion
          title={<MDBRow style={{width: '95vw'}} center>
            <MDBRow>
                <MDBCol>
                    <div className="spWRowName">{name}</div>
                </MDBCol>
            </MDBRow>
            <MDBRow>
              <MDBCol size="3">
                <div className="spWRowTBid">Bid</div>
                <div className="spWRowVBid" id={"bid_" + d.symbol}>{bid} <i className='px-2 fas fa-angle-right'></i></div>
              </MDBCol>
              <MDBCol size="3">
                <div className="spWRowTAsk">Ask</div>
                <div className="spWRowVAsk" id={"ask_" + d.symbol}>{ask} <i className='px-2 fas fa-angle-right'></i></div>
              </MDBCol>
              <MDBCol size="1">
                <div className={account == 'Micro' ? "spWRowTMirco spSelect" : "spWRowTMirco"}>Micro</div>
                <div className="spWRowVMicro" id={"micro_" + d.symbol}>{micro}</div>
              </MDBCol>
              <MDBCol size="1">
                <div className={account == 'Standard' ? "spWRowTStandard spSelect" : "spWRowTStandard"}>Standard</div>
                <div className="spWRowVStandard" id={"standard_" + d.symbol}>{standard}</div>
              </MDBCol>
              <MDBCol size="1">
                <div className={account == 'Pro' ? "spWRowTPro spSelect" : "spWRowTPro"}>Prime</div>
                <div className="spWRowVPro" id={"pro_" + d.symbol}>{pro}</div>
              </MDBCol>        
              <MDBCol size="1">
                <div className={account == 'ECN' ? "spWRowTECN spSelect" : "spWRowTECN"}>ECN</div>
                <div className="spWRowVECN" id={"ecn_" + d.symbol}>{ecn}</div>
              </MDBCol>
            </MDBRow>
          </MDBRow>}
          content={
            <MDBRow center>
              <MDBCol size="2">
                &nbsp;
              </MDBCol>
              <MDBCol size="8">
                <MDBCard style={{borderRadius: 0}}>
                  <MDBCardBody>
                      <MDBCardTitle  className="spWRowName">{name}</MDBCardTitle>
                      <MDBCardText>
                        <MDBRow center>
                          <MDBCol size="8">
                            <MDBRow center>
                              <MDBCol size="6">Margin requirement</MDBCol>
                              <MDBCol size="6">{volStep}%</MDBCol>
                            </MDBRow>
                            <MDBRow center>
                              <MDBCol size="6">Min. transaction size</MDBCol>
                              <MDBCol size="6">{volMin}</MDBCol>
                            </MDBRow>
                            <MDBRow center>
                              <MDBCol size="6">Max. transaction size</MDBCol>
                              <MDBCol size="6">{volMax}</MDBCol>
                            </MDBRow>
                          </MDBCol>
                          <MDBCol size="4">
                              <MDBBtn className="btnSelected" onClick={() => {
                                window.open('https://www.example.com/login');
                              }}>Trade</MDBBtn>            
                          </MDBCol>
                         </MDBRow> 
                      </MDBCardText>
                    </MDBCardBody>
                </MDBCard>
              </MDBCol>
              <MDBCol size="2">
                &nbsp;
              </MDBCol>
            </MDBRow>
          }
        />
        }
      })}
      </div>
  </MDBRow>}
</MDBContainer>;
}

export default App;
