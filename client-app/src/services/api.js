import axios from "axios";
import { retrieveToken } from "../App";
import { isNotNil } from "./util";

const instance = axios.create({
    baseURL: "http://localhost:5000/api"
})

instance.interceptors.request.use(config => {
    const token = retrieveToken()

    if (isNotNil(token)) {
        config.headers.Authorization = `Bearer ${token}`
    }

    return config
})

instance.interceptors.response.use(
    response => response.data,
    error => {
        let ex = {}
        if (error.response && error.response.data.statusCode === 400 && Array.isArray(error.response.data.details)) {
            ex = { type: 'validation', data: error.response.data.details }
        }
        else {
            const responseData = (error.response && error.response.data) || undefined
            ex = { type: 'api', status: responseData.statusCode, message: responseData.message }
        }
        
        throw ex
    }
)

const Account = {
    get: () => instance.get("/account"),
    login: (body) => instance.post("/account/login", body),
    loginAsGuest: () => instance.get("/account/login-as-guest"),
    register: (body) => instance.post("/account/register", body)
}

const Game = {
    createSolo: (body) => instance.post("/game/create-solo", body),
    createPrivate: (body) => instance.post("/game/create-private", body),
    respondToPrivateGameInvite: (body) => instance.post("/game/respond-private-game-invite", body)
}

const List = {
    searchInviteList: (body) => instance.post("/list/invite-search", body)
}

const api = {
    Account, Game, List
}

export default api