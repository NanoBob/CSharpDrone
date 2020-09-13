import * as React from "react";
import "./fileButton.scss";

type Props = {
    label: string,
    onChange: (e: any) => void
};

type State = {};

export class FileButton extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {};
    }

    handleButtonClick(event: any) {
        event.target.parentElement.querySelector("input").click();
    }

    async handleChange(event: any){
        if (event.target.files.length > 0) {
            const file = event.target.files[0];

            const reader = new FileReader();
            reader.onloadend = (e) => {
                this.props.onChange((e.currentTarget as any).result);
            }
            reader.readAsBinaryString(file);
        }
    }

    public render() {
        return (
            <div className="file-button-wrapper">
                <input type="file" onChange={(event) => this.handleChange(event)}/>
                <button className="file-button" onClick={(event) => this.handleButtonClick(event)}>{this.props.label}</button>
            </div>
        );
    }
}