export const PUSH_NOTIFICATION = "Notifications.Push"
export const POP_NOTIFICATION = "Notifications.Pop"

export enum NotificationType {
    Success,
    Information,
    Warning,
    Error,
}

export type Notification = {
    id: string;
    message: string;
    type: NotificationType;
}

export type NotificationState = {
    notifications: Notification[]
}

export type NotificationAction = {
  type: string;
  notification: Notification;
};
