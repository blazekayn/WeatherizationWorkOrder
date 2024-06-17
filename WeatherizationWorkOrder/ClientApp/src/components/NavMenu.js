import React, { useState, useEffect } from 'react';
import { Navbar, NavbarBrand, NavItem, NavLink, Input, Container, Row, Col } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

const changeGlobalUser = (e) => {
  let { value } = e.target;
  localStorage.setItem("gUserName", value);
}

export function NavMenu() {
  const [userData, setUserData] = useState([]);
  const [globalUser, setGlobalUser] = useState([]);

  useEffect(() => {
    fetch(`user`)
      .then((result) => result.json())
      .then((data) => { console.log(data); setUserData(data)});
  }, []);

  useEffect(() => {
    let gName = localStorage.getItem("gUserName");
    if(gName){
      setGlobalUser(gName);
    }
  })

    return (
      <header>
        <Navbar className="navbar-expand-sm" container light>
          <Container>
            <Row className="align-items-center">
              <Col xs="8">
              <NavbarBrand tag={Link} to="/">Weatherization Work Orders</NavbarBrand>
              </Col>
              <Col xs="1">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/items">Inventory</NavLink>
              </NavItem>
              </Col>
              <Col xs="1">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/work-order">Work Order</NavLink>
              </NavItem>
              </Col>
              <Col xs="2">
              <Input
                type="select"
                name="currentUser"
                id="currentUser"
                onChange={changeGlobalUser}
              >
                {userData.map((user) =>
                    <option value={user.name} key={user.id} selected={globalUser == user.name}>{user.name}</option>
                  )
                }
              </Input>
              </Col>

            </Row>
           </Container>
        </Navbar>
      </header>
    );
}
