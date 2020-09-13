import * as React from "react";
import { Panel } from "../panel/Panel";
import "./orientationReadout.scss";
import { AppState } from "../../store";
import { requestOrientation, setSensorState, setAssistState, requestSensorState, requestAssistState, requestOrientationHandler } from "../../store/orientation/actions";
import { connect } from "react-redux";
import { OrientationState } from "../../store/orientation/types";

type Props = {
    requestOrientation: any,
    requestSensorState: any,
    requestAssistState: any,
    setSensorState: any,
    setAssistState: any,
    requestOrientationHandler: any,
    orientationState: OrientationState
};

type State = {};

export class OrientationReadout extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {};

        this.props.requestOrientation()
        // setInterval(() => this.props.requestOrientation(), 250);
    }    

    public render() {
        return <Panel title="Orientation readout">
            <div className="orientation-readout">
                <div className="orientation-axis">
                    <span className="axis-value">{Math.round(this.props.orientationState.orientation.yaw * 100) / 100}</span>
                    <span className="axis-label">Yaw</span>
                </div>
                <div className="orientation-axis">
                    <span className="axis-value">{Math.round(this.props.orientationState.orientation.pitch * 100) / 100}</span>
                    <span className="axis-label">Pitch</span>
                </div>
                <div className="orientation-axis">
                    <span className="axis-value">{Math.round(this.props.orientationState.orientation.roll * 100) / 100}</span>
                    <span className="axis-label">Roll</span>
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
    { requestOrientation, requestSensorState, requestAssistState, setSensorState, setAssistState, requestOrientationHandler },
  )(OrientationReadout)
  