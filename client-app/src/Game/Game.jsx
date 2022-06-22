import { Component } from "react";
import { isNotNil } from "../services/util";
import AnswerSelection from "./AnswerSelection";
import Question from "./Question";
import ResultBoard from "./ResultBoard";
import Timer from "./Timer";

class Game extends Component {
    constructor(props) {
        super(props)
    }

    render() {
        const { results, question, timer, answerChoices } = this.props;

        return (
            <div>
                {isNotNil(results) && (<ResultBoard results={results}/>)}
                {isNotNil(question) && (<Question question={question} />)}
                {isNotNil(timer) && (<Timer timer={timer} />)}
                {isNotNil(answerChoices) && (<AnswerSelection answerChoices={answerChoices} />)}
            </div>
        )
    }
}

export default Game;