import * as React from "react";
import "./orientationHandler.scss";
import { OrientationHandlerState } from "../../../store/orientation/types";
import { Axis } from "../../../enums/Axis";
import { Toggle } from "../../toggle/Toggle";
import { DroneInput } from "../../droneInput/DroneInput";
import { DroneButton } from "../../droneButton/DroneButton";
import { FileButton } from "../../fileButton/FileButton";

type Props = {
    orientationHandler: OrientationHandlerState,
    updateOrientationHandler: (handler: OrientationHandlerState) => void,
    requestDownload: () => void,
    requestUpload: (content: string) => void,
};

type State = {
    isCollapsed: boolean,
    orientationHandler: OrientationHandlerState
};

export class OrientationHandler extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {
            isCollapsed: true,
            orientationHandler: {
                axis: Axis.Yaw,
                agression: 0,
                isQLearning: false,
                minThrottle: 0,
                maxThrottle: 0,
                throttleIncrement: 0
            }
        };
    }

    toggleBody() {
        if (this.state.isCollapsed){
            this.setState({ orientationHandler: this.props.orientationHandler })
        }
        this.setState({isCollapsed: !this.state.isCollapsed});

    }

    toggleQLearning() {
        this.setState({
            orientationHandler: {
                ...this.state.orientationHandler,
                isQLearning: !this.state.orientationHandler.isQLearning
            }
        })
    }

    updateOrientationhandlerValue(key: string, value: any) {
        this.setState({
            orientationHandler: {
                ...this.state.orientationHandler,
                [key]: value
            }
        })
    }

    submit() {
        this.props.updateOrientationHandler(this.state.orientationHandler);
    }

    downloadHandlerConfiguration() {
        this.props.requestDownload();
    }

    uploadHandlerConfiguration(fileContent: any) {
        this.props.requestUpload(fileContent);
    }

    public render() {
        return <div className="orientation-handler">
            <div className="orientation-handler-header" onClick={() => this.toggleBody()}>
                <span className="orientation-handler-axis-label">
                    { Axis[this.props.orientationHandler.axis] }
                </span>
                <span className="toggle-button">{this.state.isCollapsed ? "▲" : "▼"}</span>
            </div>
            { !this.state.isCollapsed ? 
                <div className="orientation-handler-body">
                    <div className="orientation-handler-type-switch">
                        <Toggle label="Use Q Learning" value={this.state.orientationHandler.isQLearning} onToggle={() => this.toggleQLearning()}></Toggle>
                    </div>
                    { this.state.orientationHandler.isQLearning ? 
                        <div className="orientation-handler-q-learning-options">
                            <DroneInput 
                                label="Minimum throttle"
                                type="number" 
                                step="any" 
                                value={this.state.orientationHandler.minThrottle ?? 0} 
                                onChange={(e) => this.updateOrientationhandlerValue("minThrottle", e.currentTarget.value)} 
                            />
                            <DroneInput 
                                label="Maximum throttle"
                                type="number" 
                                step="any" 
                                value={this.state.orientationHandler.maxThrottle ?? 0} 
                                onChange={(e) => this.updateOrientationhandlerValue("maxThrottle", e.currentTarget.value)} 
                            />
                            <DroneInput 
                                label="Throttle increment"
                                type="number" 
                                step="any" 
                                value={this.state.orientationHandler.throttleIncrement ?? 0} 
                                onChange={(e) => this.updateOrientationhandlerValue("throttleIncrement", e.currentTarget.value)} 
                            />
                        </div>
                        :
                        <div className="orientation-handler-regular-options">
                            <DroneInput 
                                label="Agression"
                                type="number" 
                                step="any" 
                                value={this.state.orientationHandler.agression ?? 0} 
                                onChange={(e) => this.updateOrientationhandlerValue("agression", e.currentTarget.value)} 
                            />
                            <DroneInput 
                                label="Maximum throttle"
                                type="number" 
                                step="any" 
                                value={this.state.orientationHandler.maxThrottle ?? 0} 
                                onChange={(e) => this.updateOrientationhandlerValue("maxThrottle", e.currentTarget.value)} 
                            />
                        </div>
                    }
                    <div className="orientation-handler-button-wrapper">
                        { this.state.orientationHandler.isQLearning ? 
                            <>
                                <FileButton label="⬆" onChange={(value) => this.uploadHandlerConfiguration(value)}/>
                                <DroneButton label="⬇" onClick={() => this.downloadHandlerConfiguration()}/>
                            </>
                        : <></>}
                        <DroneButton label="Submit" onClick={() => this.submit()}/>
                    </div>
                </div>
            :<></>}
        </div>
    }
}