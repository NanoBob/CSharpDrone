import {
  OrientationState,
  SET_ORIENTATION,
  SET_SENSOR,
  SET_ASSIST,
  SET_ORIENTATION_HANDLER,
} from "./types";
import { Axis } from "../../enums/Axis";

const orientationReducer = (
  state: OrientationState | undefined,
  action: any
): OrientationState => {
  if (state === undefined) {
    return {
      isSensorEnabled: false,
      isAssistEnabled: false,

      orientationHandlers: [
        {
            axis: Axis.Yaw,
            isQLearning: false,
            agression: 0,
            throttleIncrement: 0,
            minThrottle: 0,
            maxThrottle: 0
        },
        {
            axis: Axis.Pitch,
            isQLearning: false,
            agression: 0,
            throttleIncrement: 0,
            minThrottle: 0,
            maxThrottle: 0
        },
        {
            axis: Axis.Roll,
            isQLearning: false,
            agression: 0,
            throttleIncrement: 0,
            minThrottle: 0,
            maxThrottle: 0
        },
      ],

      orientation: {
        yaw: 0,
        pitch: 0,
        roll: 0,
      },
    };
  }
  switch (action.type) {
    case SET_ORIENTATION:
      return {
        ...state,
        orientation: action.value,
      };
    case SET_SENSOR:
      return {
        ...state,
        isSensorEnabled: action.value,
      };
    case SET_ASSIST:
      return {
        ...state,
        isAssistEnabled: action.value,
      };
    case SET_ORIENTATION_HANDLER:
      return {
        ...state,
        orientationHandlers: [
            ...state.orientationHandlers.filter(h => h.axis !== action.value.axis),
            action.value
        ].sort((a, b) => a.axis - b.axis)
      };
    default:
      return state;
  }
};

export default orientationReducer;
