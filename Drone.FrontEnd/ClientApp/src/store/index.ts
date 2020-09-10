import { createStore, combineReducers, applyMiddleware } from "redux";
import { composeWithDevTools } from "redux-devtools-extension";
import orientation from "./orientation/reducer";
import motors from "./motors/reducer";
import gps from "./gps/reducer";
import notifications from "./notifications/reducer";
import thunkMiddleware from "redux-thunk";

const rootReducer = combineReducers({
  orientation,
  motors,
  gps,
  notifications,
});

export type AppState = ReturnType<typeof rootReducer>;

export function getStore() {
  const middlewares = [thunkMiddleware];
  const middleWareEnhancer = applyMiddleware(...middlewares);

  return createStore(rootReducer, composeWithDevTools(middleWareEnhancer));
}
