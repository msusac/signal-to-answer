import { Component } from 'react';
import { toast } from 'react-toastify';
import { hideLoadingModal, isUserSigned, presenceHubStartConnection, setToken, setUser, showLoadingModal } from '../App';
import LoginModal from '../Modals/LoginModal';
import RegisterModal from '../Modals/RegisterModal';
import api from '../services/api';

class MenuNotSigned extends Component {
    constructor(props) {
        super(props)

        this.state = {
            showLoginModal: false,
            showRegisterModal: false
        }

        this.onShowLoginModal = this.onShowLoginModal.bind(this)
        this.onShowRegisterModal = this.onShowRegisterModal.bind(this)
        this.onLoginAsGuest = this.onLoginAsGuest.bind(this)
    }

    onShowLoginModal(show) {
        this.setState({ showLoginModal: show })
    }

    onShowRegisterModal(show) {
        this.setState({ showRegisterModal: show })
    }

    async onLoginAsGuest() {
        showLoadingModal("Signing as Guest. Please wait!")

        try {
            const user = await api.Account.loginAsGuest()
            setUser(user)
            setToken(user.token)
            toast.success("You are now signed as Guest!", { containerId: "info" })
            presenceHubStartConnection()
            hideLoadingModal()
        }
        catch (ex) {
            toast.error("An error has occurred during sign up!", { containerId: "info" })
            hideLoadingModal()
        }
    }

    render() {
        return (
            <>
                <div className="row">
                    <div className="col-lg-6">
                        <div className="card m-3 border border-dark border-5 bg-dark">
                            <div className="card-header border border-primary bg-primary border-5 text-center">
                                <h3 className="fw-bold text-white">Sign as User ...</h3>
                            </div>
                            <div className="card-body bg-white" style={{minHeight: "650px"}}>
                                <div className="text-center p-4">
                                    <i className="fa fa-user fa-5x" />
                                </div>
                                <div className="fw-bold">
                                    <p>Etiam ornare sed massa vel porta. Donec elementum a quam eu lacinia. Mauris eu mattis arcu, a euismod odio. Phasellus sit amet suscipit felis. Morbi bibendum pretium libero id mollis. Donec in odio dolor. Vivamus bibendum lobortis commodo. Nunc et bibendum massa. Praesent orci metus, semper vitae ultrices sit amet, mollis vel tellus.</p>
                                </div>
                                <div className="card-text fw-bold">
                                    <p className="text-success">+ Nullam urna neque, congue id volutpat id, egestas ac justo. Duis sed sem orci.</p>
                                    <p className="text-success">+ Aenean nunc nisi, fermentum a purus vitae, condimentum vulputate est.</p>
                                    <p className="text-success">+ Praesent orci metus, semper vitae ultrices sit amet, mollis vel tellus..</p>
                                    <p className="text-success">+ Maecenas ligula nibh, lobortis iaculis sapien a, porttitor convallis urna</p>
                                </div>
                            </div>
                            <div className="card-footer border border-primary border-5 text-center align-items-center bg-primary" style={{minHeight: "185px"}}>
                                <div className="m-2">
                                    <button type="button" className="btn btn-warning btn-outline-dark btn-lg border border-white border-4 fw-bold text-white m-2" style={{width: "60%"}} onClick={() => this.onShowLoginModal(true)}>Sign In</button>
                                    <button type="button" className="btn btn-info btn-outline-dark btn-lg border border-white border-4 fw-bold text-white m-2" style={{width: "60%"}} onClick={() => this.onShowRegisterModal(true)}>Create a new Account</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="col-lg-6">
                        <div className="card m-3 border border-dark border-5 bg-dark">
                            <div className="card-header border border-secondary border-5 text-center bg-secondary">
                                <h3 className="fw-bold text-white">... Or Sign as Secret Guest</h3>
                            </div>
                            <div className="card-body border-5 bg-white" style={{minHeight: "650px"}}>
                                <div className="text-center p-4">
                                    <i className="fa fa-user-secret fa-5x" />
                                </div>
                                <div className="fw-bold">
                                    <p>Nullam urna neque, congue id volutpat id, egestas ac justo. Duis sed sem orci. Nam porta hendrerit eros, mollis eleifend tortor gravida sit amet. Nullam suscipit eu lectus ac sodales. Maecenas ligula nibh, lobortis iaculis sapien a, porttitor convallis urna. In non maximus augue. Nunc ullamcorper ex et ipsum tincidunt semper. Suspendisse venenatis tincidunt eros, sed egestas augue viverra quis</p>
                                </div>
                                <div className="card-text fw-bold">
                                    <p className="text-success">+ Nullam urna neque, congue id volutpat id, egestas ac justo. Duis sed sem orci.</p>
                                    <p className="text-success">+ Aenean nunc nisi, fermentum a purus vitae, condimentum vulputate est.</p>
                                    <p className="text-danger">- Praesent orci metus, semper vitae ultrices sit amet, mollis vel tellus..</p>
                                    <p className="text-danger">- Maecenas ligula nibh, lobortis iaculis sapien a, porttitor convallis urna</p>
                                </div>
                            </div>
                            <div className="card-footer border-top border-secondary border-5 text-center bg-secondary" style={{minHeight: "185px"}}>
                                <div className="m-2">
                                    <button type="button" className="btn btn-primary btn-outline-dark btn-lg border border-white border-4 fw-bold text-white" style={{width: "60%"}} onClick={this.onLoginAsGuest}>Sign As Guest</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <LoginModal show={this.state.showLoginModal && !isUserSigned()}
                    onShowModal={() => this.onShowLoginModal()} />
                <RegisterModal show={this.state.showRegisterModal && !isUserSigned()}
                    onShowModal={() => this.onShowRegisterModal()} />
            </>
        )
    }
}

export default MenuNotSigned;