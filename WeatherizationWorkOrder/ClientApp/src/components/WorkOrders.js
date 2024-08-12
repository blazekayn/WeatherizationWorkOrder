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
  const [printModal, setPrintModal] = useState(false);
  const [reload, setReload] = useState(false);
  const [description, setDescription] = useState("");
  const [consumer, setConsumer] = useState("");
  const [userData, setUserData] = useState([]);
  const [preparedBy, setPreparedBy] = useState("");
  const [globalUser, setGlobalUser] = useState([]);
  const [itemData, setItemData] = useState([]);
  const [selectedItem, setSelectedItem] = useState(-1);
  const [newMatrialData, setNewMaterialData] = useState([]);
  const [newWoId, setNewWoId] = useState("");
  const [newMaterialAmount, setNewMaterialAmount] = useState("");
  const [materialCostTotal, setMaterialCostTotal] = useState("");
  const [selectedMaterial, setSelectedMaterial] = useState(null);
  const [laborCostTotal, setLaborCostTotal] = useState("");
  const [laborData, setLaborData] = useState([]);
  const [laborResource, setLaborResource] = useState("");
  const [laborHours, setLaborHours] = useState("");
  const [laborCost, setLaborCost] = useState("");
  const [selectedLabor, setSelectedLabor] = useState(null);
  const [workDate, setWorkDate] = useState(null);
  const [selectedItemDDValue,setSelectedItemDDValue] = useState(-1);
  const [printFromDate, setPrintFromDate] = useState("");
  const [printToDate, setPrintToDate] = useState("");

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
    setSelectedMaterial(null);
  };

  const togglePrint = () => {
    setPrintModal(!printModal);
  }

  const formatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
  });

  const moneyFormatter = (params) => {
    return formatter.format(params.value);
  }

  /* Format Date Cells */
  const dateFormatter = (params) => {
    if(params.value){
      return new Date(params.value).toLocaleDateString("en-us", {
        month: '2-digit',day: '2-digit',year: 'numeric'});
    }else{
      return "-";
    }
  };

  const numberFormatter = (params) => {
    return params.value.toFixed(2);
  }

  const materialColDefs = [
    {
      field: "description",
      width: "400",
    },
    {
      field: "costPer",
      width: "150",
      valueFormatter: moneyFormatter,
      type: 'rightAligned',
    },
    {
      field: "units",
      width: "150",
    },
    {
      field: "amountUsed",
      width: "150",
      type: 'rightAligned',
      valueFormatter: numberFormatter,
    },
    {
      field: "total",
      width: "150",
      valueFormatter: moneyFormatter,
      type: 'rightAligned',
    },
  ];

  const laborColDefs = [
    {
      field: "resource",
      width: "400",
    },
    {
      field: "cost",
      width: "150",
      valueFormatter: moneyFormatter,
      type: 'rightAligned',
    },
    {
      field: "hours",
      width: "150",
      type: 'rightAligned',
      valueFormatter: numberFormatter,
    },
    {
      field: "total",
      width: "150",
      valueFormatter: moneyFormatter,
      type: 'rightAligned',
    },
  ];

  const colDefs = [
    {
      field: "id",
      width: 65,
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
      field: "workDate",
      width: 150,
      valueFormatter: dateFormatter,
    },
    {
      field: "preparedDate",
      width: 150,
      valueFormatter: dateFormatter,
    },
    {
      field: "materialCost",
      width: 130,
      valueFormatter: moneyFormatter,
      type: 'rightAligned',
    },
    {
      field: "laborCost",
      width: 130,
      valueFormatter: moneyFormatter,
      type: 'rightAligned',
    },
    {
      field: "totalCost",
      width: 130,
      valueFormatter: moneyFormatter,
      type: 'rightAligned',
    },
  ];

  const selectedMaterialChanged = (event) => {
    if (event.api.getSelectedNodes().length > 0) {
      const selected = event.api.getSelectedNodes()[0].data;
      console.log(selected.id);
      setSelectedMaterial(selected.id);
    } else {
      setSelectedMaterial(null);
    }
  }

  
  const selectedLaborChanged = (event) => {
    if (event.api.getSelectedNodes().length > 0) {
      const selected = event.api.getSelectedNodes()[0].data;
      console.log(selected.id);
      setSelectedLabor(selected.id);
    } else {
      setSelectedLabor(null);
    }
  }

  const onSelectionChanged = (event) => {
    if (event.api.getSelectedNodes().length > 0) {
      const newRow = event.api.getSelectedNodes()[0].data;
      console.log(newRow.id);
      editWorkOrder(newRow.id);
      setModal(true);
    } else {
      setModal(false);
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
  const handleLaborResource = (e) => {
    setLaborResource(e.target.value);
  };
  const handleLaborCost = (e) => {
    setLaborCost(e.target.value);
  };
  const handleLaborHours = (e) => {
    setLaborHours(e.target.value);
  };
  const handleWorkDate = (e) => {
    setWorkDate(e.target.value);
  }
  const handleFromDate = (e) => {
    setPrintFromDate(e.target.value);
  }
  const handleToDate = (e) => {
    setPrintToDate(e.target.value);
  }

  const updateMaterials = (materials) => {
    setNewMaterialData(materials);
    setMaterialCostTotal(materials?.reduce((n, {total}) => n + total, 0));
    setNewMaterialAmount("");
    setSelectedItem(-1);
    setSelectedItemDDValue(-1);

    fetch(`inventory?showOOS=false&unique=true`)
      .then((result) => result.json())
      .then((data) => {
        setItemData(data);
      });
  }

  const updateLabor = (labor) => {
    setLaborData(labor);
    setLaborCostTotal(labor?.reduce((n, {total}) => n + total, 0));
  }

  const handleSelectedItemChanged = (e) => {
    var item = itemData.find(
      (i) => `${i.description} (${i.units})` === e.target.value
    );
    console.log(e.target.value, item);
    setSelectedItem(item);
    setSelectedItemDDValue(e.target.value);
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
    }).then((response) => response.json())
    .then((data) => {
      console.log("in here", data);
      if(!data.success){
        alert(data.message);
      }else{
        updateMaterials(data.materials);
      }
    });
  };

  const addLabor = () => {
    fetch(`workOrder/AddLabor`, {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        woId: newWoId,
        resource: laborResource,
        cost: laborCost,
        hours: laborHours,
      }),
    }).then((response) => response.json())
    .then((data) => {
      updateLabor(data);
      setLaborResource("");
      setLaborCost("");
      setLaborHours("");
    });
  }

  const createWorkOrder = () => {
    setConsumer('');
    setDescription('');
    setNewMaterialData([]);
    setLaborData([]);
    setWorkDate("");
    setMaterialCostTotal("");
    setLaborCostTotal("");
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
        setPreparedBy(globalUser);
        toggle();
      });
  };

  const editWorkOrder = (id) => {
    if(id === null) return;
    setNewWoId(id);
    fetch(`workOrder/${id}`)
      .then((result) => result.json())
      .then((data) => {
        console.log(data);
        setConsumer(data.consumer);
        setDescription(data.description);
        setPreparedBy(data.preparedBy);
        updateMaterials(data.materials);
        updateLabor(data.labors);
        var date = new Date(data.workDate);
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are 0-indexed, so add 1
        const day = String(date.getDate()).padStart(2, '0');

        // Format as YYYY-MM-DD
        const formattedDate = `${year}-${month}-${day}`;
        setWorkDate(formattedDate);
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
        workDate: workDate,
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

  const deleteWorkOrder = () => {
    const doit = window.confirm("Are you sure you want to Delete this Work Order? It cannot be restored.");
    if(doit){
      fetch(`workOrder/${newWoId}`, {
        method: "DELETE",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
        },
      }).then((response) => {
        if (!response.ok) {
          if (response.status == 400) {
            console.log("validation error");
          }
        } else {
          toggle();
        }
      })
    }
  }

  const deleteMaterial = () => {
    const doit = window.confirm("Are you sure you want to Delete? It cannot be restored. Materials will be re-added to inventory.");
    if(doit && selectedMaterial){
      fetch(`workOrder/materials/${selectedMaterial}`, {
        method: "DELETE",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
        },
      }).then((response) => response.json())
      .then((data) => {
        updateMaterials(data);
      });
    }
  }

  const deleteLabor = () => {
    const doit = window.confirm("Are you sure you want to Delete?");
    if(doit && selectedLabor){
      fetch(`workOrder/labor/${selectedLabor}`, {
        method: "DELETE",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
        },
      }).then((response) => response.json())
      .then((data) => {
        console.log(data);
        updateLabor(data);
      });
    }
  }

  const print = () => {
    window.location = `/#/printwos?from=${encodeURIComponent(printFromDate)}&to=${encodeURIComponent(printToDate)}`;
  }

  const printWo = () => {
    window.location = `/#/printwo?id=${newWoId}`;
  }

  return (
    <>
      <Modal size="md" isOpen={printModal} toggle={togglePrint}>
        <ModalBody>
          <Form>
          <FormGroup row>
              <Label for="fromDate" sm={4}>
                From Date:
              </Label>
              <Col sm={6}>
                <Input id="fromDate" name="fromDate" type="date" value={printFromDate} onChange={handleFromDate}/>
              </Col>
            </FormGroup>
            <FormGroup row>
              <Label for="toDate" sm={4}>
                To Date:
              </Label>
              <Col sm={6}>
                <Input id="toDate" name="toDate" type="date" value={printToDate} onChange={handleToDate}/>
              </Col>
            </FormGroup>
          </Form>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={print}>
            Print
          </Button>{" "}
          <Button color="secondary" onClick={togglePrint}>
            Cancel
          </Button>
        </ModalFooter>
      </Modal>
      <Modal size="xl" isOpen={modal} toggle={toggle}>
        <ModalHeader toggle={toggle}>
          <Row>
            <Col xs="10">
              Work Order - {newWoId}
            </Col>
            <Col xs="2">
            <Button
                color="primary"
                onClick={printWo}
              >
                Print
              </Button>
            </Col>
          </Row>

        </ModalHeader>
        <ModalBody>
          <Form>
            <FormGroup row>
              <Label for="consumer" sm={2}>
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
              <Label for="date" sm={2}>
                Work Date:
              </Label>
              <Col sm={4}>
                <Input id="date" name="date" type="date" value={workDate} onChange={handleWorkDate}/>
              </Col>
            </FormGroup>
            <FormGroup row>
              <Label for="preparedBy" sm={2}>
                Prepared By:
              </Label>
              <Col sm={4}>
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
                      selected={preparedBy ? preparedBy == user.name : globalUser == user.name}
                    >
                      {user.name}
                    </option>
                  ))}
                </Input>
              </Col>
            </FormGroup>
            <FormGroup row>
              <Label for="description" sm={2}>
                Description:
              </Label>
              <Col sm={10}>
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
          </Form>
          <hr/>
          <h3>Materials</h3>
          <Row>
            <Form>
              <FormGroup row>
                <Label for="consumer" sm={1}>
                  Item:
                </Label>
                <Col sm={4}>
                  <Input
                    type="select"
                    name="addItemSelect"
                    id="addItemSelect"
                    onChange={handleSelectedItemChanged}
                    value={selectedItemDDValue}
                  >
                    <option disabled selected value={-1}> -- select an material -- </option>
                    {itemData.map((item) => (
                      <option
                        value={`${item.description} (${item.units})`}
                        key={`${item.description} (${item.units})`}
                      >{`${item.description} (${item.units})`}</option>
                    ))}
                  </Input>
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
                <Label sm={2}>{selectedItem && selectedItem != -1 ? selectedItem.units + " / " + selectedItem.remaining : ""}</Label>
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
            rowData={newMatrialData}
            colDefs={materialColDefs}
            onSelectionChanged={selectedMaterialChanged}
          />
          <br />
          <Row>
            <Col sm={2} style={{fontWeight:"bold"}}>
            Materials Total:
            </Col>
            <Col sm={9}>
             {materialCostTotal ? formatter.format(materialCostTotal) : ""}
            </Col>
              <Col sm={1}><Button color="danger" onClick={deleteMaterial}>
                Delete
              </Button>
            </Col>
          </Row>
          <hr/>
          <h3>Labor</h3>
          <Row>
            <Form>
              <FormGroup row>
                <Label for="resource" sm={1}>
                  Resource:
                </Label>
                <Col sm={3}>
                  <Input type="text" name="resource" id="resource" value={laborResource} onChange={handleLaborResource}></Input>
                </Col>
                <Label for="cost" sm={1}>
                  Cost:
                </Label>
                <Col sm={2}>
                  <Input id="cost" name="cost" type="number" value={laborCost} onChange={handleLaborCost} />
                </Col>
                <Label for="hours" sm={1}>
                  Hours:
                </Label>
                <Col sm={2}>
                  <Input id="hours" name="hours" type="number" value={laborHours} onChange={handleLaborHours} />
                </Col>
                <Col sm={1}>
                  <Button color="primary" onClick={addLabor}>
                    Add
                  </Button>{" "}
                </Col>
              </FormGroup>
            </Form>
          </Row>
          <Grid
            height="250px"
            rowData={laborData}
            colDefs={laborColDefs}
            onSelectionChanged={selectedLaborChanged}
          />
          <br/>
          <Row>
            <Col sm={2} style={{fontWeight:"bold"}}>
            Labor Total:
            </Col>
            <Col sm={9}>
             {laborCostTotal ? formatter.format(laborCostTotal) : ""}
            </Col>
              <Col sm={1}><Button color="danger" onClick={deleteLabor}>
                Delete
              </Button>
            </Col>
          </Row>
          <hr/>
          <Row>
            <Col sm={9} style={{fontWeight:"bold"}}>
            Materials Total:
            </Col>
            <Col sm={2} style={{textAlign:"right"}}>
             {materialCostTotal ? formatter.format(materialCostTotal) : ""}
            </Col>
          </Row>
          <Row>
            <Col sm={9} style={{fontWeight:"bold"}}>
            Labor Total:
            </Col>
            <Col sm={2} style={{textAlign:"right"}}>
             {laborCostTotal ? formatter.format(laborCostTotal) : ""}
            </Col>
          </Row>
          <Row>
            <Col sm={9} style={{fontWeight:"bold"}}>
            Total:
            </Col>
            <Col sm={2} style={{fontWeight:"bold", textAlign:"right"}}>
             {formatter.format((materialCostTotal ? materialCostTotal : 0) + (laborCostTotal ? laborCostTotal : 0))}
            </Col>
          </Row>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={saveWorkOrder}>
            Save
          </Button>{" "}
          <Button color="secondary" onClick={toggle}>
            Cancel
          </Button>{" "}
          <Button color="danger" onClick={deleteWorkOrder}>
            Delete
          </Button>
        </ModalFooter>
      </Modal>
      <Row>
        <Col xs="9">
          <h2>Work Orders</h2>
        </Col>
        <Col xs="3" style={{ textAlign: "right" }}>
              <Button color="primary" onClick={createWorkOrder}>
                New Work Order
              </Button> {" "}
              <Button
                color="secondary"
                onClick={togglePrint}
              >
                Print
              </Button>
        </Col>
      </Row>
      <div
        className={"ag-theme-quartz-dark"}
        style={{ width: "100%", height: "500px" }}
      >
        {rowData?.length > 0 ? (
          <Grid
            height="750px"
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
