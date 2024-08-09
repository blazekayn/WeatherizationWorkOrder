import React, { Component, useState, useEffect } from 'react';
import { Grid } from './Table';
import { Input, Label, Button, Row, Col, Modal, Form,
  ModalHeader,
  ModalBody,
  ModalFooter,
  FormGroup, } from 'reactstrap';

export function Items() {
  const [rowData, setRowData] = useState([]);
  const [showOutOfStock, setShowOutOfStock] = useState(false);
  const [modal, setModal] = useState(false);
  const [costPer, setCostPer] = useState('');
  const [description, setDescription] = useState('');
  const [startingAmount, setStartingAmount] = useState('');
  const [itemUnits, setItemUnits] = useState('');
  const [units, setUnits] = useState([]);
  const [purchaseDate, setPurchaseDate] = useState([]);
  const [reload, setReload] = useState(false);
  const [selectedRow, setSelectedRow] = useState();
  const [editing, setEditing] = useState(false);

  useEffect(() => {
    fetch(`inventory?showOOS=${showOutOfStock}`)
      .then((result) => result.json())
      .then((data) => { setRowData(data)});
  }, [showOutOfStock, reload]);

  useEffect(() => {
    fetch(`inventory/units`)
      .then((result) => result.json())
      .then((data) => { setUnits(data)});
  }, [reload]);

  const toggle = () => {
    setModal(!modal);
    setCostPer("");
    setDescription("");
    setStartingAmount("");
    setItemUnits("");
    setPurchaseDate("");
    setReload(!reload);
  }

  /* Format Date Cells */
  const dateFormatter = (params) => {
    return new Date(params.value).toLocaleDateString("en-us", {
      month: '2-digit',day: '2-digit',year: 'numeric'});
  };

  const colDefs = [
    {
      field: "id",
      width: 90,
    },
    {
      field: "description",
      width: 360,
    },
    {
      headerName: "Cost Each",
      field: "cost",
      width: 125,
      valueFormatter: (params) => {
        const formatter = new Intl.NumberFormat('en-US', {
          style: 'currency',
          currency: 'USD',
        });
        
        return formatter.format(params.value)
      },
    },
    {
      field: "units",
      width: 175,
    },
    {
      headerName: "Starting Each",
      field: "startingAmount",
      width: 175,
    },
    {
      headerName: "Remaining Each",
      field: "remaining",
      width: 175,
    },
    {
      field: "purchaseDate",
      width: 175,
      valueFormatter: dateFormatter,
    },
  ];

  // const updateCost = (e) => {
  //   setCost(e.target.value);
  //   //calcualteCosts();
  // }
  const updateCostPer = (e) => {
    setCostPer(e.target.value);
    //calcualteCosts();
  }

  const handleDescription = (e) => {
    setDescription(e.target.value);
  }

  const createItem = () => {
    if(editing){
      updateItem();
    }else{
      fetch(`inventory`, {
        method: 'PUT',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          description: description,
          cost: costPer,
          units: itemUnits,
          startingAmount: startingAmount,
          remaining: startingAmount,
          purchaseDate: purchaseDate,
          createdBy: localStorage.getItem("gUserName"),
          lastModifiedBy: localStorage.getItem("gUserName")
        })
      })
      .then(response => {
        if(!response.ok){
          if(response.status == 400){
            console.log("validation error");
          }
        } else{   
          toggle()
        }
      })
    }
  }

  const updateItem = () => {
    fetch(`inventory`, {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        id: selectedRow.id,
        description: description,
        cost: costPer,
        units: itemUnits,
        startingAmount: startingAmount,
        remaining: startingAmount,
        purchaseDate: purchaseDate,
        createdBy: selectedRow.createdBy,
        lastModifiedBy: localStorage.getItem("gUserName")
      })
    })
    .then(response => {
      if(!response.ok){
        if(response.status == 400){
          console.log("validation error");
        }
      } else{
        setEditing(false);
        toggle()
      }
    })
  }

  // const updateCount = (e) => {
  //   setCount(e.target.value);
  //   //calcualteCosts();
  // }

  // const calcualteCosts = () => {
  //   console.log("Calculating costs", costPer, cost, count);
  //   if(costPer && count){
  //     console.log("set cost" + costPer * count);
  //     setCost(costPer * count);
  //   }else if(cost && count){
  //     console.log("set cost per" + cost/count);
  //     setCostPer(cost/count);
  //   }
  // }

  const handleUnits = (e) => {
    setItemUnits(e.target.value);
  }

  const handleStartingAmount = (e) => {
    setStartingAmount(e.target.value);
  }

  const handlePurchaseDate = (e) => {
    setPurchaseDate(e.target.value);
  }

  const changeUnit = (e) => {
    let { value } = e.target;
    setItemUnits(value);
  }

  const print = () => {
    window.location = '/#/printitems';
  }

  const onSelectionChanged = (event) => {
    if(event.api.getSelectedNodes().length > 0){
      const data = event.api.getSelectedNodes()[0].data;
      setSelectedRow(data);
      console.log(data);
      setCostPer(data.cost);
      setDescription(data.description);
      setStartingAmount(data.startingAmount);
      setItemUnits(data.units);
      setPurchaseDate(data.purchaseDate);
      setModal(true);
      setEditing(true);
    }else{
      setEditing(false);
      setSelectedRow(null);
      setCostPer("");
      setDescription("");
      setStartingAmount("");
      setItemUnits("");
      setPurchaseDate("");
    }
  }

  return (
    <>
    <Modal
    size="lg"
      isOpen={modal}
      toggle={toggle}
    >
      <ModalHeader toggle={toggle}>{editing ? "Edit Item" : "Add Item"}</ModalHeader>
      <ModalBody>
        <Form>
          <FormGroup row>
            <Label for="description" sm={3}>
              Description:
            </Label>
            <Col sm={9}>
              <Input id="description" name="description" placeholder='Item Description' type="textarea" value={description} onChange={handleDescription} maxLength={255}
                    rows="2"/>
            </Col>
          </FormGroup>
          <FormGroup row>
            <Label for="cost" sm={3}>
              Cost per unit ($):
            </Label>
            <Col sm={3}>
              <Input id="cost" name="cost" type="number" onChange={updateCostPer} value={costPer}/>
            </Col>
          </FormGroup>
          <FormGroup row>
            <Label for="unitLabel" sm={3}>
              Unit Label:
            </Label>
            <Col sm={4}>
              <Input id="unitLabel" name="unitLabel" type="text" value={itemUnits} onChange={handleUnits} maxLength={50}/>
            </Col>
            <Col sm={4}>
              <Input
                  type="select"
                  name="currentUser"
                  id="currentUser"
                  onChange={changeUnit}
                >
                  {units.map((unit, i) =>
                      <option value={unit} key={i}>{unit}</option>
                    )
                  }
                </Input>
            </Col>
          </FormGroup>
          <FormGroup row>
            <Label for="amount" sm={3}>
              Starting Amount:
            </Label>
            <Col sm={3}>
              <Input id="amount" name="amount" type="number" value={startingAmount} onChange={handleStartingAmount}/>
            </Col>
          </FormGroup>
          <FormGroup row>
            <Label for="date" sm={3}>
              Purchase Date:
            </Label>
            <Col sm={5}>
              <Input id="date" name="date" type="date" value={purchaseDate} onChange={handlePurchaseDate}/>
            </Col>
          </FormGroup>
        </Form>
      </ModalBody>
      <ModalFooter>
        <Button color="primary" onClick={createItem}>
          Save
        </Button>{' '}
        <Button color="secondary" onClick={toggle}>
          Cancel
        </Button>
      </ModalFooter>
    </Modal>
    <Row>
      <Col xs="9">
        <h2>Inventory
        </h2>
      </Col>
      <Col xs="3" style={{textAlign:"right"}}>
        <Button
          color="primary"
          onClick={toggle}
        >
          Add Item
        </Button>
        <Button
          color="secondary"
          onClick={print}
        >
          Print
        </Button>
        </Col>
      </Row>
      <Input
          id="chkShowAll"
          type="checkbox"
          checked={showOutOfStock}
          onChange={
            (e) => {
              setShowOutOfStock(e.target.checked)
            }
          }
        />
        {' '}
        <Label check>
          Show out of Stock
        </Label>
      <div
        className={
          "ag-theme-quartz-dark"
        }
        style={{ width: "100%", height: "500px" }}
      >
        {rowData?.length > 0 ?
        <Grid
          rowData={rowData}
          colDefs={colDefs}
          onSelectionChanged={onSelectionChanged}
        />
        : <></>}
      </div>
    </>
  );

}
