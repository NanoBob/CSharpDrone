import * as React from "react";
import * as atlas from "azure-maps-control";
import "./map.scss";
import "azure-maps-control/";
import { config } from "../../config";
import { Panel } from "../panel/Panel";
import { AppState } from "../../store";
import { connect } from "react-redux";
import { GpsState } from "../../store/gps/types";
import { requestPosition } from "../../store/gps/actions";

type Props = {
  gpsState: GpsState,
  requestPosition: any
};

type State = {};

export class Map extends React.Component<Props, State> {
  public map: atlas.Map;
  public isReady: boolean;
  public methodQueue: (() => void)[];

  constructor(props: any) {
    super(props);
    this.state = {};

    this.map = null as any;
    this.isReady = false;
    this.methodQueue = [];

    this.props.requestPosition();
    // setInterval(() => this.props.requestPosition(), 250)
  }

  componentDidMount() {
    this.methodQueue = [];

    this.map = new atlas.Map("mapContainer", {
      authOptions: {
        authType: atlas.AuthenticationType.subscriptionKey,
        subscriptionKey: config.azureMapsKey,
      },
      view: "Auto",
      showFeedbackLink: false,
      showLogo: false,
      autoResize: true,
    });

    this.map.events.add("ready", this.handleMapReady.bind(this));
  }

  componentDidUpdate(previousProps: any) {
    if (previousProps.gpsState.position !== this.props.gpsState.position) {
      this.focusMap(this.props.gpsState.position.latitude, this.props.gpsState.position.longitude, 15);
    }
  }

  handleMapReady() {
    this.isReady = true;

    this.focusMap(
      config.initCoordinates.longitude,
      config.initCoordinates.latitude,
      config.initCoordinates.zoom
    );

    for (const method of this.methodQueue) {
      method();
    }
    this.methodQueue = [];
  }

  focusMap(latitude: number, longitude: number, zoom: number = 1) {
    if (!this.isReady) {
      this.methodQueue.push(() => {
        this.focusMap(latitude, longitude, zoom);
      });
      return;
    }

    this.map.setCamera({
      zoom: zoom,
      center: new atlas.data.Position(longitude, latitude, zoom),
    });
  }

  render() {
    return (
      <Panel title="Map">
        <div id="mapContainer"></div>
      </Panel>
    );
  }
}

export default connect((state: AppState) => {
  return {
    gpsState: state.gps,
  };
}, { requestPosition })(Map);
