import { Component } from "react"
import { hideLoadingModal, showLoadingModal, setUser, setToken, presenceHubStartConnection, showInfoModal, hideInfoModal } from "../App"
import Textbox from "../Common/Textbox"
import api from "../services/api"
import { isValid, onChange, validateRequired } from "../services/util"

class LoginModal extends Component {
    constructor(props) {
        super(props)

        this.state = this.initState()

        this.onClear = this.onClear.bind(this)
        this.onChange = onChange.bind(this, [])
        this.onClose = this.onClose.bind(this)
        this.onSubmit = this.onSubmit.bind(this)
    }

    initState() {
        return {
            username: '',
            password: '',
            validations: {}
        }
    }

    onClear() {
        this.setState(this.initState())
    }

    onSubmit() {
        const validations = {
            username: validateRequired(this.state.username, "Username"),
            password: validateRequired(this.state.password, "Password")
        }

        this.setState({ validations: validations })

        if (isValid(validations)) {
            this.onLogin();
        }
    }

    async onLogin() {
        this.setState({ errors: [] })
        showLoadingModal("Signing in...")

        const body = {
            username: this.state.username,
            password: this.state.password
        }

        try {
            const user = await api.Account.login(body)
            this.onClear()
            setUser(user)
            setToken(user.token)
            presenceHubStartConnection()
            hideLoadingModal()
        }
        catch (ex) {
            if (ex.type === 'api') {
                if (ex.status === 400) {
                    showInfoModal(ex.message, true, () => hideInfoModal())
                }
                else if (ex.status === 500) {
                    showInfoModal("An error has occurred", true, () => hideInfoModal())
                }
            }
            hideLoadingModal()
        }
    }

    onClose() {
        this.onClear()
        this.props.onShowModal(false)
    }

    render() {
        const { show } = this.props;

        return (
            <div className={`modal modal-login ${show ? "modal-show" : "modal-hide"}`}> 
                <div className="modal-content modal-lg border-warning border-5 bg-warning">
                    <div className="modal-header fa-2x justify-content-center">
                        <h3 className="fw-bolder text-white">Sign in as User</h3>
                    </div>
                    <div className="modal-body bg-white">
                        <div className="m-3 row">
                            <div className="col-2">
                                <i className="fa-solid fa-user fa-2x" />
                            </div>
                            <div className="col-10">
                                <Textbox type="text" 
                                    name="username" 
                                    placeholder="Username"
                                    value={this.state.username}
                                    onChange={this.onChange}
                                    validation={this.state.validations["username"]}
                                />
                            </div> 
                        </div>
                        <div className="m-3 row">
                            <div className="col-2">
                                <i className="fa-solid fa-key fa-2x" />
                            </div>
                            <div className="col-10">
                                <Textbox type="password" 
                                    name="password" 
                                    placeholder="Password"
                                    value={this.state.password}
                                    onChange={this.onChange}
                                    validation={this.state.validations["password"]}
                                />
                            </div> 
                        </div>
                    </div>
                    <div className="modal-footer bg-white justify-content-center">
                        <button type="button" className="btn btn-success btn-outline-info btn-lg border border-dark border-4 col-5-md fw-bold text-white" onClick={() => this.onSubmit()}>Sign In</button>
                        <button type="button" className="btn btn-danger btn-outline-info btn-lg border border-dark border-4 col-5-md fw-bold text-white" onClick={() => this.onClose()}>Cancel</button>
                    </div>
                </div>
            </div>
        )
    }
}

export default LoginModal;