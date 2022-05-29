import { Component } from "react";
import { hideInfoModal, hideLoadingModal, showInfoModal, showLoadingModal } from "../App";
import api from "../services/api";

class PrivateGameJoinToast extends Component {
    constructor (props) {
        super (props)

        this.onSubmit = this.onSubmit.bind(this)
    }

    async onSubmit(isAccepted) {
        if (isAccepted) {
            showLoadingModal("Joining private game ...")
        }

        const body = {
            gameId: this.props.gameId,
            groupId: this.props.groupId,
            isAccepted: isAccepted
        }

        try {
            await api.Game.respondToPrivateGameInvite(body)
        }
        catch (ex) {
            hideLoadingModal()
            showInfoModal("An error has occured", true, () => hideInfoModal())
        }
    }

    render() {
        console.log(this.props)
        return (
            <>
                <div className="text-center m-2">
                    <p>User {this.props.fromUser} has invited you to game! Do you want to join in?</p>
                </div>
                <div className="text-center m-2">
                    <button type="button" className="btn btn-danger m-2" onClick={() => this.onSubmit(true)}>Accept</button>
                    <button type="button" className="btn btn-warning m-2" onClick={() => this.onSubmit(false)}>Reject</button>
                </div>
            </>
        )
    }
}

export default PrivateGameJoinToast;