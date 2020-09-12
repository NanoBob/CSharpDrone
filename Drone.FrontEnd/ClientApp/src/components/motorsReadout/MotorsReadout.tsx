import * as React from "react";
import { Panel } from "../panel/Panel";
import "./motorsReadout.scss";
import { AppState } from "../../store";
import { requestMotorsState, requestThrottles } from "../../store/motors/actions";
import { connect } from "react-redux";
import { MotorsState } from "../../store/motors/types";

type Props = {
    requestMotorsState: any,
    requestThrottles: any,
    motorsState: MotorsState
};

type State = {
};

export class MotorsReadout extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {};

        this.props.requestThrottles()
        // setInterval(() => this.props.requestThrottles(), 250);
    }    

    public render() {
        return <Panel title="Motors readout">
            <div className="motors-readout">
                <div className="motor-readout">
                    <span className="motor-value">
                        { Math.round(this.props.motorsState.throttles?.frontLeft * 1000) / 10}%
                    </span>
                    <span className="motor-label">1</span>
                </div>
                <div className="motor-readout">
                    <span className="motor-value">
                        { Math.round(this.props.motorsState.throttles?.frontRight * 1000) / 10}%
                    </span>
                    <span className="motor-label">2</span>
                </div>
                <div className="motor-readout">
                    <span className="motor-value">
                        { Math.round(this.props.motorsState.throttles?.rearLeft * 1000) / 10}%
                    </span>
                    <span className="motor-label">3</span>
                </div>
                <div className="motor-readout">
                    <span className="motor-value">
                        { Math.round(this.props.motorsState.throttles?.rearRight * 1000) / 10}%
                    </span>
                    <span className="motor-label">4</span>
                </div>
            </div>
        </Panel>
    }
}

export default connect(
    (state: AppState) => {
      return {
        motorsState: state.motors
      }
    },
    { requestMotorsState, requestThrottles },
  )(MotorsReadout)
  