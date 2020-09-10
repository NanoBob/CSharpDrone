import {
  GpsState,
  SET_GPS,
  SET_GPS_POSITION
} from "./types";

const orientationReducer = (
  state: GpsState | undefined,
  action: any
): GpsState => {
  if (state === undefined) {
    return {
      isGpsEnabled: false,
      position: {
        longitude: 0,
        latitude: 0
      }
    };
  }
  switch (action.type) {
    case SET_GPS:
      return {
        ...state,
        isGpsEnabled: action.value,
      };
    case SET_GPS_POSITION:
      return {
        ...state,
        position: action.value,
      };
    default:
      return state;
  }
};

export default orientationReducer;
