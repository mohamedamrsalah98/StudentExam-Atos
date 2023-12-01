import React, { useEffect, useState, useCallback, useContext, useRef } from "react";
import { useHistory } from "react-router-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { AuthContext } from "../Context/AuthContext";
import Cookies from "js-cookie";
import axios from "axios";
import { jwtDecode } from "jwt-decode";



function Login(){
    const history = useHistory();
    const { login } = useContext(AuthContext);
  
    useEffect(() => {
      const token = Cookies.get("token");
      if (token) {
        const decodedToken = jwtDecode(token);
        const userRole = decodedToken.roles;
        if (userRole === "User") {
            history.push("/student"); 
          } else if (userRole === "Admin") {
            history.push("/dashboard"); 
          }
      }
    }, []);
  
    const [info, setInfo] = useState({
      Email: "",
      Password: "",
    });
  

    
    const ChangeInfo = useCallback((e) => {
      setInfo((prevData) => ({ ...prevData, [e.target.name]: e.target.value }));
    }, []);
  
    const emailRef = useRef(null);
    const passwordRef = useRef(null);
  
    const ValidateUser = async (e) => {
      e.preventDefault();
      const email = emailRef.current.value;
      const password = passwordRef.current.value;
      if (email.length === 0 || password.length === 0) {
        toast.error("Please fill all fields");
      } else {
        try {
          const { data } = await axios.post("https://localhost:7206/token", info);
          if (data.token) {
            const decodedToken = jwtDecode(data.token);
            const userRole = decodedToken.roles;
            Cookies.set("token", data.token);
            login();
            if (userRole === "User") {
                history.push("/student"); 
              } else if (userRole === "Admin") {
                history.push("/dashboard"); 
              }
  
          }
        } catch ({ response: { data } }) {
          toast.error(data);
        }
      }
    };
  
    const submitUserData = (e) => {
      e.preventDefault();
    };
  
    return (
        <>
          <div className="container p-5 w-50" style={{ marginTop: "4%" }}>
            <div
              className=" p-5 rounded-5"
              style={{ boxShadow: "rgb(10 66 117) 1px -1px 15px 1px" }}
            >
              <h1
                className="text-center animate__animated animate__backInLeft"
                style={{ color: "#0a4275" }}
              >
                {" "}
                Log In
              </h1>
              <form
                method="post"
                id="loginForm"
                onSubmit={(e) => submitUserData(e)}
              >
                <div className="Email text-start mt-5">
                  <label
                    className="my-3 animate__animated animate__bounce ms-2 fs-4"
                    style={{ color: "#0a4275" }}
                  >
                    Email
                  </label>
                  <input
                    type="text"
                    name="Email"
                    id="Email"
                    className="form-control"
                    placeholder="Enter Email"
                    ref={emailRef}
                    onChange={(e) => ChangeInfo(e)}
                  />
                  <p className="text-danger mt-2 fw-bold"></p>
                </div>
                <div className="pass text-start">
                  <label
                    className="my-3 animate__animated animate__bounce ms-2 fs-4"
                    style={{ color: "#0a4275" }}
                  >
                    Password
                  </label>
                  <input
                    type="password"
                    name="Password"
                    id="pass"
                    className="form-control"
                    placeholder="Enter Password"
                    ref={passwordRef}
                    onChange={(e) => ChangeInfo(e)}
                  />
                  <p className="text-danger mt-2 fw-bold"></p>
                </div>
                <div className="text-center">
                  <button
                    type="submit"
                    className="mt-5 px-5 pb-2 m-auto btn btn-outline-primary"
                    onClick={(e) => ValidateUser(e)}
                  >
                    Log In
                  </button>
                </div>
              </form>
            </div>
          </div>
        </>
      );
}

export default Login