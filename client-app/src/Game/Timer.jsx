const Timer = (props) => {
    const { timer } = props

    const time = timer * 10

    return (
        <div className="row m-4">
            <div className="progress">
                <div className="progress-bar progress-bar-striped progress-bar-animated bg-info" 
                    role="progressbar" 
                    aria-valuenow={timer} 
                    aria-valuemin="0" 
                    aria-valuemax="100"
                    style={{width: time + '%'}}></div>
            </div>
            <div className="text-center m-2">
                <h3 className="text-info fw-bold">{timer}</h3>
            </div>
        </div>
    )
}

export default Timer;