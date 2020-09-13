import * as React from "react";
import { Panel } from "../panel/Panel";
import { Toggle } from "../toggle/Toggle";
import { DroneSelectInput } from "../droneSelectInput/DroneSelectInput";
import { OrientationHandler } from "./orientationHandler/OrientationHandler";
import "./orientationSettings.scss";
import { AppState } from "../../store";
import { 
    requestOrientation, 
    setSensorState, 
    setAssistState, 
    requestSensorState, 
    requestAssistState, 
    requestOrientationHandler, 
    setOrientationHandler, 
    requestAssistRate, 
    setAssistRate,
    downloadOrientationHandlerConfiguration,
    uploadOrientationHandlerConfiguration
} from "../../store/orientation/actions";
import { connect } from "react-redux";
import { OrientationState } from "../../store/orientation/types";
import { Axis } from "../../enums/Axis";

type Props = {
    requestOrientation: any,
    requestSensorState: any,
    requestAssistState: any,
    requestAssistRate: any,
    requestOrientationHandler: any,
    setSensorState: any,
    setAssistState: any,
    setAssistRate: any,
    setOrientationHandler: any,
    downloadOrientationHandlerConfiguration: any,
    uploadOrientationHandlerConfiguration: any,
    orientationState: OrientationState
};

type State = {
};

export class OrientationSettings extends React.Component<Props, State> {
    private assistRates = [
        { label: "100Hz", value: 1 },
        { label: "50Hz", value: 2 },
        // { label: "33Hz", value: 3 },
        { label: "25Hz", value: 4 },
        { label: "20Hz", value: 5 },
        // { label: "17Hz", value: 6 },
        // { label: "14Hz", value: 7 },
        // { label: "13Hz", value: 8 },
        // { label: "11Hz", value: 9 },
        { label: "10Hz", value: 10 },
        { label: "5Hz", value: 20 },
        { label: "2Hz", value: 50 },
        { label: "1Hz", value: 100 },
    ]

    constructor(props: Props) {
        super(props);
        this.state = {};

        this.props.requestAssistRate();
        this.props.requestOrientationHandler(Axis.Yaw);
        this.props.requestOrientationHandler(Axis.Pitch);
        this.props.requestOrientationHandler(Axis.Roll);
    }

    public render() {
        return <Panel title="Orientation settings">
            <div className="orientation-toggles-wrapper">
                <div className="orientation-toggles">
                    <Toggle label="Sensor" value={this.props.orientationState.isSensorEnabled} onToggle={(state) => this.props.setSensorState(state)}></Toggle>
                    <Toggle label="Assist" value={this.props.orientationState.isAssistEnabled} onToggle={this.props.setAssistState}></Toggle>
                </div>
                <div className="assist-rate-wrapper">
                    <DroneSelectInput value={this.props.orientationState.assistRate} items={this.assistRates} label="Assist rate" onChange={this.props.setAssistRate}></DroneSelectInput>
                </div>
            </div>                
            <div className="orientation-handlers-wrapper">
                <div className="orientation-handlers">
                    <OrientationHandler 
                        orientationHandler={this.props.orientationState.orientationHandlers[Axis.Yaw]}
                        updateOrientationHandler={(handler) => this.props.setOrientationHandler(handler)}
                        requestDownload={() => this.props.downloadOrientationHandlerConfiguration(Axis.Yaw)}
                        requestUpload={(content) => this.props.uploadOrientationHandlerConfiguration(Axis.Yaw, content)}
                    />
                    <OrientationHandler 
                        orientationHandler={this.props.orientationState.orientationHandlers[Axis.Pitch]}
                        updateOrientationHandler={(handler) => this.props.setOrientationHandler(handler)}
                        requestDownload={() => this.props.downloadOrientationHandlerConfiguration(Axis.Pitch)}
                        requestUpload={(content) => this.props.uploadOrientationHandlerConfiguration(Axis.Pitch, content)}
                    />
                    <OrientationHandler 
                        orientationHandler={this.props.orientationState.orientationHandlers[Axis.Roll]}
                        updateOrientationHandler={(handler) => this.props.setOrientationHandler(handler)}
                        requestDownload={() => this.props.downloadOrientationHandlerConfiguration(Axis.Roll)}
                        requestUpload={(content) => this.props.uploadOrientationHandlerConfiguration(Axis.Roll, content)}
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
    { 
        requestOrientation, 
        requestSensorState, 
        requestAssistState, 
        setSensorState, 
        setAssistState, 
        requestOrientationHandler, 
        setOrientationHandler, 
        requestAssistRate, 
        setAssistRate,
        downloadOrientationHandlerConfiguration,
        uploadOrientationHandlerConfiguration
    },
  )(OrientationSettings)
  