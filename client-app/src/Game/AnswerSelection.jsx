import AnswerSelectionItem from "./AnswerSelectionItem";

const AnswerSelection = (props) => {

    const { answerChoices } = props

    return (
        <div className="m-2 d-grid gap-3">
            {answerChoices.map((a, i) => (
                <AnswerSelectionItem 
                    answer={a.answer} 
                    answerIndex={i} 
                    status={a.status} 
                    isDisabled={a.isDisabled} />
            ))}
        </div>
    )
}

export default AnswerSelection;