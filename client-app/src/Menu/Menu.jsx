import { isNotNil } from "../services/util";
import UserCard from "../UI/UserCard";
import MenuNotSigned from "./MenuNotSigned";
import MenuSigned from "./MenuSigned";

const Menu = (props) => {
    
  return (
    <>
      {isNotNil(props.connection) ? (
        <>
          <UserCard />
          <MenuSigned connection={props.connection} />
        </>
      ) : (
        <MenuNotSigned />
      )}
    </>
  );
};

export default Menu;
