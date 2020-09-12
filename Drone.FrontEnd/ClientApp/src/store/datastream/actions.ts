import { config } from "../../config";
import { addHttpErrorNotification, addNotification} from "../notifications/actions";
import { SET_GPS_POSITION } from "../gps/types";
import { SET_MOTOR_THROTTLES } from "../motors/types";
import { SET_ORIENTATION } from "../orientation/types";
import { NotificationType } from "../notifications/types";

enum MessageType {
  Throttles,
  Orientation,
  Position
}

let socket: WebSocket | null;
let socketDispatch: any;

function openSocket() {
  socket = new WebSocket(config.baseUrl.replace("http", "ws"), "drone")

  socket.addEventListener("open", handleSocketOpen)
  socket.addEventListener("error", console.error);
  socket.addEventListener("message", handleSocketMessage)
  socket.addEventListener("close", handleSocketClose);
}

function handleSocketOpen() {
  console.log("WebSocket opened");
  addNotification("Data stream connected", NotificationType.Success)(socketDispatch);
}

async function handleSocketMessage(event: MessageEvent) {
  const buffer = await event.data.arrayBuffer();
  const view = new DataView(buffer);

  var type = view.getUint8(0);
  switch (type) {
    case MessageType.Throttles:
      handleThrottlesMessage(view);
      break;
    case MessageType.Orientation:
      handleOrientationMessage(view);
      break;
    case MessageType.Position:
      handlePositionMessage(view);
     break;
  }
}

function handleThrottlesMessage(view: DataView) {
  const value = {
    frontLeft: view.getFloat32(1, true),
    frontRight: view.getFloat32(5, true),
    rearLeft: view.getFloat32(9, true),
    rearRight: view.getFloat32(13, true)
  };
  socketDispatch({
    type: SET_MOTOR_THROTTLES,
    value: value
  });
}

function handleOrientationMessage(view: DataView) {
  const value = {
    yaw: view.getFloat32(1, true),
    pitch: view.getFloat32(5, true),
    roll: view.getFloat32(9, true)
  };
  socketDispatch({
    type: SET_ORIENTATION,
    value: value
  });
}

function handlePositionMessage(view: DataView) {
  const value = {
    longitude: view.getFloat32(1, true),
    latitude: view.getFloat32(5, true)
  };
  socketDispatch({
    type: SET_GPS_POSITION,
    value: value
  });
}

function handleSocketClose(event: CloseEvent) {
  socket = null;
  addNotification("Data stream disconnected", NotificationType.Error)(socketDispatch);
  openSocket();
}

export const requestDataStream = () => {
  return async (dispatch: any, getState: any) => {
    try {
      if (socket == null){
        socketDispatch = dispatch;
        openSocket();
      }
    } catch (error) {
      addHttpErrorNotification(error)(dispatch);
    }
  };
};