import React, { Component } from 'react';
import { Route, Routes, Navigate } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';
import {PrintItems} from './components/PrintItems';
import {PrintWos} from './components/PrintWos';
import {PrintWo} from './components/PrintWo';

export default class App extends Component {
  static displayName = App.name;


  render() {

    return (
        <Routes>
          <Route element={<Layout/>}>
            {AppRoutes.map((route, index) => {
              const { element, ...rest } = route;
              return <Route key={index} {...rest} element={element} />;
            })}
          </Route>
          <Route
            path="/printitems"
            element={<PrintItems/>}
          />
          <Route
            path="/printwos"
            element={<PrintWos/>}
          />
          <Route
            path="/printwo"
            element={<PrintWo/>}
          />
          <Route
            path="*"
            element={<Navigate to="/work-order" replace={true} />}
          />
        </Routes>
    );
  }
}
