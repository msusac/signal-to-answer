import { Component } from "react";
import { toast } from "react-toastify";
import { hideLoadingModal, presenceHubLogout, showLoadingModal } from "../App";

class LogoutModal extends Component {
    constructor(props) {
        super(props)

        this.onLogout = this.onLogout.bind(this)
        this.onClose = this.onClose.bind(this)
    }

    onLogout() {
        showLoadingModal("Signing out...")
        try {
            presenceHubLogout();
            this.onClose()
            toast.success("You have successfully signed out!", { containerId: "info" })
            hideLoadingModal()
        }
        catch (ex) {
            toast.error("An error has ocurred!", { containerId: "info" })
            hideLoadingModal()
        }
    }

    onClose() {
        this.props.onShowModal(false)
    }

    render() {
        const { show } = this.props

        return (
            <div className={`modal ${show ? "modal-show" : "modal-hide"}`}>
                <div className="modal-content modal-lg border-primary border-5 bg-primary">
                    <div className="modal-header bg-primary justify-content-center fa-2x">
                        <h3 className="fw-bolder text-white">Signing out?</h3>
                    </div>
                    <div className="modal-body text-center bg-white">
                        <h4 className="text-dark fw-bold">Do you want to logout?</h4>
                    </div>
                    <div className="modal-footer bg-white justify-content-center">
                        <button type="button" className="btn btn-success btn-outline-info btn-lg border border-dark border-4 col-4-md fw-bold text-white" onClick={() => this.onLogout()}>Confirm</button>
                        <button type="button" className="btn btn-danger btn-outline-info btn-lg border border-dark border-4 col-4-md fw-bold text-white" onClick={() => this.onClose()}>Cancel</button>
                    </div>
                </div>
            </div>
        )
    }
}

export default LogoutModal;