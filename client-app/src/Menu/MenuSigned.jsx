import { Component } from "react";
import { isGroupType, isRole, presenceHubChangeGroupUnique } from "../App";
import CreateGameModal from "../Modals/CreateGameModal";
import LogoutModal from "../Modals/LogoutModal";
import WaitingRoomModal from "../Modals/WaitingRoomModal";
import { GameType, GroupType, Roles } from "../services/contants";

class MenuSigned extends Component {
    
    constructor(props) {
        super(props)

        this.state = {
            logoutModalShow: false
        }

        this.onShowCreateGameModal = this.onShowCreateGameModal.bind(this)
        this.onShowLogoutModal = this.onShowLogoutModal.bind(this)
        this.onShowWaitingRoomModal = this.onShowWaitingRoomModal.bind(this)
    }

    onShowCreateGameModal(show, type) {
        if (show === true && type.id === GameType.SOLO.id) {
            presenceHubChangeGroupUnique(GroupType.SOLO_LOBBY)
        }
        else if (show === true && type.id === GameType.PRIVATE.id) {
            presenceHubChangeGroupUnique(GroupType.PRIVATE_LOBBY)
        }
        else {
            presenceHubChangeGroupUnique(GroupType.MAIN_LOBBY)
        }
    }

    onShowWaitingRoomModal(show) {
        if (show === true) {
            presenceHubChangeGroupUnique(GroupType.PUBLIC_LOBBY)
        }
        else {
            presenceHubChangeGroupUnique(GroupType.MAIN_LOBBY)
        }
    }

    onShowLogoutModal(show) {
        this.setState({ logoutModalShow: show })
    }

    render() {
        const isUser = isRole(Roles.USER)

        return (
            <>
                <div className="row g-4 p-4 justify-content-center">
                    {isUser && (
                        <div className="col-lg-4">
                            <div className="card border border-success border-5 bg-success">
                                <div className="card-header bg-success text-center fa-2x">
                                    <h3 className="fw-bolder text-white">TRAINING MODE</h3>
                                </div>
                                <div className="card-body text-success fw-bold bg-white">
                                    <div className="text-center py-2">
                                        <i className="fa-solid fa-dumbbell fa-5x"/>
                                    </div>
                                    <div className="text-center py-2" style={{minHeight: "125px"}}>
                                        <p className="card-text">Fusce nec pharetra erat. Nulla accumsan id tellus condimentum blandit. Ut vitae elit in neque mollis ultrices.</p>
                                    </div>
                                    <div className="text-center py-2 d-grid">
                                        <button type="button" className="btn btn-warning btn-outline-info btn-lg border border-dark border-4 fw-bold text-black" onClick={() => this.onShowCreateGameModal(true, GameType.SOLO)}>Try it!</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    )}
                    <div className="col-lg-4">
                        <div className="card border border-danger border-5 bg-danger">
                            <div className="card-header bg-danger text-center fa-2x">
                                <h3 className="fw-bolder text-white">PUBLIC GAME</h3>
                            </div>
                            <div className="card-body text-danger fw-bold bg-white">
                                <div className="text-center py-2">
                                    <i className="fa-solid fa-users-viewfinder fa-5x"/>
                                </div>
                                <div className="text-center py-2" style={{minHeight: "125px"}}>
                                    <p className="card-text">Phasellus sit amet suscipit felis. Morbi bibendum pretium libero id mollis</p>
                                </div>
                                <div className="text-center py-2 d-grid">
                                    <button type="button" className="btn btn-warning btn-outline-info btn-lg border border-dark border-4 fw-bold text-black" onClick={() => this.onShowWaitingRoomModal(true)}>Play!</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    {isUser && (
                        <div className="col-lg-4">
                            <div className="card border border-info border-5 bg-info">
                                <div className="card-header bg-info text-center fa-2x">
                                    <h3 className="fw-bolder text-white">PRIVATE GAME</h3>
                                </div>
                                <div className="card-body text-info fw-bold bg-white">
                                    <div className="text-center py-2">
                                        <i className="fa-solid fa-elevator fa-5x"/>
                                    </div>
                                    <div className="text-center py-2" style={{minHeight: "125px"}}>
                                        <p className="card-text">Suspendisse venenatis tincidunt eros, sed egestas augue viverra quis</p>
                                    </div>
                                    <div className="text-center py-2 d-grid">
                                        <button type="button" className="btn btn-warning btn-outline-info btn-lg border border-dark border-4 fw-bold text-black" onClick={() => this.onShowCreateGameModal(true, GameType.PRIVATE)}>Invite to game!</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    )}
                    {isUser && (
                    <>
                        <div className="col-lg-4">
                            <div className="card border border-warning border-5 bg-warning">
                                <div className="card-header bg-warning text-center fa-2x">
                                    <h3 className="fw-bolder text-white">PROFILE</h3>
                                </div>
                                <div className="card-body text-warning fw-bold bg-white">
                                    <div className="text-center py-2">
                                        <i className="fa-solid fa-address-book fa-5x"/>
                                    </div>
                                    <div className="text-center py-2" style={{minHeight: "125px"}}>
                                        <p className="card-text">Donec elementum a quam eu lacinia. Mauris eu mattis arcu, a euismod odio</p>
                                    </div>
                                    <div className="text-center py-2 d-grid">
                                        <button type="button" className="btn btn-warning btn-outline-info btn-lg border border-dark border-4 fw-bold text-black" onClick={this.loginAsGuest}>Access here!</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="col-lg-4">
                            <div className="card border border-dark border-5 bg-dark">
                                <div className="card-header bg-dark text-center fa-2x">
                                    <h3 className="fw-bolder text-white">LEADERBOARD</h3>
                                </div>
                                <div className="card-body text-dark fw-bold bg-white">
                                    <div className="text-center py-2">
                                        <i className="fa-solid fa-ranking-star fa-5x"/>
                                    </div>
                                    <div className="text-center py-2" style={{minHeight: "125px"}}>
                                        <p className="card-text">Maecenas ligula nibh, lobortis iaculis sapien a, porttitor convallis urna. In non maximus augu</p>
                                    </div>
                                    <div className="text-center py-2 d-grid">
                                        <button type="button" className="btn btn-warning btn-outline-info btn-lg border border-dark border-4 fw-bold text-black" onClick={this.loginAsGuest}>Check it!</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </>
                    )}
                    <div className="col-lg-4">
                        <div className="card border border-secondary border-5 bg-secondary">
                            <div className="card-header bg-secondary text-center fa-2x">
                                <h3 className="fw-bolder text-white">LOGOUT</h3>
                            </div>
                            <div className="card-body text-secondary fw-bold bg-white">
                                <div className="text-center py-2">
                                    <i className="fa-solid fa-sign-out fa-5x"/>
                                </div>
                                <div className="text-center py-2" style={{minHeight: "125px"}}>
                                    <p className="card-text">Sed egestas augue viverra quis?</p>
                                </div>
                                <div className="text-center py-2 d-grid">
                                    <button type="button" className="btn btn-warning btn-outline-info btn-lg border border-dark border-4 fw-bold text-black" onClick={() => this.onShowLogoutModal(true)}>Leave!</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <CreateGameModal show={isGroupType(GroupType.SOLO_LOBBY) || isGroupType(GroupType.PRIVATE_LOBBY)} 
                    onShowModal={() => this.onShowCreateGameModal()} 
                />
                <LogoutModal show={this.state.logoutModalShow} 
                    onShowModal={() => this.onShowLogoutModal()} />
                <WaitingRoomModal show={isGroupType(GroupType.PUBLIC_LOBBY)}
                    onShowModal={() => this.onShowWaitingRoomModal()} /> 
            </>
        )
    }
}

export default MenuSigned;