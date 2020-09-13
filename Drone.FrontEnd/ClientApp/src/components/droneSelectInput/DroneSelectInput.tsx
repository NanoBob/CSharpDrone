import * as React from "react";
import "./droneSelectInput.scss";

type Props = {
    label: string,
    value: any,
    items: { label: any, value: any }[],
    onChange: (e: any) => void
};

type State = {
    isOpen: boolean
};

export class DroneSelectInput extends React.Component<Props, State> {
    static defaultProps = {
        step: ""
    }

    constructor(props: Props) {
        super(props);
        this.state = {
            isOpen: false
        };
    }

    toggleOpen() {
        this.setState({ isOpen: !this.state.isOpen});
    }

    triggerChange(item: { label: any, value: any}) {
        this.props.onChange(item.value);
        this.toggleOpen();
    }

    public render() {
        return <div className="input-select-wrapper">
            <div className="input-select-label">
                {this.props.label}
            </div>
            <div className="input-select-input-wrapper">
                <input value={this.props.items.find(x => x.value === this.props.value)?.label} readOnly onClick={() => this.toggleOpen()}/>
                { this.state.isOpen ? 
                    <div className="select-options-wrapper">
                        { this.props.items.map(item => (
                            <div key={item.value} className="select-option" onClick={() => this.triggerChange(item)}>{ item.label }</div>
                        ))}
                    </div>
                : <></>}
            </div>
        </div>
    }
}