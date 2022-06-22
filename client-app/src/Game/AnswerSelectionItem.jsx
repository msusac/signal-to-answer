import { gameHubAnswerQuestion } from "../App";
import { AnswerStatus } from "../services/contants";

const AnswerSelectionItem = (props) => {
    const { answer, answerIndex, status, isDisabled } = props;

    let btnColor = 'btn-primary'

    if (status === AnswerStatus.CORRECT) {
        btnColor = 'btn-success'
    }
    else if (status === AnswerStatus.INCORRECT) {
        btnColor = 'btn-danger'
    }

    return (
        <button type="button" 
            key={answerIndex}
            className={`btn ${btnColor} border-3 border-dark btn-lg fw-bold`} 
            disabled={isDisabled}
            onClick={() => gameHubAnswerQuestion(answerIndex)}>{answer}</button>
    )
}

export default AnswerSelectionItem;