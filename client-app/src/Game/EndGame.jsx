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
                </div>
            </div>
        </div>
    )
}

export default EndGame;