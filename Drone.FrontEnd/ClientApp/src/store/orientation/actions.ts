import { SET_ORIENTATION, SET_SENSOR, SET_ASSIST, SET_ORIENTATION_HANDLER, OrientationHandlerState, SET_ASSIST_RATE } from "./types";
import axios from "axios";
import { getAuthenticationHash } from "../../utilities/authenticationHash";
import { config } from "../../config";
import { Axis } from "../../enums/Axis";
import { addHttpErrorNotification, addNotification } from "../notifications/actions";

export const requestOrientation = () => {
  return async (dispatch: any, getState: any) => {
    try {
      const url = `${config.baseUrl}/orientation`;
      const result = await axios.get(url, {
        headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
      });

      dispatch({
        type: SET_ORIENTATION,
        value: result.data,
      });
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
};

export const requestSensorState = () => {
  return async (dispatch: any, getState: any) => {
    try {
      const url = `${config.baseUrl}/orientation/sensor`;
      const result = await axios.get(url, {
        headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
      });
     
      dispatch({
        type: SET_SENSOR,
        value: result.data.value,
      });
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
};

export const setSensorState = (state: boolean) => {
  return async (dispatch: any, getState: any) => {
    try {
      const url = `${config.baseUrl}/orientation/sensor`;
      const result = state ? 
        await axios.post(url, {}, {
            headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
        }) : 
        await axios.delete(url, {
            headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
        });

      dispatch({
        type: SET_SENSOR,
        value: state,
      });
      addNotification(result.data.message)(dispatch);
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
};

export const requestAssistState = () => {
  return async (dispatch: any, getState: any) => {
    try {
      const url = `${config.baseUrl}/orientation/assist`;
      const result = await axios.get(url, {
        headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
    });

      dispatch({
        type: SET_ASSIST,
        value: result.data.value,
      });
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
};

export const setAssistState = (state: boolean) => {
  return async (dispatch: any, getState: any) => {
    try {
      const url = `${config.baseUrl}/orientation/assist`;
      const result = state ? 
        await axios.post(url, {}, {
            headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
        }) : 
        await axios.delete(url, {
            headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
        });

      dispatch({
        type: SET_ASSIST,
        value: state,
      });
      addNotification(result.data.message)(dispatch);
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
};

export const requestAssistRate = () => {
  return async (dispatch: any, getState: any) => {
    try {
      const url = `${config.baseUrl}/orientation/assist/rate`;
      const result = await axios.get(url, {
        headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
    });

      dispatch({
        type: SET_ASSIST_RATE,
        value: result.data.value,
      });
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
};

export const setAssistRate = (rate: number) => {
  return async (dispatch: any, getState: any) => {
    try {
      const url = `${config.baseUrl}/orientation/assist/rate`;
      const result = await axios.post(url, {
          value: rate
        }, {
            headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
        });

      dispatch({
        type: SET_ASSIST_RATE,
        value: rate,
      });
      addNotification(result.data.message)(dispatch);
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
};

export const requestOrientationHandler = (axis: Axis) => {
    return async (dispatch: any, getState: any) => {
      try {
        const url = `${config.baseUrl}/orientation/assist/handler/${Axis[axis]}`;
        const result = await axios.get(url, {
          headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
        });
  
        dispatch({
          type: SET_ORIENTATION_HANDLER,
          value: {
            axis: result.data.axis,
            agression: Number(result.data.agression ?? 0),

            isQLearning: result.data.isQLearning,            

            minThrottle: Number(result.data.minThrottle ?? 0),
            maxThrottle: Number(result.data.maxThrottle ?? 0),
            throttleIncrement: Number(result.data.throttleIncrement ?? 0)
          },
        });
      } catch (error) {
        addHttpErrorNotification(error)(dispatch);
      }
    };
}

export const setOrientationHandler = (orientationHandler: OrientationHandlerState) => {
    return async (dispatch: any, getState: any) => {
      try {
        const url = `${config.baseUrl}/orientation/assist/handler/${Axis[orientationHandler.axis]}`;
        const result = await axios.post(url, orientationHandler, {
          headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
        });
  
        dispatch({
          type: SET_ORIENTATION_HANDLER,
          value: orientationHandler,
        });
        addNotification(result.data.message)(dispatch);
      } catch (error) {
        addHttpErrorNotification(error)(dispatch);
      }
    };
}

export const downloadOrientationHandlerConfiguration = (axis: Axis) => {
    return async (dispatch: any, getState: any) => {
      try {
        const url = `${config.baseUrl}/orientation/assist/handler/${Axis[axis]}/json`;
        const result = await axios.get(url, {
          headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
        });
  

        const downloadUrl = window.URL.createObjectURL(new Blob([JSON.stringify(result.data)]));
        const link = document.createElement("a");
        link.href = downloadUrl;
        link.setAttribute("download", `${Axis[axis]}.json`);
        document.body.appendChild(link);
        link.click();
        link.remove();
      } catch (error) {
        addHttpErrorNotification(error)(dispatch);
      }
    };
}


export const uploadOrientationHandlerConfiguration = (axis: Axis, json: string) => {
  return async (dispatch: any, getState: any) => {
    try {
      const url = `${config.baseUrl}/orientation/assist/handler/${Axis[axis]}/json`;
      const result = await axios.post(url, JSON.parse(json), {
        headers: { Authorization: `Bearer ${getAuthenticationHash(config.authenticationKey)}` },
      });
      
      addNotification(result.data.message)(dispatch);
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
}
