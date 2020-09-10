import {
  MotorsState,
  SET_MOTORS,
  SET_MOTOR_THROTTLES
} from "./types";

const orientationReducer = (
  state: MotorsState | undefined,
  action: any
): MotorsState => {
  if (state === undefined) {
    return {
      areMotorsEnabled: false,
      throttles: {
        frontLeft: 0,
        frontRight: 0,
        rearLeft: 0,
        rearRight: 0
      }
    };
  }
  switch (action.type) {
    case SET_MOTORS:
      return {
        ...state,
        areMotorsEnabled: action.value,
      };
    case SET_MOTOR_THROTTLES:
      return {
        ...state,
        throttles: action.value,
      };
    default:
      return state;
  }
};

export default orientationReducer;
