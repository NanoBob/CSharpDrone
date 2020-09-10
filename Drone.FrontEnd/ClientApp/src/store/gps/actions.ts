import { SET_GPS, SET_GPS_POSITION } from "./types";
import axios from "axios";
import { getAuthenticationHash } from "../../utilities/authenticationHash";
import { config } from "../../config";
import { addHttpErrorNotification, addNotification } from "../notifications/actions";

export const requestGpsState = () => {
  return async (dispatch: any, getState: any) => {
    try {
      const url = `${config.baseUrl}/gps`;
      const result = await axios.get(url, {
        headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
      });
     
      dispatch({
        type: SET_GPS,
        value: result.data.value,
      });
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
};

export const setGpsState = (state: boolean) => {
  return async (dispatch: any, getState: any) => {
    try {
      const url = `${config.baseUrl}/gps`;
      const result = state ? 
        await axios.post(url, {}, {
            headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
        }) : 
        await axios.delete(url, {
            headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
        });

      dispatch({
        type: SET_GPS,
        value: state,
      });
      addNotification(result.data.message)(dispatch);
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
};

export const requestPosition = () => {
  return async (dispatch: any, getState: any) => {
    try {
      const url = `${config.baseUrl}/gps/position`;
      const result = await axios.get(url, {
        headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
      });
     
      dispatch({
        type: SET_GPS_POSITION,
        value: result.data,
      });
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
};