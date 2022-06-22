import { AnswerStatus } from "../services/contants"

const ResultBoardItem = (props) => {

    const { result, isMain } = props

    let playerText = "Player"
    let playerStyle = "border-info bg-info"

    if (!isMain) {
        playerText = "Opponent"
        playerStyle = "border-warning bg-warning"
    }

    let currentAnswerText = ""
    let currentAnswerStyle = ""

    if (result.currentAnswerStatus === AnswerStatus.CORRECT) {
        currentAnswerText= "CORRECT + " + result.currentAnswerScore
        currentAnswerStyle = "text-success"
    }
    else if (result.currentAnswerStatus === AnswerStatus.INCORRECT) {
        currentAnswerText= "INCORRECT"
        currentAnswerStyle = "text-danger"
    }

    return (
        <div className={`card border-5 m-2 ${playerStyle}`}>
            <div className={`card-header border text-center ${playerStyle}`}>
                <h3 className="card-title text-white">{playerText}</h3>
            </div>
            <div className="card-body text-center bg-white">
                <h3 className="text-primary">{result.player}</h3>
             </div>
             <div className="card-footer text-center bg-white">
                <h3 className="text-secondary">{result.score}</h3>
             </div>
             <div className="card-footer text-center bg-white">
                <h3 className={`${currentAnswerStyle}`}>{currentAnswerText}</h3>
             </div>
        </div>
    )
}

export default ResultBoardItem;