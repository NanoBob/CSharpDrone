import * as React from "react";
import "./panel.scss";

type Props = {
    title: string;
};

type State = {
    isCollapsed: boolean;
};

export class Panel extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = { isCollapsed: false };
    }

    toggleBody() {
        this.setState({isCollapsed: !this.state.isCollapsed});
    }

    public render() {
        return <div className="panel">
            <div className="panel-header" onClick={() => this.toggleBody()}>
                <span className="panel-title">{ this.props.title }</span>
        
                <span className="toggle-button">{this.state.isCollapsed ? "▲" : "▼"}</span>
            </div>
            {this.state.isCollapsed ? 
                <></> : 
                <div className="panel-body">
                   {this.props.children}
                </div>
            }
        </div>
    }
}