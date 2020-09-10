import { PUSH_NOTIFICATION, POP_NOTIFICATION, NotificationType } from "./types";

let notificationId = 0;

export const addNotification = (message: string, type: NotificationType = NotificationType.Success) => {
  return (dispatch: any) => {
    dispatch({
      type: PUSH_NOTIFICATION,
      notification: {
        id: notificationId++,
        message: message,
        type: type
      }
    });

    setTimeout(() => {
      dispatch(removeNotification());
    }, 3000);
  };
};

export const addHttpErrorNotification = (error: Error) => {
  return (dispatch: any) => {
    addNotification(error.message, NotificationType.Error)(dispatch);
  };
};

export const removeNotification = () => {
  return {
    type: POP_NOTIFICATION
  };
};
