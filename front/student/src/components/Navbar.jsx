import React, { useContext } from "react";
import { Link } from "react-router-dom";
import { AuthContext } from "../Context/AuthContext";

function Navbar() {
  const { isLoggedIn, logout, userRole } = useContext(AuthContext);
  //const history = useHistory();


  const handleLogout = () => {
    logout();
  };
    // // Check if the current path is login or signup
    // const isLoginOrSignup = history.location.pathname === "/login" || history.location.pathname === "/signup";

    // // Don't render Navbar on login and signup pages
    // if (isLoginOrSignup) {
    //   return null;
    // }
  return (
    <nav className="navbar navbar-expand-lg" style={{ backgroundColor: "#0a4275" }}>
      <div className="container my-2">
        <h1  className="text-white fs-4 ms-3 text-decoration-none">
          Atos
        </h1>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarSupportedContent"
          aria-controls="navbarSupportedContent"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div
          className="collapse navbar-collapse d-flex justify-content-end"
          id="navbarSupportedContent"
        >
          <ul className="navbar-nav mb-2 mb-lg-0">
            {!isLoggedIn && (
              <>
                 <li className="nav-item">
                  <Link
                    className="nav-link active text-white fs-5"
                    aria-current="page"
                    to={"/"}
                  >
                    Login
                  </Link>
                </li>
                <li className="nav-item">
                  <Link
                    className="nav-link active text-white fs-5"
                    aria-current="page"
                    to={"/signup"}
                  >
                    SignUp
                  </Link>
                </li>
              </>
            )}
            {isLoggedIn && (
              <>
                {/* Common items for both User and Admin */}


                {/* Conditional items based on user role */}
                {userRole === "User" && (
                  <>
                 <li className="nav-item">
                      <Link
                        className="nav-link active text-white fs-5"
                        aria-current="page"
                        to={"/student"}
                      >
                        AddSubject
                      </Link>
                    </li>
                    <li className="nav-item">
                      <Link
                        className="nav-link active text-white fs-5"
                        aria-current="page"
                        to={"/startexam"}
                      >
                        StartExam
                      </Link>
                    </li>
                    <li className="nav-item">
                      <Link
                        className="nav-link active text-white fs-5"
                        aria-current="page"
                        to={"/examresult"}
                      >
                        ExamResult
                      </Link>
                    </li>
                  </>
                )}
                {userRole === "Admin" && (
                  <>
                <li>
                  <Link
                    className="nav-link active text-white fs-5"
                    aria-current="page"
                    to={"/dashboard"}
                  >
                    Dashboard
                  </Link>
                </li>
                <li>
                  <Link
                    className="nav-link active text-white fs-5"
                    aria-current="page"
                    to={"/studentscreen"}
                  >
                    StudentScreen
                  </Link>
                </li>
                <li>
                  <Link
                    className="nav-link active text-white fs-5"
                    aria-current="page"
                    to={"/examscreen"}
                  >
                    ExamScreen
                  </Link>
                </li>
                <li>
                  <Link
                    className="nav-link active text-white fs-5"
                    aria-current="page"
                    to={"/addexam"}
                  >
                    AddExam
                  </Link>
                </li>
                <li>
                  <Link
                    className="nav-link active text-white fs-5"
                    aria-current="page"
                    to={"/addquestion"}
                  >
                    AddQuestion
                  </Link>
                </li>
                <li>
                  <Link
                    className="nav-link active text-white fs-5"
                    aria-current="page"
                    to={"/addchoice"}
                  >
                    AddChoice
                  </Link>
                </li>
                  </>
                )}

                {/* Common item for both User and Admin */}
                <li className="nav-item">
                  <Link
                    to="/"
                    className="nav-link active text-white fs-5"
                    onClick={handleLogout}
                  >
                    Logout
                  </Link>
                </li>
              </>
            )}
          </ul>
        </div>
      </div>
    </nav>
  );
}

export default Navbar;
