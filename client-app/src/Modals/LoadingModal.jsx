import { isNotNil } from "../services/util";

const LoadingModal = (props) => {
    const { show, message, showBtn, btnAction, btnActionName } = props;

    return (
        <div className={`modal modal-loading ${show ? "modal-show" : "modal-hide"}`}>
            <div className="modal-body modal-body-loading">
                <div className="spinner-border modal-spinner-loading"></div>
                    {isNotNil(message) && (
                        <div className="modal-text-loading">
                            <span>{message}</span>
                        </div>
                    )}
                    {showBtn && (
                        <div className="mt-2">
                            <button type="button" className="btn btn-warning btn-outline-info btn-lg border border-dark border-4 col-5-md fw-bold text-white" onClick={btnAction}>{btnActionName}</button>
                        </div>
                    )}
            </div>
        </div>
    )
}

export default LoadingModal;