import { call, put, takeLatest } from 'redux-saga/effects';
import {
  GET_SPREAD, GET_SPREAD_FAILURE, GET_SPREAD_SUCCESS
} from '../constant/spread';
import { ACCOUNT_CODE, API_URL } from '../constant/url';

//
// SPREAD
//
function* getSpread() {
    const spreadLink = `${API_URL}/start/${ACCOUNT_CODE}`;
    try {
      const res = yield call(fetch, spreadLink, {
        method: 'GET'
      });
      const data = yield call([res, 'json']);
    yield put({
        type: GET_SPREAD_SUCCESS,
        data: data,
    });
    } catch (error) {
      yield put({
        type: GET_SPREAD_FAILURE,
        error,
      });
    }
  }
  
export function* watchSpread() {
    yield takeLatest(GET_SPREAD, getSpread);
  }