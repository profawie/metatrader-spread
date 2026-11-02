import { all, fork } from 'redux-saga/effects';
import { watchSpread } from './spread';

export function* rootSaga() {
    yield all([
      fork(watchSpread),
    ]);
  }
  