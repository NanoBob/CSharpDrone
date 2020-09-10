export const SET_GPS = "Gps.Set"
export const SET_GPS_POSITION = "Gps.Position.Set"

export type Position = {
    longitude: number,
    latitude: number
}

export type GpsState = {
    isGpsEnabled: boolean,
    position: Position
}