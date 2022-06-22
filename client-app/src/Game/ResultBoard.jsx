import { retrieveUser } from "../App";
import ResultBoardItem from "./ResultBoardItem";

const ResultBoard = (props) => {

    const { results } = props;

    const user = retrieveUser()

    const player = results.find(r => r.player === user.username)
    const opponents = results.filter(r => r.player !== user.username)

    return (
        <div className="row m-2">
            {opponents.length === 0 && (
                <div className="col-sm-12">
                    <ResultBoardItem result={player} isMain={true} />
                </div>
            )}
            {opponents.length === 1 && (
                <>
                    <div className="col-sm-6 col-md-6 col-lg-5">
                        <ResultBoardItem result={player} isMain={true} />
                    </div>
                    <div className="col-sm-6 col-sm-6 col-lg-5 offset-lg-2">
                        <ResultBoardItem result={opponents[0]} isMain={false} />
                    </div>
                </>
            )}
        </div>
    )
}

export default ResultBoard;