
import * as React from "react";
import "./notificationContainer.scss"
import { NotificationState } from "../../store/notifications/types";
import { Notification } from "./Notification"
import { AppState } from "../../store";
import { connect } from "react-redux";

type Props = {
  notifications: NotificationState
};

type State = {};

export class NotificationContainer extends React.Component<Props, State> {

  getNotificationStackTop() {
    const length = this.props.notifications.notifications.length;
    return this.props.notifications.notifications.slice(Math.max(length - 5, 0), length)
  }

  render() {
    return (
      <div className="Notification-container-wrapper">
        <div className="Notification-container">
          {this.getNotificationStackTop().map((notification) => 
            <Notification key={notification.id} notification={notification} />
          )}
        </div>
      </div>
    );
  }
}

export default connect(
    (state: AppState) => {
      return {
        notifications: state.notifications
      }
    },
    { },
  )(NotificationContainer)
  