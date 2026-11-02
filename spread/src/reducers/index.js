import { combineReducers } from '@reduxjs/toolkit';
import spreadReducer from './spread';

const rootReducer = combineReducers({
  spread: spreadReducer
});

export default rootReducer;
