import { Component } from "react"
import { hideLoadingModal, showLoadingModal, showInfoModal, hideInfoModal } from "../App"
import Textbox from "../Common/Textbox"
import api from "../services/api"
import { isValid, onChange, validateEqualFields, validateRequired } from "../services/util"

class RegisterModal extends Component {
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
            passwordRepeat: '',
            email: '',
            validations: {},
            errors: []
        }
    }

    onClear() {
        this.setState(this.initState())
    }

    onSubmit() {
        const validations = {
            username: validateRequired(this.state.username, "Username"),
            email: validateRequired(this.state.email, "Email"),
            password: validateRequired(this.state.password, "Password"),
            passwordRepeat: validateEqualFields(this.state.passwordRepeat, this.state.password, "Password Repeat", "Password")
        }

        this.setState({ validations: validations })

        if (isValid(validations)) {
            this.onRegister();
        }
    }

    async onRegister() {
        this.setState({ errors: [] })
        showLoadingModal("Creating a new account ...")

        const body = {
            username: this.state.username,
            password: this.state.password,
            email: this.state.email
        }

        try {
            await api.Account.register(body)
            hideLoadingModal()
            showInfoModal("Your account is created. Please sign in.", false, () => {
                hideInfoModal()
                this.onClose()
            })
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
            else if (ex.type === 'validation') {
                const errors = []
                ex.data.forEach(d => {
                    errors.push(d.message)
                })
                this.setState({ errors: errors })
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
            <div className={`modal modal-register ${show ? "modal-show" : "modal-hide"}`}> 
                <div className="modal-content modal-lg border-info border-5 bg-info">
                    <div className="modal-header fa-2x justify-content-center">
                        <h3 className="fw-bolder text-white">Create a new Account</h3>
                    </div>
                    <div className="modal-body bg-white">
                        {this.state.errors.map((e, i) => (<div class="alert alert-danger m-2 row">{e}</div>))}
                        <div className="m-3 row">
                            <div className="col-2">
                                <i className="fa-solid fa-user fa-2x" />
                            </div>
                            <div className="col-10">
                                <Textbox type="text" 
                                    name="username" 
                                    id="username"
                                    placeholder="Username"
                                    value={this.state.username}
                                    onChange={this.onChange}
                                    validation={this.state.validations["username"]}
                                />
                            </div> 
                        </div>
                        <div className="m-3 row">
                            <div className="col-2">
                                <i className="fa-solid fa-envelope fa-2x" />
                            </div>
                            <div className="col-10">
                                <Textbox type="email" 
                                    name="email" 
                                    placeholder="E-mail"
                                    value={this.state.email}
                                    onChange={this.onChange}
                                    validation={this.state.validations["email"]}
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
                        <div className="m-3 row">
                            <div className="col-2">
                            </div>
                            <div className="col-10">
                                <Textbox type="password" 
                                    name="passwordRepeat" 
                                    placeholder="Password Repeat"
                                    value={this.state.passwordRepeat}
                                    onChange={this.onChange}
                                    validation={this.state.validations["passwordRepeat"]}
                                />
                            </div> 
                        </div>
                    </div>
                    <div className="modal-footer bg-white justify-content-center">
                        <button type="button" className="btn btn-success btn-outline-info btn-lg border border-dark border-4 col-5-md fw-bold text-white" onClick={() => this.onSubmit()}>Sign Up</button>
                        <button type="button" className="btn btn-danger btn-outline-info btn-lg border border-dark border-4 col-5-md fw-bold text-white" onClick={() => this.onClose()}>Cancel</button>
                    </div>
                </div>
            </div>
        )
    }
}

export default RegisterModal;