import * as React from "react";
import { Panel } from "../panel/Panel";
import { Toggle } from "../toggle/Toggle";
import { OrientationHandler } from "./orientationHandler/OrientationHandler";
import "./orientationSettings.scss";
import { AppState } from "../../store";
import { requestOrientation, setSensorState, setAssistState, requestSensorState, requestAssistState, requestOrientationHandler, setOrientationHandler } from "../../store/orientation/actions";
import { connect } from "react-redux";
import { OrientationState } from "../../store/orientation/types";
import { Axis } from "../../enums/Axis";

type Props = {
    requestOrientation: any,
    requestSensorState: any,
    requestAssistState: any,
    setSensorState: any,
    setAssistState: any,
    requestOrientationHandler: any,
    setOrientationHandler: any,
    orientationState: OrientationState
};

type State = {
};

export class OrientationSettings extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {};

        this.props.requestSensorState()
        this.props.requestAssistState()
        this.props.requestOrientationHandler(Axis.Yaw);
        this.props.requestOrientationHandler(Axis.Pitch);
        this.props.requestOrientationHandler(Axis.Roll);
    }    

    public render() {
        return <Panel title="Orientation settings">
            <div className="orientation-settings-wrapper">
                <div className="orientation-toggles">
                    <Toggle label="Sensor" value={this.props.orientationState.isSensorEnabled} onToggle={(state) => this.props.setSensorState(state)}></Toggle>
                    <Toggle label="Assist" value={this.props.orientationState.isAssistEnabled} onToggle={this.props.setAssistState}></Toggle>
                </div>
                <div className="orientation-handlers">
                    <OrientationHandler 
                        orientationHandler={this.props.orientationState.orientationHandlers[Axis.Yaw]}
                        updateOrientationHandler={(handler) => this.props.setOrientationHandler(handler)}
                    />
                    <OrientationHandler 
                        orientationHandler={this.props.orientationState.orientationHandlers[Axis.Pitch]}
                        updateOrientationHandler={(handler) => this.props.setOrientationHandler(handler)}
                    />
                    <OrientationHandler 
                        orientationHandler={this.props.orientationState.orientationHandlers[Axis.Roll]}
                        updateOrientationHandler={(handler) => this.props.setOrientationHandler(handler)}
                    />
                </div>
            </div>
        </Panel>
    }
}

export default connect(
    (state: AppState) => {
      return {
        orientationState: state.orientation
      }
    },
    { requestOrientation, requestSensorState, requestAssistState, setSensorState, setAssistState, requestOrientationHandler, setOrientationHandler },
  )(OrientationSettings)
  