import * as React from "react";
import "./droneInput.scss";

type Props = {
    label: string,
    value: any,
    type: string,
    step?: string,
    onChange: (e: any) => void
};

type State = {};

export class DroneInput extends React.Component<Props, State> {
    static defaultProps = {
        step: ""
    }

    constructor(props: Props) {
        super(props);
        this.state = {};
    }

    public render() {
        return <div className="input-wrapper">
            <div className="input-label">
                {this.props.label}
            </div>
            <input type={this.props.type} value={this.props.value} onChange={this.props.onChange} step={this.props.step} />
        </div>
    }
}