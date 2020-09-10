import * as React from "react";
import "./toggle.scss";

type Props = {
    label: string,
    value: boolean,
    onToggle: (value: boolean) => void,
};

type State = {
};

export class Toggle extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {};
    }

    public render() {
        return <div className="toggle-row">
            <span className="toggle-label">{ this.props.label }</span>
            <span className="toggle-wrapper">
                <span className={`toggle ${this.props.value ? "active" : ""}`} onClick={() => this.props.onToggle(!this.props.value)}>
                    <span className={`toggle-indicator`}></span>
                </span>
            </span>
        </div>
    }
}