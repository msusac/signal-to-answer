import { leaveGame } from "../App";
import { GameType } from "../services/contants";

const EndGame = (props) => {

    const { gameType } = props.endGame

    const gameTypeName = Object.values(GameType).find(a => a.id === gameType).name

    return (
        <div className="row m-2">
            <div className="col-sm-12">
                <div className="card border border-dark bg-danger border-5">
                    <div className="card-header border border-danger bg-danger text-center">
                        <h3 className="card-title text-white">{gameTypeName} Ended!</h3>
                    </div>
                    <div className="modal-footer bg-white justify-content-center">
                        <button type="button" className="btn btn-success btn-outline-info btn-lg border border-dark border-4 col-5-md fw-bold text-white" onClick={() => this.onSubmit()}>Replay</button>
                        <button type="button" className="btn btn-danger btn-outline-info btn-lg border border-dark border-4 col-5-md fw-bold text-white" onClick={() => leaveGame()}>Leave</button>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default EndGame;