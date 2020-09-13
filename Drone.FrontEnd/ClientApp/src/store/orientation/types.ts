import { Axis } from "../../enums/Axis";

export const SET_ORIENTATION = "Orientation.Set"
export const SET_SENSOR = "Orientation.Sensor.Set"
export const SET_ASSIST = "Orientation.Assist.Set"
export const SET_ASSIST_RATE = "Orientation.Assist.Rate.Set"
export const SET_ORIENTATION_HANDLER = "Orientation.Assist.Handler.Set"

export type Orientation = {
    yaw: number;
    pitch: number;
    roll: number;
}

export type OrientationHandlerState = {
    axis: Axis,

    agression: number,
    isQLearning: boolean,
    minThrottle: number,
    maxThrottle: number,
    throttleIncrement: number
}

export type OrientationState = {
    isSensorEnabled: boolean,
    isAssistEnabled: boolean,

    orientationHandlers: OrientationHandlerState[];

    orientation: Orientation;

    assistRate: number;
}