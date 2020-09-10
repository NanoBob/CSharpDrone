import { NotificationState, NotificationAction, PUSH_NOTIFICATION, POP_NOTIFICATION } from "./types";

const notificationReducer = (state: NotificationState | undefined, action: NotificationAction): NotificationState => {
  if (state === undefined) {
    return { notifications: [] };
  }
  switch (action.type) {
    case PUSH_NOTIFICATION:
      return {
        notifications: [...state.notifications, action.notification]
      };
    case POP_NOTIFICATION:
      return {
        notifications: state.notifications.slice(1, state.notifications.length)
      };
    default:
      return state;
  }
};

export default notificationReducer;
