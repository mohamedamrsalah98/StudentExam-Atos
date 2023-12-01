import React, { useState, createContext, useEffect } from "react";
import Cookies from "js-cookie";
import { jwtDecode } from "jwt-decode";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [isLoggedIn, setIsLoggedIn] = useState(!!Cookies.get("token"));
  const [userRole, setUserRole] = useState(null);

  useEffect(() => {
    const token = Cookies.get("token");
    if (token) {
      const decodedToken = jwtDecode(token);
      setUserRole(decodedToken.roles);
    }
  }, [isLoggedIn]);

  const login = () => {
    setIsLoggedIn(true);
  };

  const logout = () => {
    Cookies.remove("token");
    setIsLoggedIn(false);
    setUserRole(null); 
    localStorage.removeItem("takenSubjects");

  };

  return (
    <AuthContext.Provider value={{ isLoggedIn, login, logout, userRole }}>
      {children}
    </AuthContext.Provider>
  );
};
