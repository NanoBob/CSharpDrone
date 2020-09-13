import * as React from "react";
import { Panel } from "../panel/Panel";
import { Toggle } from "../toggle/Toggle";
import "./generalSettings.scss";
import { AppState } from "../../store";
import { connect } from "react-redux";
import { MotorsState } from "../../store/motors/types";
import { requestMotorsState, setMotorsState } from "../../store/motors/actions";
import { GpsState } from "../../store/gps/types";
import { requestGpsState, setGpsState } from "../../store/gps/actions";

type Props = {
    requestMotorsState: any,
    setMotorsState: any,
    motorsState: MotorsState,

    requestGpsState: any,
    setGpsState: any,
    gpsState: GpsState
};

type State = {};

export class GeneralSettings extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {};
    }    

    public render() {
        return <Panel title="general settings">
            <div className="general-settings-wrapper">
                <div className="general-settings-toggles">
                    <Toggle label="Motors" value={this.props.motorsState.areMotorsEnabled} onToggle={(state) => this.props.setMotorsState(state)}></Toggle>
                    <Toggle label="Gps" value={this.props.gpsState.isGpsEnabled} onToggle={(state) => this.props.setGpsState(state)}></Toggle>
                </div>
            </div>
        </Panel>
    }
}

export default connect(
    (state: AppState) => {
      return {
        motorsState: state.motors,
        gpsState: state.gps
      }
    },
    { requestMotorsState, setMotorsState, requestGpsState, setGpsState },
  )(GeneralSettings)
  