import { QuestionCategories, QuestionDifficulties } from "../services/contants";
import { isNotNil } from "../services/util";

const Question = (props) => {

    const { row, totalRows, description, category, difficulty, scoreMultiplier, correctAnswer } = props.question;

    return (
        <div className="row m-2">
            <div className="col-sm-12">
                <div className="card border border-success bg-success border-5">
                    <div className="card-header border border-success bg-success text-center">
                        <h3 className="card-title text-white">Question {row}/{totalRows}</h3>
                    </div>
                    <div className="card-body text-center bg-white">
                        <h2 className="text-black">{description}</h2>
                    </div>
                    {isNotNil(correctAnswer) && (
                        <div className="card-footer bg-white text-center">
                             <h3 className="text-success">{correctAnswer}</h3>
                        </div>
                    )}
                    <div className="card-footer bg-white text-center">
                        <h3 className="text-black">{Object.values(QuestionCategories).find(a => a.id === category).name} - {Object.values(QuestionDifficulties).find(a => a.id === difficulty).name} (x{scoreMultiplier})</h3>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default Question;