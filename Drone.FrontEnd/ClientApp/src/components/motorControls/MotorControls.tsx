import * as React from "react";
import { Panel } from "../panel/Panel";
import "./motorControls.scss";
import { AppState } from "../../store";
import { requestMotorsState, requestThrottles, setThrottle } from "../../store/motors/actions";
import { connect } from "react-redux";
import { MotorsState } from "../../store/motors/types";
import { DroneInput } from "../droneInput/DroneInput";

type Props = {
    requestMotorsState: any,
    requestThrottles: any,
    setThrottle: any,
    motorsState: MotorsState
};

type State = {
    desiredThrottle: number
};

export class MotorControls extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {
            desiredThrottle: 0
        };
    }    

    setDesiredThrottle(value: number) {
        this.setState({
            desiredThrottle: value
        });
        this.props.setThrottle(value / 100);
    }

    public render() {
        return <Panel title="Motors">
            <DroneInput type="number" value={this.state.desiredThrottle} onChange={(value) => this.setDesiredThrottle(value.target.value)} label="Throttle"></DroneInput>
        </Panel>
    }
}

export default connect(
    (state: AppState) => {
      return {
        motorsState: state.motors
      }
    },
    { requestMotorsState, requestThrottles, setThrottle },
  )(MotorControls)
  