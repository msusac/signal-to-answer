import { isNotNil } from "../services/util";
import AnswerSelection from "./AnswerSelection";
import EndGame from "./EndGame";
import Question from "./Question";
import ResultBoard from "./ResultBoard";
import Timer from "./Timer";

const Game = (props) => {

    const { results, question, timer, answerChoices, endGame, showReplay } = props;

    return (
        <div>
            {isNotNil(endGame) && (<EndGame gameType={endGame.gameType} showReplay={showReplay}/>) }
            {isNotNil(results) && (<ResultBoard results={results}/>)}
            {isNotNil(question) && (<Question question={question} />)}
            {isNotNil(timer) && (<Timer timer={timer} />)}
            {isNotNil(answerChoices) && (<AnswerSelection answerChoices={answerChoices} />)}
        </div>
    )
}

export default Game;