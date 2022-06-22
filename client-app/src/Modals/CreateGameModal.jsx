import { Component } from "react";
import { toast } from "react-toastify";
import { hideLoadingModal, showLoadingModal, gameHubStartConnection, isGroupType, showLoadingModalWithButton, leaveInviteLobby } from "../App";
import LiveSelect from "../Common/LiveSelect";
import MultiSelect from "../Common/MultiSelect";
import Range from "../Common/Range";
import Select from "../Common/Select";
import api from "../services/api";
import { GroupType, QuestionCategories, QuestionDifficulties } from "../services/contants";
import { isValid, onChange, validateRequired } from "../services/util";

class CreateGameModal extends Component {
    constructor(props) {
        super(props)

        this.state = this.initState()

        this.onClear = this.onClear.bind(this)
        this.onClose = this.onClose.bind(this)
        this.onChange = onChange.bind(this, [])
        this.onInputChangeLiveSelect = this.onInputChangeLiveSelect.bind(this)
        this.onChangeMultiSelect = this.onChangeMultiSelect.bind(this)
        this.onSubmit = this.onSubmit.bind(this)
    }

    initState() {
        return {
            limit: 10,
            difficulty: 0,
            categories: [],
            selectedUser: '',
            validations: {},
            errors: [],
            userOption: '',
            userOptions: [],
            isLiveSelectLoading: false
        }
    }

    onChangeLiveSelect(e) {
        this.setState({ selectedUser: e })
    }

    onInputChangeLiveSelect(e) {
        this.setState({ userOption: e })

        if (this.state.userOption.length >= 2) {
            this.onSearchInviteList(this.state.userOption)
        }
    }

    async onSearchInviteList(username) {
        this.setState({ isLiveSelectLoading: true })
        
        const body = {
            username: username
        }

        try {
            const userOptions = await api.List.searchInviteList(body)
            this.setState({ userOptions: userOptions, isLiveSelectLoading: false })
        }
        catch (ex) {
            toast.error("An error has occurred!", { containerId: "info" })
            this.setState({ isLiveSelectLoading: false })
        }
    }

    onChangeMultiSelect(e) {
        this.setState({ categories: e })
    }

    onClear() {
        this.setState(this.initState())
    }

    onClose() {
        this.onClear()
        this.props.onShowModal(false, null)
    }

    onSubmit() {
        let validations = {
            limit: validateRequired(this.state.limit, "Limit"),
            difficulty: validateRequired(this.state.difficulty, "Difficulty"),
            categories: validateRequired(this.state.categories, "Categories"),
        }

        if (isGroupType(GroupType.PRIVATE_LOBBY)) {
            validations = { ...validations, inviteUser: validateRequired(this.state.selectedUser, "Invite User")}
        }

        this.setState({ validations: validations })

        if (isValid(validations)) {
            if(isGroupType(GroupType.SOLO_LOBBY)) {
                this.onCreateSoloGame();
            }
            else if (isGroupType(GroupType.PRIVATE_LOBBY)) {
                this.onCreatePrivateGame();
            }
        }
    }

    async onCreateSoloGame() {
        showLoadingModal("Creating solo game...")

        let categories = []
        this.state.categories.forEach(c => {
            categories.push(c.id)
        })

        const body = {
            limit: this.state.limit,
            categories: categories,
            difficulty: this.state.difficulty
        }
        
        try {
            const gameId = await api.Game.createSolo(body)
            gameHubStartConnection(gameId)
            hideLoadingModal()
            this.onClear()
        }
        catch (ex) {
            if (ex.type === 'api') {
                if (ex.status === 400) {
                    toast.error(ex.message, { containerId: "info" })
                }
                else if (ex.status === 500) {
                    toast.error("An error has occurred!", { containerId: "info" })
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

    async onCreatePrivateGame() {
        showLoadingModalWithButton("Sending private game invite.", "Leave", () => { 
            leaveInviteLobby()
            hideLoadingModal()
        })

        let categories = []
        this.state.categories.forEach(c => {
            categories.push(c.id)
        })

        const body = {
            limit: this.state.limit,
            categories: categories,
            difficulty: this.state.difficulty,
            inviteUsers: [this.state.selectedUser.name]
        }
        
        try {
            await api.Game.createPrivate(body)
        }
        catch (ex) {
            if (ex.type === 'api') {
                if (ex.status === 400) {
                    toast.error(ex.message, { containerId: "info" })
                }
                else if (ex.status === 500) {
                    toast.error("An error has occurred!", { containerId: "info" })
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

    render() {
        let title = ""
        let style = ""

        if(isGroupType(GroupType.SOLO_LOBBY)) {
            title = "Solo Game"
            style = "border-success bg-success"
        }
        else if (isGroupType(GroupType.PRIVATE_LOBBY)) {
            title = "Private Game"
            style = "border-info bg-info"
        }

        return (
            <div className={`modal modal-create-game ${this.props.show ? "modal-show" : "modal-hide"}`}>
                <div className={`modal-content modal-lg border-5 ${style}`}>
                    <div className="modal-header fa-2x justify-content-center">
                        <h3 className="fw-bolder text-white">Create {title}</h3>
                    </div>
                    <div className="modal-body bg-white">
                        {this.state.errors.map((e, i) => (<div className="alert alert-danger m-2 row">{e}</div>))}
                        <div className="m-3 row">
                            <label htmlFor="limit" className="form-label text-start fw-bolder mb-2">Questions count</label>
                            <Range name="limit"
                                min={5}
                                max={20}
                                value={this.state.limit}
                                onChange={this.onChange}
                                validation={this.state.validations["limit"]}
                            />
                        </div>
                        <div className="m-3 row">
                            <label htmlFor="difficulty" className="form-label text-start fw-bolder mb-2">Difficulty</label>
                            <Select name="difficulty" 
                                value={this.state.difficulty}
                                values={Object.values(QuestionDifficulties)}
                                onChange={this.onChange}
                                validation={this.state.validations["difficulty"]}
                            />
                        </div>
                        <div className="m-3 row">
                            <label htmlFor="difficulty" className="form-label text-start fw-bolder">Categories</label>
                            <MultiSelect name="categories"
                                value={this.state.categories}
                                values={Object.values(QuestionCategories)}
                                onChange={(e) => this.onChangeMultiSelect(e)}
                                validation={this.state.validations["categories"]}/>
                        </div>
                        {isGroupType(GroupType.PRIVATE_LOBBY) && (
                            <div className="m-3 row">
                                <label htmlFor="inviteUser" className="form-label text-start fw-bolder">Invite User</label>
                                <LiveSelect name="inviteUser"
                                    isLoading={this.state.isLiveSelectLoading}
                                    value={this.state.selectedUser}
                                    values={this.state.userOptions}
                                    inputValue={this.state.userOption}
                                    onChange={(e) => this.onChangeLiveSelect(e)}
                                    onInputChange={(e) => this.onInputChangeLiveSelect(e)}
                                    validation={this.state.validations["inviteUser"]}
                                />
                            </div>
                        )}
                    </div>
                    <div className="modal-footer bg-white justify-content-center">
                        <button type="button" className="btn btn-success btn-outline-info btn-lg border border-dark border-4 col-5-md fw-bold text-white" onClick={() => this.onSubmit()}>Start game</button>
                        <button type="button" className="btn btn-danger btn-outline-info btn-lg border border-dark border-4 col-5-md fw-bold text-white" onClick={() => this.onClose()}>Cancel</button>
                    </div>
                </div>
            </div>
        )
    }
}

export default CreateGameModal;