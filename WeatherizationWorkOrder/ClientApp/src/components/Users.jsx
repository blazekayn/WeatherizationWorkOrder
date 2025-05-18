import React, { Component, useState, useEffect, useRef } from 'react';
import { Grid } from './Table';
import { Input, Label, Button, Row, Col, Modal, Form,
  ModalHeader,
  ModalBody,
  ModalFooter,
  FormGroup, } from 'reactstrap';

export function Users() {
  const [rowData, setRowData] = useState([]);
  const [selectedRow, setSelectedRow] = useState();
  const [newName, setNewName] = useState();
  const [reload, setReload] = useState(false);
  const [modal, setModal] = useState(false);
  const [editName, setEditName] = useState();

  useEffect(() => {
    fetch(`user`)
      .then((result) => result.json())
      .then((data) => { setRowData(data)});
  }, [reload]);

  const toggle = () => {
    setModal(!modal);
  }

  const colDefs = [
    {
      field: "id",
      width: 90
    },
    {
      field: "name",
      width: 360,
    }
  ];

  const gridRef = useRef();

  const onSelectionChanged = (event) => {
    if(event.api.getSelectedNodes().length > 0){
      setSelectedRow(event.api.getSelectedNodes()[0].data);
    }else{
      setSelectedRow(null);
    }
  };

  const createUser = () => {
    fetch(`user`, {
      method: 'PUT',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(newName)
    })
    .then(response => {
      if(!response.ok){
        if(response.status == 400){
          alert("validation error");
        }
      }else{
        setNewName("");
        setReload(!reload);
      }
    })
  }

  const handleNewName = (e) => {
    setNewName(e.target.value);
  }

  const handleEditName = (e) => {
    setEditName(e.target.value);
  }

  const editUserModal = () => {
    if(selectedRow !== null && selectedRow?.name){
      setEditName(selectedRow?.name);
      toggle();
    }
  }
  
  const saveEditUser = () => {
    fetch(`user/${selectedRow.id}`, {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(editName)
    })
    .then(response => {
      if(!response.ok){
        if(response.status == 400){
          alert("validation error");
        }
      }else{
        toggle();
        setReload(!reload);
      }
    })
  }

  const deleteUser = () => {
    if(selectedRow){
      if(window.confirm(`Are you sure you want to delete ${selectedRow.name}?`)){
        fetch(`user/${selectedRow.id}`, {
          method: 'DELETE',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
          }
        })
        .then(response => {
            setReload(!reload);
        })
      }
    }
  }

  return (
    <>
    <Modal  isOpen={modal}
      toggle={toggle}>
      <ModalHeader toggle={toggle}>Edit User</ModalHeader>
      <ModalBody>
        <Form>
          <FormGroup row>
            <Label for="editName" sm={3}>
              Name:
            </Label>
            <Col sm={9}>
              <Input id="editName" name="editName" placeholder='Edit Name' type="textarea" value={editName} onChange={handleEditName}
                    rows="2"/>
            </Col>
          </FormGroup>
        </Form>
      </ModalBody>
      <ModalFooter>
        <Button color="primary" onClick={saveEditUser}>
          Save
        </Button>{' '}
        <Button color="secondary" onClick={toggle}>
          Cancel
        </Button>
      </ModalFooter>
    </Modal>
    <Row>
      <Col xs="5">
        <h2>Users
        </h2>
      </Col>
      <Col xs="3">
      <Row>
        <Col xs="7">
          <Input id="name" name="name" placeholder='New User' type="text" value={newName} onChange={handleNewName}/>
        </Col>
        <Col xs="5">
          <Button
            color="primary"
            onClick={createUser}
          >
            Add User
          </Button>
        </Col>
        </Row>
      </Col>
      <Col xs="4" style={{textAlign:"right"}}>
        <Button
          color="success"
          onClick={editUserModal}
          disabled={!selectedRow}
        >
          Edit User
        </Button>{' '}
        <Button
          color="danger"
          onClick={deleteUser}
          disabled={!selectedRow}
        >
          Delete User
        </Button>
        </Col>
      </Row>
      <div
        className={
          "ag-theme-quartz-dark"
        }
        style={{ width: "100%", height: "500px" }}
      >
        {rowData?.length > 0 ?
        <Grid 
          onSelectionChanged={onSelectionChanged}
          rowData={rowData} 
          colDefs={colDefs}
        />
        : <></>}
      </div>
    </>
  );

}
