import React from 'react';
import { Navigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';

const isAuthenticated = () => {
  const token = localStorage.getItem('token');
  if (!token) return false;

  try {
    // const { exp } = jwtDecode(token);
    // if (Date.now() >= exp * 1000) {
    //   localStorage.removeItem('token');
    //   return false;
    // }
    return true;
  } catch {
    return false;
  }
};

const PrivateRoute = ({ children }) => {
  return isAuthenticated() ? children : <Navigate to="/login" replace />;
};

export default PrivateRoute;