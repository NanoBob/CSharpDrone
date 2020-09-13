export enum MessageType {
  Throttles,
  Orientation,
  Position,
  Flags,
  Authorization
}

export enum DroneFlags {
  MotorsEnabled = 0x01,
  GpsEnabled = 0x02,
  OrientationSensorEnabled = 0x04,
  OrientationAssistEnabled = 0x08,
}
