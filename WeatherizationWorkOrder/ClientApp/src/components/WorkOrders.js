import React, { Component, useState, useEffect } from "react";
import { Grid } from "./Table";
import {
  Input,
  Label,
  Button,
  Row,
  Col,
  Modal,
  Form,
  ModalHeader,
  ModalBody,
  ModalFooter,
  FormGroup,
} from "reactstrap";

export function WorkOrders() {
  const [rowData, setRowData] = useState([]);
  const [modal, setModal] = useState(false);
  const [reload, setReload] = useState(false);
  const [selectedRow, setSelectedRow] = useState();
  const [description, setDescription] = useState("");
  const [consumer, setConsumer] = useState("");
  const [userData, setUserData] = useState([]);
  const [preparedBy, setPreparedBy] = useState("");
  const [globalUser, setGlobalUser] = useState([]);
  const [itemData, setItemData] = useState([]);
  const [selectedItem, setSelectedItem] = useState({});
  const [newMatrialData, setNewMaterialData] = useState([]);
  const [newWoId, setNewWoId] = useState("");
  const [newMaterialAmount, setNewMaterialAmount] = useState("");
  const [editWorkOrderData, setEditWorkOrderData] = useState({});

  useEffect(() => {
    fetch(`workOrder`)
      .then((result) => result.json())
      .then((data) => {
        setRowData(data);
      });
  }, [reload]);

  useEffect(() => {
    fetch(`user`)
      .then((result) => result.json())
      .then((data) => {
        console.log(data);
        setUserData(data);
      });
  }, []);

  useEffect(() => {
    fetch(`inventory?showOOS=false&unique=true`)
      .then((result) => result.json())
      .then((data) => {
        console.log(data);
        setItemData(data);
      });
  }, []);

  useEffect(() => {
    let gName = localStorage.getItem("gUserName");
    if (gName) {
      setGlobalUser(gName);
    }
  });

  const toggle = () => {
    setModal(!modal);
    setReload(!reload);
  };

  /* Format Date Cells */
  const dateFormatter = (params) => {
    return new Date(params.value).toLocaleDateString("en-us", {
      month: "2-digit",
      day: "2-digit",
      year: "numeric",
    });
  };

  const materialColDefs = [
    {
      field: "description",
      width: "100",
    },
    {
      field: "costPer",
      width: "90",
    },
    {
      field: "amountUsed",
      width: "75",
    },
    {
      field: "total",
      width: "100",
    },
  ];

  const colDefs = [
    {
      field: "id",
      width: 90,
    },
    {
      field: "consumer",
      width: 200,
    },
    {
      field: "preparedBy",
      width: 200,
    },
    {
      field: "description",
      width: 360,
    },
    {
      field: "preparedDate",
      width: 175,
      valueFormatter: dateFormatter,
    },
  ];

  const onSelectionChanged = (event) => {
    if (event.api.getSelectedNodes().length > 0) {
      setSelectedRow(event.api.getSelectedNodes()[0].data);
    } else {
      setSelectedRow(null);
    }
  };
  const handleDescription = (e) => {
    setDescription(e.target.value);
  };
  const handleConsumer = (e) => {
    setConsumer(e.target.value);
  };
  const handlePreparedBy = (e) => {
    setPreparedBy(e.target.value);
  };
  const handleNewMaterialAmount = (e) => {
    setNewMaterialAmount(e.target.value);
  };

  const handleSelectedItemChanged = (e) => {
    var item = itemData.find(
      (i) => `${i.description} (${i.units})` === e.target.value
    );
    console.log(e.target.value, item);
    setSelectedItem(item);
  };

  const addItemToMaterials = () => {
    fetch(`workOrder/AddMaterial`, {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        description: selectedItem.description,
        units: selectedItem.units,
        woId: newWoId,
        used: newMaterialAmount,
      }),
    }).then((response) => {
      if (!response.ok) {
        if (response.status == 400) {
          console.log("validation error");
        }
      } else {
        //TODO reload material table
      }
    });
  };

  const createWorkOrder = () => {
    fetch(`workOrder`, {
      method: "PUT",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        consumer: "",
        preparedBy: globalUser,
        description: "",
      }),
    })
      .then((response) => response.json())
      .then((data) => {
        setNewWoId(data);
        toggle();
      });
  };

  const editWorkOrder = () => {
    fetch(`workOrder/${selectedItem}`)
      .then((result) => result.json())
      .then((data) => {
        setEditWorkOrderData(data);
      });
  };

  const saveWorkOrder = () => {
    fetch(`workOrder`, {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        id: newWoId,
        consumer: consumer,
        preparedBy: preparedBy,
        description: description,
        lastModifiedBy: globalUser,
      }),
    }).then((response) => {
      if (!response.ok) {
        if (response.status == 400) {
          console.log("validation error");
        }
      } else {
        toggle();
      }
    });
  };

  return (
    <>
      <Modal size="xl" isOpen={modal} toggle={toggle}>
        <ModalHeader toggle={toggle}>New Work Order</ModalHeader>
        <ModalBody>
          <Form>
            <FormGroup row>
              <Label for="consumer" sm={3}>
                Consumer:
              </Label>
              <Col sm={4}>
                <Input
                  id="consumer"
                  name="consumer"
                  type="text"
                  value={consumer}
                  onChange={handleConsumer}
                />
              </Col>
            </FormGroup>
            <FormGroup row>
              <Label for="description" sm={3}>
                Description:
              </Label>
              <Col sm={9}>
                <Input
                  id="description"
                  name="description"
                  placeholder="Description"
                  type="textarea"
                  value={description}
                  onChange={handleDescription}
                  rows="2"
                />
              </Col>
            </FormGroup>
            <FormGroup row>
              <Label for="preparedBy" sm={3}>
                Prepared By:
              </Label>
              <Col sm={9}>
                <Input
                  type="select"
                  name="preparedBy"
                  id="preparedBy"
                  onChange={handlePreparedBy}
                >
                  {userData.map((user) => (
                    <option
                      value={user.name}
                      key={user.id}
                      selected={globalUser == user.name}
                    >
                      {user.name}
                    </option>
                  ))}
                </Input>
              </Col>
            </FormGroup>
          </Form>
          <h3>Materials</h3>
          <Row>
            <Form>
              <FormGroup row>
                <Label for="consumer" sm={1}>
                  Item:
                </Label>
                <Col sm={5}>
                  <Input
                    type="select"
                    name="addItemSelect"
                    id="addItemSelect"
                    onChange={handleSelectedItemChanged}
                  >
                    {itemData.map((item) => (
                      <option
                        value={`${item.description} (${item.units})`}
                        key={`${item.description} (${item.units})`}
                      >{`${item.description} (${item.units})`}</option>
                    ))}
                  </Input>
                </Col>
                <Col>
                    {selectedItem.remaining}
                </Col>
                <Label for="amount" sm={1}>
                  Amount:
                </Label>
                <Col sm={2}>
                  <Input
                    id="amount"
                    name="amount"
                    type="number"
                    value={newMaterialAmount}
                    onChange={handleNewMaterialAmount}
                  />
                </Col>
                <Label sm={1}>{selectedItem.units}</Label>
                <Col sm={1}>
                  <Button color="primary" onClick={addItemToMaterials}>
                    Add
                  </Button>
                </Col>
              </FormGroup>
            </Form>
          </Row>
          <Grid
            height="250px"
            onSelectionChanged={onSelectionChanged}
            rowData={newMatrialData}
            colDefs={materialColDefs}
          />
          <br />
          <h3>Labor</h3>
          <Row>
            <Form>
              <FormGroup row>
                <Label for="consumer" sm={1}>
                  Resource:
                </Label>
                <Col sm={3}>
                  <Input type="text"></Input>
                </Col>
                <Label for="cost" sm={1}>
                  Cost:
                </Label>
                <Col sm={2}>
                  <Input id="cost" name="cost" type="number" />
                </Col>
                <Label for="hours" sm={1}>
                  Hours:
                </Label>
                <Col sm={2}>
                  <Input id="hours" name="hours" type="number" />
                </Col>
                <Col sm={1}>
                  <Button color="primary" onClick={toggle}>
                    Add
                  </Button>
                </Col>
              </FormGroup>
            </Form>
          </Row>
          <Grid
            height="250px"
            onSelectionChanged={onSelectionChanged}
            rowData={rowData}
            colDefs={materialColDefs}
          />
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={saveWorkOrder}>
            Save
          </Button>{" "}
          <Button color="secondary" onClick={toggle}>
            Cancel
          </Button>
        </ModalFooter>
      </Modal>
      <Row>
        <Col xs="8">
          <h2>Work Orders</h2>
        </Col>
        <Col xs="4" style={{ textAlign: "right" }}>
          <Row>
            <Col>
              <Button color="primary" onClick={createWorkOrder}>
                New WO
              </Button>
            </Col>
            <Col>
              <Button onClick={editWorkOrder} color="success">
                Edit WO
              </Button>
            </Col>
            <Col>
              <Button color="danger">Delete WO</Button>
            </Col>
          </Row>
        </Col>
      </Row>
      <div
        className={"ag-theme-quartz-dark"}
        style={{ width: "100%", height: "500px" }}
      >
        {rowData?.length > 0 ? (
          <Grid
            onSelectionChanged={onSelectionChanged}
            rowData={rowData}
            colDefs={colDefs}
          />
        ) : (
          <></>
        )}
      </div>
    </>
  );
}
