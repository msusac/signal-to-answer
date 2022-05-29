import { Component } from "react";
import { retrievePublicLobbyCount } from "../App";

class WaitingRoomModal extends Component {
    constructor(props) {
        super(props)

        this.onClose = this.onClose.bind(this)
    }

    onClose() {
        this.props.onShowModal(false)
    }

    render() {
        const count = retrievePublicLobbyCount() - 1
        const message = count > 0 ? `${count} player(s) searching for game ...` : `No players in public lobby ...`

        return (
            <div className={`modal modal-waiting-room ${this.props.show ? "modal-show" : "modal-hide"}`}>
                <div className="modal-content modal-lg border-danger border-5 bg-danger">
                    <div className="modal-header bg-danger justify-content-center fa-2x">
                        <h3 className="fw-bolder text-white">Public Lobby - Waiting Room</h3>
                    </div>
                    <div className="modal-body text-center bg-white">
                        <div className="spinner-grow modal-spinner-waiting-room"></div>
                        <div className="fw-bolder fa-lg m-3">
                            <span>{message}</span>
                        </div>
                    </div>
                    <div className="modal-footer bg-white justify-content-center">
                        <button type="button" className="btn btn-danger btn-outline-info btn-lg border border-dark border-4 col-4-md fw-bold text-white" onClick={() => this.onClose()}>Leave</button>
                    </div>
                </div>
            </div>
        )
    }
}

export default WaitingRoomModal;