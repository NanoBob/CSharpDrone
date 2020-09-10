import React from 'react';
import "./scss/body.scss";
import "./scss/layout.scss";
import GeneralSettings from "./components/generalSettings/GeneralSettings";
import OrientationSettings from "./components/orientationSettings/OrientationSettings";
import OrientationReadout from "./components/orientationReadout/OrientationReadout";
import MotorsReadout from "./components/motorsReadout/MotorsReadout";
import NotificationContainer from "./components/notifications/NotificationContainer";
import MotorControls from "./components/motorControls/MotorControls";
import { Panel } from "./components/panel/Panel";
import Map from "./components/map/Map";

const App: React.FC = () => {
	return (
		<div className="app">
			<div>
				<div className="general-settings">
					<GeneralSettings></GeneralSettings>
				</div>
				<div className="orientation-settings">
					<OrientationSettings></OrientationSettings>
				</div>
				<div className="orientation-readout">
					<OrientationReadout></OrientationReadout>
				</div>
				<div className="motors-readout">
					<MotorsReadout></MotorsReadout>
				</div>
			</div>
			<div>
				<div className="map">
					<Map></Map>
					<div className="below-map">
						<div>
							<MotorControls></MotorControls>
							{/* <Panel title="TEST 2"></Panel> */}
						</div>
						<div>
							{/* 
							<Panel title="TEST 3"></Panel>
							<Panel title="TEST 4"></Panel> 
							*/}
						</div>
					</div>
				</div>
			</div>
			<NotificationContainer></NotificationContainer>
		</div>
	);
};

export default App;
