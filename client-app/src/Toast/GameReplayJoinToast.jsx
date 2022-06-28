import { Component } from "react";
import { gameHubReplyToGameReplayInvite } from "../App";
import { ReplayStatus } from "../services/contants";

class GameReplayJoinToast extends Component {
    constructor (props) {
        super (props)

        this.onSubmit = this.onSubmit.bind(this)
    }

    onSubmit(replyId) {
        gameHubReplyToGameReplayInvite(replyId)
    }

    render() {
        return (
            <>
                <div className="text-center m-2">
                    <p>User {this.props.fromUser} wants to replay game! Do you want to accept?</p>
                </div>
                <div className="text-center m-2">
                    <button type="button" className="btn btn-danger m-2" onClick={() => this.onSubmit(ReplayStatus.WANTS_TO_REPLAY)}>Accept</button>
                    <button type="button" className="btn btn-warning m-2" onClick={() => this.onSubmit(ReplayStatus.DOES_NOT_WANT_TO_REPLAY)}>Reject</button>
                </div>
            </>
        )
    }
}

export default GameReplayJoinToast;