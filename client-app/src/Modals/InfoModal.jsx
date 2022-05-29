const InfoModal = (props) => {

    const { show, message, hasError, action} = props

    let modalHeaderStyle = "border-primary bg-primary"
    let modalContentStyle = "border-primary"

    if (hasError) {
        modalHeaderStyle = "border-danger bg-danger"
        modalContentStyle = "border-danger"
    }
    
    return (
        <div className={`modal modal-info ${show ? "modal-show" : "modal-hide"}`}>
            <div className={`modal-content modal-lg border-5 ${modalHeaderStyle}`}>
                <div className={`modal-header fa-2x justify-content-center ${modalContentStyle}`}>
                    <h3 className="fw-bolder text-white">{hasError ? "Error" : "Info"}</h3>
                </div>
                <div className="modal-body text-center bg-white">
                    <h4 className="text-dark fw-bold">{message}</h4>
                </div>
                <div className="modal-footer bg-white justify-content-center">
                    <button type="button" className="btn btn-primary btn-outline-info btn-lg border border-dark border-4 fw-bold text-white" style={{width: "60%"}} onClick={action}>Continue</button>
                </div>
            </div>
        </div>
    )
}

export default InfoModal;