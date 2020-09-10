
import * as React from "react";
import "./notification.scss"
import { Notification as NotificationModel, NotificationType } from "../../store/notifications/types"

type Props = {
  notification: NotificationModel;
};

type State = {};

export class Notification extends React.Component<Props, State> {

  render() {
    return (
      <div className={(`Notification Notification-${NotificationType[this.props.notification.type]}`)}>
        <div className="Notification-message">{this.props.notification.message}</div>
      </div>
    );
  }
}
