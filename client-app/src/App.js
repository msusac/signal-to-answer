import { Component } from 'react';
import { toast, ToastContainer } from 'react-toastify';
import LoadingModal from './Modals/LoadingModal';
import Header from './Other/Header';
import Footer from './Other/Footer';
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import api from './services/api';
import { isNotNil } from './services/util';
import InfoModal from './Modals/InfoModal';
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

export function hideInfoModal() {
  app.setState({ infoModalShow: false, infoModalMessage: '', infoModalHasError: false, infoModalAction: null })
}

export function showInfoModal(message, hasError, action) {
  app.setState({ infoModalShow: true, infoModalMessage: message, infoModalHasError: hasError, infoModalAction: action })
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

// Game Hub
export function gameHubStartConnection(gameId) {
  const connection = new HubConnectionBuilder()
    .withUrl("http://localhost:5000/hub/game-hub?gameId=" + gameId, {
       accessTokenFactory: () => app.state.token
    })
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Information)
    .build()

  try {
    connection.start()
    app.setState({ gameHubConnection: connection })
    hideLoadingModal()
  }
  catch (ex) {
    hideLoadingModal()
    showInfoModal("An error has occurred during connection to game.", true, () => {
      hideInfoModal()
    })
  }
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

  try {
    connection.start()
    hideLoadingModal()
  }
  catch (ex) {
    hideLoadingModal()
    showInfoModal("An error has occurred during connection.", true, () => {
      hideInfoModal()
    })
  }

  connection.on("ReceiveGroupType", (groupId) => {
    try {
      app.setState({ groupType: Object.values(GroupType).find(a => a.id === groupId) })

      if (isGroupType(GroupType.OFFLINE)) {
        app.setState({ presenceHubConnection: null, gameHubConnection: null })
      }
      else {
        app.setState({ presenceHubConnection: connection, gameHubConnection: null })
      }
    }
    catch (ex) {
      showInfoModal("An error has occurred during connection.", true, () => {
        hideInfoModal()
      })
    }
  });

  connection.on("ReceivePublicLobbyCount", (count) => {
    try {
      app.setState({ publicLobbyCount: count })
    }
    catch (ex) {
      showInfoModal("An error has occurred while retrieving player count.", true, () => {
        hideInfoModal()
      })
    }
  })

  connection.on("ReceivePublicGame", (gameId) => {
    showLoadingModal("Connecting to public game...");

    try {
      gameHubStartConnection(gameId)
      hideLoadingModal()
    }
    catch (ex) {
      hideLoadingModal();
      showInfoModal("An error has occurred during joining public game.", true, () => {
        hideInfoModal()
      })
    }
  });

  connection.on("ReceivePrivateGame", (gameId) => {
    showLoadingModal("Connecting to private game...");

    try {
      gameHubStartConnection(gameId)
      hideLoadingModal()
    }
    catch (ex) {
      hideLoadingModal();
      showInfoModal("An error has occurred during joining private game.", true, () => {
        hideInfoModal()
      })
    }
  });

  connection.on("ReceivePrivateGameInvite", (fromUser, gameId, groupId) => {
    app.setState({ toastMsgTimeEnabled: true })

    try {
      const audio = new Audio(notificationAudio)
      audio.play()
    
      toast.info(<PrivateGameJoinToast fromUser={fromUser} gameId={gameId} groupId={groupId} />)
    }
    catch (ex) {
      showInfoModal("An error has occurred while receiving private game invite.", true, () => {
        hideInfoModal()
      })
    }
  })

  connection.on("ReceivePrivateGameCancelled", (message) => {
    hideLoadingModal()
    app.setState({ toastMsgTimeEnabled: false })

    try {
      toast.info("Private game was cancelled for following reason: " + message)
    }
    catch (ex) {
      showInfoModal("An error has occurred while receiving private game invite.", true, () => {
        hideInfoModal()
      })
    }
  })

  connection.on("ReceivePrivateGameLoadingMessage", (message) => {
    app.setState({ loadingModalMessage: message })
  })
}

export function presenceHubStopConnection() {
  try {
    const connection = app.state.presenceHubConnection
    connection.stop()

    app.setState({ presenceHubConnection: null, gameHubConnection: null, groupType: GroupType.OFFLINE })
  }
  catch (ex) {
    showInfoModal("An error has occurred while disconnecting.", true, () => {
      hideInfoModal()
    })
  }
}

export function presenceHubChangeGroupUnique(groupType) {
  try {
    app.state.presenceHubConnection.invoke("ChangeGroupUnique", groupType.id)
  }
  catch (ex) {
    showInfoModal("An error has occurred while changing connection groupType.", true, () => {
      hideInfoModal()
    })
  }
}

export function retrievePublicLobbyCount() {
  return app.state.publicLobbyCount
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

      infoModalShow: false,
      infoModalMessage: '',
      infoModalHasError: false,
      infoModalAction: '',

      gameHubConnection: '',
      presenceHubConnection: '',

      user: '',
      token: retrieveToken(),
      groupType: GroupType.OFFLINE,
      
      publicLobbyCount: 0,

      toastMsgTime: 25000,
      toastMsgTimeEnabled: false,
      toastMsgPause: false
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
      hideLoadingModal()
      showInfoModal("An error has occurred!", true, () => {
        hideInfoModal();
      });
    }
  }

  render() {
    return (
      <>
        {this.state.toastMsgTimeEnabled ? (
          <ToastContainer 
            position="top-left"
            autoClose={this.state.toastMsgTime}
            pauseOnFocusLoss={this.state.toastMsgPause}
            pauseOnHover={this.state.toastMsgPause}  
          />
        )
        :
        (
          <ToastContainer position="top-left" />
        )}
        <InfoModal show={this.state.infoModalShow} 
          message={this.state.infoModalMessage} 
          hasError={this.state.infoModalHasError} 
          action={this.state.infoModalAction} 
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
              (<Game connection={this.state.gameHubConnection} />)}
            </div>
          </main>
          <Footer />
        </>
      </>
    )
  }
}

export default App;
