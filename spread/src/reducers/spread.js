import {
  GET_SPREAD, GET_SPREAD_FAILURE, GET_SPREAD_SUCCESS
} from '../constant/spread';
  
  const initialState = {
    loading: false,
    data: [],
    error: null,
  };
  
  const spreadReducer = (state = initialState, { type, data, error }) => {
    switch (type) {
      //
      // SPREAd
      //
      case GET_SPREAD: {
        return {
          ...state,
          loading: true
        };
      }
      case GET_SPREAD_SUCCESS: {
        return {
          ...state,
          loading: false,
          data: data
        };
      }
      case GET_SPREAD_FAILURE: {
        return {
          ...state,
          loading: false,
          error,
        };
      }
      default: {
        return state;
      }
    }
  };
  
  export default spreadReducer;
  