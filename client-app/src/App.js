import { Component } from 'react';
import { toast, ToastContainer } from 'react-toastify';
import LoadingModal from './Modals/LoadingModal';
import Header from './Other/Header';
import Footer from './Other/Footer';
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import api from './services/api';
import { isNotNil } from './services/util';
import { GroupType, Roles } from './services/contants';
import { Redirect, Route, Switch } from 'react-router-dom';
import Menu from './Menu/Menu';
import Game from './Game/Game';
import "./App.css"
import PrivateGameJoinToast from './Toast/PrivateGameJoinToast';
import notificationAudio from './assets/audios/notification.mp3'

var app = {}

export function retrieveToken() {
  return window.localStorage.getItem("jwt")
}

export function hideLoadingModal() {
  app.setState({ 
    loadingModalShow: false, 
    loadingModalMessage: '',
    loadingModalBtnShow: false, 
    loadingModalBtnAction: '',
    loadingModalBtnActionName: ''
  })
}

export function showLoadingModal(message) {
  app.setState({ 
    loadingModalShow: true, 
    loadingModalMessage: message,
    loadingModalBtnShow: false, 
    loadingModalBtnAction: '',
    loadingModalBtnActionName: ''
  })
}

export function showLoadingModalWithButton(message, btnActionName, btnAction) {
  app.setState({ 
    loadingModalShow: true, 
    loadingModalMessage: message,
    loadingModalBtnShow: true, 
    loadingModalBtnAction: btnAction,
    loadingModalBtnActionName: btnActionName
  })
}

export function setGroup(groupType) {
  app.setState({ groupType: groupType })
}

export function setToken(token) {
  window.localStorage.setItem("jwt", token)
  app.setState({ token: token })
}

export function setUser(user) {
  app.setState({ user: user })
}

export function isUserSigned() {
  return isNotNil(app.state.token) && isNotNil(app.state.user)
}

export function isRole(role) {
  return isUserSigned() && app.state.user.role === role.mark
}

export function isGroupType(groupType) {
  return app.state.groupType.id === groupType.id
}

export function isGroupTypeInGame() {
  var groups = [GroupType.IN_GAME_SOLO, GroupType.IN_GAME_PUBLIC, GroupType.IN_GAME_PRIVATE]

  return groups.some(a => a.id === app.state.groupType.id);
}

export function clearGameData() {
  app.setState({
    results: [],
    answerChoices: [],
    question: '',
    timer: '',
    endGame: ''
  })
}

export function retrieveUser() {
  return app.state.user
}

// Game Hub
export function gameHubStartConnection(gameId) {
  showLoadingModal("Establishing connection to game...")

  const connection = new HubConnectionBuilder()
    .withUrl("http://localhost:5000/hub/game-hub?gameId=" + gameId, {
       accessTokenFactory: () => app.state.token
    })
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Information)
    .build()

  connection.start().then(() => {
    app.setState({ gameHubConnection: connection })
    hideLoadingModal()
  }).catch((ex) => {
    if (ex.type === 'api') {
      if (ex.status === 400) {
        toast.error(ex.message, { containerId: "info" });
      }
      else if (ex.status === 500) {
        toast.error("An error has occurred!", { containerId: "info" });
      }
    }
    leaveGame()
    hideLoadingModal()
  })

  connection.on("ReceiveLoadingMessage", (message) => {
    showLoadingModalWithButton(message, "Leave game", () => {
      leaveGame()
      hideLoadingModal()
    })
  })
  
  connection.on("ReceiveHideLoading", () => {
    hideLoadingModal()
  })

  connection.on("ReceiveResultInfo", (results) => {
    app.setState({ results: results })
  })

  connection.on("ReceiveQuestionInfo", (question) => {
      app.setState({ question: question })
  })

  connection.on("ReceiveTimerInfo", (timer) => {
    app.setState({ timer: timer })
  })
  
  connection.on("ReceiveAnswerChoice", (answers) => {
    app.setState({ answerChoices: answers })
  })

  connection.on("ReceiveGameCancelled", (message) => {
    toast.info("Game match cancelled due following reason: " + message, { containerId: "info" })
    leaveGame()
    hideLoadingModal()
  })

  connection.on("ReceiveEndGameInfo", (endGame) => {
    clearGameData()
    app.setState({ endGame: endGame })
  })
}

export function gameHubAnswerQuestion(selectedAnswerIndex) {
  app.state.gameHubConnection.invoke("AnswerQuestion", selectedAnswerIndex).catch((ex) => {
    toast.error("An error has occurred while sending question answer.", { containerId: "info" })
  })
}

// Presence Hub
export function presenceHubStartConnection() {
  showLoadingModal("Establishing connection...")
  
  const connection = new HubConnectionBuilder()
    .withUrl("http://localhost:5000/hub/presence-hub", {
       accessTokenFactory: () => app.state.token
    })
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Information)
    .build()

  connection.start().then(() => {
    hideLoadingModal()
  })
  .catch((ex) => {
    toast.error("An error has occurred during connection.", { containerId: "info" })
  })

  connection.on("ReceiveGroupType", (groupId) => {
    const groupType = Object.values(GroupType).find(a => a.id === groupId)
    app.setState({ groupType: groupType })

    if (isGroupType(GroupType.OFFLINE)) {
      app.setState({ presenceHubConnection: null, gameHubConnection: null })
    }
    else {
      app.setState({ presenceHubConnection: connection, gameHubConnection: null })
    }
  });

  connection.on("ReceivePublicLobbyCount", (count) => {
    app.setState({ publicLobbyCount: count })
  });

  connection.on("ReceivePublicGame", (gameId) => {
    gameHubStartConnection(gameId)
    hideLoadingModal()
  });

  connection.on("ReceivePrivateGame", (gameId) => {
    gameHubStartConnection(gameId)
    hideLoadingModal()
  });

  connection.on("ReceivePrivateGameInvite", (fromUser, gameId, groupId) => {
    const audio = new Audio(notificationAudio)
    audio.play()
      
    toast.info(<PrivateGameJoinToast fromUser={fromUser} gameId={gameId} groupId={groupId} />, { containerId: "invite" })
  })

  connection.on("ReceivePrivateGameCancelled", (message) => {
    hideLoadingModal()
    toast.info("Private game cancelled due following reason: " + message, { containerId: "info" })
  })

  connection.on("ReceivePrivateGameLoadingMessage", (message) => {
    app.setState({ loadingModalMessage: message })
  })
}

export function presenceHubStopConnection() {
  app.state.presenceHubConnection.stop().then(() => {
      app.setState({ presenceHubConnection: null, gameHubConnection: null, groupType: GroupType.OFFLINE })
  })
  .catch((ex) => {
    toast.error("An error has occurred while disconnecting.", { containerId: "info" })
  })
}

export function presenceHubChangeGroupUnique(groupType) {
  app.state.presenceHubConnection.invoke("ChangeGroupUnique", groupType.id).catch((ex) => {
    toast.error("An error has occurred while changing connection groupType.", { containerId: "info" })
  })
}

// Other connection functions
export function retrievePublicLobbyCount() {
  return app.state.publicLobbyCount
}

export function leaveInviteLobby() {
  presenceHubChangeGroupUnique(GroupType.MAIN_LOBBY)
}

export function leaveGame() {
  app.state.gameHubConnection.stop().then(() => {
    presenceHubChangeGroupUnique(GroupType.MAIN_LOBBY)
  })
}

class App extends Component {
  constructor(props) {
    super(props)

    this.state = {
      loadingModalShow: false,
      loadingModalMessage: '',
      loadingModalBtnShow: false, 
      loadingModalBtnAction: '',
      loadingModalBtnActionName: '',

      gameHubConnection: '',
      presenceHubConnection: '',

      user: '',
      token: retrieveToken(),
      groupType: GroupType.OFFLINE,
      
      publicLobbyCount: 0,

      results: [],
      answerChoices: [],
      question: '',
      timer: '',
      endGame: ''
    }

    app = this
  }

  async componentDidMount() {
    showLoadingModal("Loading...")

    try {
      if (isNotNil(this.state.token)) {
        const user = await api.Account.get();
        setUser(user)
        presenceHubStartConnection()
      }
      hideLoadingModal()
    }
    catch (ex) {
      toast.error("An error has occurred!", { containerId: "info" })
      hideLoadingModal()
    }
  }

  render() {
    return (
      <>
      {isGroupType(GroupType.MAIN_LOBBY) && (
        <ToastContainer enableMultiContainer 
          containerId={"invite"}
          position="top-left"
          autoClose={25000}
          pauseOnFocusLoss={false}
          pauseOnHover={false}
        />
      )}
        <ToastContainer enableMultiContainer
          containerId={"info"}
          position="top-right"
          autoClose={10000}
        />
        <>
          <LoadingModal 
            show={this.state.loadingModalShow} 
            message={this.state.loadingModalMessage}
            showBtn={this.state.loadingModalBtnShow}
            btnAction={this.state.loadingModalBtnAction}
            btnActionName={this.state.loadingModalBtnActionName} 
          />
          <Header groupType={this.state.groupType} />
          <main className="bg-light d-flex align-items-center min-vh-100">
            <div className="container py-3">
              {!isGroupTypeInGame() ? (
                  <>
                    <Route exact path="/">
                      <Menu connection={this.state.presenceHubConnection} />
                    </Route>
                    <Route path={'/(.+)'} render={() => (
                        <>
                            <Switch>
                                <Route path={"/leaderboard"} render={() => isRole(Roles.USER) ? <Menu/> : <Redirect to='/' />} />
                            </Switch>
                        </>
                    )} />
                </>
              ) : 
              (<Game connection={this.state.gameHubConnection} 
                results={this.state.results} 
                question={this.state.question}
                timer={this.state.timer}
                answerChoices={this.state.answerChoices}
                endGame={this.state.endGame} />)}
            </div>
          </main>
          <Footer />
        </>
      </>
    )
  }
}

export default App;
