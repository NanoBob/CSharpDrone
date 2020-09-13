import React from 'react';
import "./scss/body.scss";
import "./scss/layout.scss";
import GeneralSettings from "./components/generalSettings/GeneralSettings";
import OrientationSettings from "./components/orientationSettings/OrientationSettings";
import OrientationReadout from "./components/orientationReadout/OrientationReadout";
import MotorsReadout from "./components/motorsReadout/MotorsReadout";
import NotificationContainer from "./components/notifications/NotificationContainer";
import MotorControls from "./components/motorControls/MotorControls";
import Map from "./components/map/Map";
import { AppState } from "./store";
import { requestDataStream } from "./store/datastream/actions";
import { setMotorsState } from "./store/motors/actions";
import { connect } from "react-redux";

type Props = {
	requestDataStream: any,
	setMotorsState: any
};

type State = {};

export class App extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {};
	}

	componentDidMount() {
		this.props.requestDataStream()
		
		document.body.addEventListener("keydown", (event) => {
			
			if (event.key === "Delete"){
				this.props.setMotorsState(false);
			}
		})
	}

    public render() {
		return (
			<div className="app">
				<div>
					<div className="general-settings">
						<GeneralSettings></GeneralSettings>
					</div>
					<div className="orientation-settings">
						<OrientationSettings></OrientationSettings>
					</div>
					<div className="motor-controls">
						<MotorControls></MotorControls>
					</div>
				</div>
				<div>
					<div className="map">
						<Map></Map>
						<div className="below-map">
							<div>
								<OrientationReadout></OrientationReadout>
							</div>
							<div>
								<MotorsReadout></MotorsReadout>
							</div>
						</div>
					</div>
				</div>
				<NotificationContainer></NotificationContainer>
			</div>
		);
	}
};

export default connect(
    (state: AppState) => {
      return {
		  
      }
    },
    { requestDataStream, setMotorsState },
  )(App)
