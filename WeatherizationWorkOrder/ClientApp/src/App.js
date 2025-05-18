import React, { Component } from 'react';
import { Route, Routes, Navigate } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';
import { PrintItems } from './components/PrintItems';
import { PrintWos } from './components/PrintWos';
import { PrintWo } from './components/PrintWo';
import LoginPage from './components/LoginPage'; 
import PrivateRoute from './components/PrivateRoute';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Routes>
        <Route path="/login" element={<LoginPage />} />

        <Route
          element={
            <PrivateRoute>
              <Layout />
            </PrivateRoute>
          }
        >
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
          })}
        </Route>

        <Route path="/printitems" element={<PrivateRoute><PrintItems /></PrivateRoute>} />
        <Route path="/printwos" element={<PrivateRoute><PrintWos /></PrivateRoute>} />
        <Route path="/printwo" element={<PrivateRoute><PrintWo /></PrivateRoute>} />

        <Route path="*" element={<Navigate to="/work-order" replace />} />
      </Routes>
    );
  }
}