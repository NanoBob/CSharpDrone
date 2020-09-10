import { SET_MOTORS, SET_MOTOR_THROTTLES } from "./types";
import axios from "axios";
import { getAuthenticationHash } from "../../utilities/authenticationHash";
import { config } from "../../config";
import { addHttpErrorNotification, addNotification } from "../notifications/actions";

export const requestMotorsState = () => {
  return async (dispatch: any, getState: any) => {
    try {
      const url = `${config.baseUrl}/motors`;
      const result = await axios.get(url, {
        headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
      });
     
      dispatch({
        type: SET_MOTORS,
        value: result.data.value,
      });
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
};

export const setMotorsState = (state: boolean) => {
  return async (dispatch: any, getState: any) => {
    try {
      const url = `${config.baseUrl}/motors`;
      const result = state ? 
        await axios.post(url, {}, {
            headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
        }) : 
        await axios.delete(url, {
            headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
        });

      dispatch({
        type: SET_MOTORS,
        value: state,
      });
      
      addNotification(result.data.message)(dispatch);
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
};

export const requestThrottles = () => {
  return async (dispatch: any, getState: any) => {
    try {
      const url = `${config.baseUrl}/motors/throttles`;
      const result = await axios.get(url, {
        headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
      });
     
      dispatch({
        type: SET_MOTOR_THROTTLES,
        value: result.data,
      });
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
};

export const setThrottle = (throttle: number) => {
  return async (dispatch: any, getState: any) => {
    try {
      const url = `${config.baseUrl}/motors/throttle`;
      const result = await axios.post(url, {
        value: throttle
      }, {
        headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
      });
     
      addNotification(result.data.message)(dispatch);
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
};