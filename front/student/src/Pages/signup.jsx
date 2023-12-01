import axios from "axios";
import { useCallback, useRef, useState } from "react";
import { useHistory } from "react-router-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const initialStateInfo = {
    Username: "",
    Email: "",
    Password: "",
    repeatpassword: "",

  };


function SignUp() {
    const [info, setInfo] = useState(initialStateInfo);

    const history = useHistory();
    const usernameRef = useRef(null);
    const emailRef = useRef(null);
    const passwordRef = useRef(null);
    const repeatpasswordRef = useRef(null);


    const ChangeInfo = useCallback((e) => {
        const { name, value } = e.target;
        setInfo((prevData) => ({ ...prevData, [name]: value }));
      }, []);

    const ValidateUser = useCallback(
        async (e) => {
          e.preventDefault();
          const Username = usernameRef.current.value;
          const email = emailRef.current.value;
          const password = passwordRef.current.value;
          const repeatpassword = repeatpasswordRef.current.value;

    
          if (
            Username.length === 0 ||
            email.length === 0 ||
            password.length === 0 ||
            repeatpassword.length === 0

          ) {
            toast.error("Please fill all fields");
          } else {
                if (password !== repeatpassword)
                    {
                        toast.error("Passwords do not match")
                    }
                else if (!/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(email))
                    {
                        toast.error("Invalid email format")
                    }
                 else {
                        try {
                            const { data } = await axios.post(
                            "https://localhost:7206/register",
                            info
                            );
                            toast.success("User Added successfully");
                            history.push("/");
                        } catch ({ response: { data } }) {
                            toast.error(data);

              }
            }
          }
        },
        [info, history]
      );
      const submitUserData = (e) => {
        e.preventDefault();
      };
    return(
        <>
              <div className="container p-5 w-50" style={{ marginTop: "3%" }}>
        <div
          className=" p-5 rounded-5"
          style={{ boxShadow: "rgb(10 66 117) 1px -1px 15px 1px" }}
        >
          <h1
            className="text-center animate__animated animate__backInLeft"
            style={{ color: "#0a4275" }}
          >
            {" "}
            Sign Up
          </h1>

          <form
            method="post"
            id="loginForm"
            onSubmit={(e) => submitUserData(e)}
          >
            <div className="UserName text-start">
              <label
                className="my-3 animate__animated animate__bounce ms-2 fs-4"
                style={{ color: "#0a4275" }}
              >
                User Name
              </label>
              <input
                type="text"
                name="Username"
                id="Username"
                className="form-control"
                placeholder="Enter UserName"
                ref={usernameRef}
                onChange={(e) => ChangeInfo(e)}
              />
              <p className="text-danger mt-2 fw-bold"></p>
            </div>
            <div className="email text-start">
              <label
                className="my-3 animate__animated animate__bounce ms-2 fs-4"
                style={{ color: "#0a4275" }}
              >
                Email
              </label>
              <input
                type="text"
                name="Email"
                id="email"
                className="form-control"
                placeholder="Enter Email"
                ref={emailRef}
                onChange={(e) => ChangeInfo(e)}
              />
              <p className="text-danger mt-2 fw-bold"></p>
            </div>
            <div className="password text-start">
              <label
                className="my-3 animate__animated animate__bounce ms-2 fs-4"
                style={{ color: "#0a4275" }}
              >
                Password
              </label>
              <input
                type="password"
                name="Password"
                id="password"
                className="form-control"
                placeholder="Enter Password"
                ref={passwordRef}
                onChange={(e) => ChangeInfo(e)}
              />
              <p className="text-danger mt-2 fw-bold"></p>
            </div>
            <div className="repeatpassword text-start">
              <label
                className="my-3 animate__animated animate__bounce ms-2 fs-4"
                style={{ color: "#0a4275" }}
              >
                Repeat Password
              </label>
              <input
                type="password"
                name="repeatpassword"
                id="repeatpassword"
                className="form-control"
                placeholder="Enter Repeat Password"
                ref={repeatpasswordRef}
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
                Sign Up
              </button>
            </div>
          </form>
        </div>
      </div>
        </>
    )
}


export default SignUp;
