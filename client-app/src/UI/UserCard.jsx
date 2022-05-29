import { Component } from 'react';
import { isRole } from '../App';
import { Roles } from '../services/contants';

class UserCard extends Component {

    constructor(props) {
        super(props)
        
        this.state = {
            username : '',
            wins: 0,
            games: 0,
            lastlyPlayed: ''
        }
    }

    componentDidMount() {}

    render() {
        return (
            <div className="row justify-content-center">
                <div className="card mb-3 border border-warning border-5 bg-dark" style={{width: "60%"}}>
                    <div className="row g-0">
                        <div className="col-md-4 d-flex justify-content-center align-items-center">
                            <div className="text-center border border-info border border-3 bg-white m-2 p-5">
                                {isRole(Roles.USER) ? <i className="fa fa-user fa-4x" /> : <i className="fa fa-user-secret fa-4x" />}
                            </div>
                        </div>
                        <div className="col-md-8 text-white">
                            <div className="card-body">
                                <p className="card-title fa-2x">{this.state.username}</p>
                                <p className="card-text fa-2x"><i className="fa fa-trophy"></i> - {this.state.wins}</p>
                                <p className="card-text fa-2x"><i className="fa fa-gamepad"></i> - {this.state.games}</p>
                                <p className="card-text"><small className="text-white">Lastly played on: {this.state.lastlyPlayed}</small></p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

export default UserCard;