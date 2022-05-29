import { isNotNil } from "../services/util";

const LoadingModal = (props) => {
    const { show, message } = props;

    return (
        <div className={`modal modal-loading ${show ? "modal-show" : "modal-hide"}`}>
            <div className="modal-body modal-body-loading">
                <div className="spinner-border modal-spinner-loading"></div>
                    {isNotNil(message) &&(
                        <div className="modal-text-loading">
                            <span>{message}</span>
                        </div>
                    )}
            </div>
        </div>
    )
}

export default LoadingModal;