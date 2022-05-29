import "./Footer.css";

const Footer = () => {
    return (
        <div className="footer">
            <div className="p-3 text-center">
                <section>
                    <div className="row">
                        <div className="col-sm p-1">
                            <a className="btn btn-info btn-outline-warning m-2" title="Code Repository" href="#!">
                                <i className="fa-solid fa-code-branch fa-lg text-white p-1 fw-bold"></i>
                            </a>
                            <a className="btn btn-info btn-outline-warning m-2" title="Contact" href="#!">
                                <i className="fa-solid fa-comment-medical fa-lg text-white p-1 fw-bold"></i>
                            </a>
                            <a className="btn btn-info btn-outline-warning m-2" title="The Trivia Api" href="#!">
                                <i className="fa-solid fa-clipboard-question fa-lg text-white p-1 fw-bold"></i>
                            </a>
                        </div>
                        <div className="col-sm p-1">
                            <div className="text-center">
                               <p>© 2022 Copyright - Signal To Answer</p>
                            </div>
                        </div>
                        <div className="col-sm p-1">
                            <div className="text-center">
                                <p>Work In Progress</p>
                            </div>
                        </div>
                    </div>
                 </section>
            </div>
        </div>
    )
}

export default Footer;
