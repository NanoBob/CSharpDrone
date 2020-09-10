import * as React from "react";
import "./droneButton.scss";

type Props = {
    label: string,
    onClick: (e: any) => void
};

type State = {};

export class DroneButton extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {};
    }

    public render() {
        return <button className="drone-button" onClick={this.props.onClick}>{this.props.label}</button>
    }
}