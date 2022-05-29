import { isGroupType } from "../App";
import { GroupType } from "../services/contants";
import "./Header.css"

const Header = (props) => {
    const groupType = props.groupType

    return (
        <div className="navbar navbar-header">
            <div className="container p-1 justify-content-center">
                <div className="navbar-brand navbar-brand-header fs-3">
                    <span>Signal To Answer {!isGroupType(GroupType.OFFLINE) && (<span>- [{groupType.name}]</span>)}</span>
                </div>
            </div>
        </div>
    )
}

export default Header;