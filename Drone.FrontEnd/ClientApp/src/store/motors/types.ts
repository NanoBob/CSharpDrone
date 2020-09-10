export const SET_MOTORS = "Motors.Set"
export const SET_MOTOR_THROTTLES = "Motors.Throttles.Set"

export type MotorsState = {
    areMotorsEnabled: boolean,

    throttles: {
        frontLeft: number,
        frontRight: number,
        rearLeft: number,
        rearRight: number
    }
}