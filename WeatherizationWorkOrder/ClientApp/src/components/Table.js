import React, { useState, useEffect, useMemo } from "react";
import { AgGridReact } from "ag-grid-react"; // React Grid Logic
import "ag-grid-community/styles/ag-grid.css"; // Core CSS
import "ag-grid-community/styles/ag-theme-quartz.css"; // Theme

export function Grid(props) {
  // Row Data: The data to be displayed.
  const [rowData, setRowData] = useState(props.rowData);

  // Column Definitions: Defines & controls grid columns.
  const [colDefs] = useState(props.colDefs);

  useEffect(() => {
    setRowData(props.rowData);
  }, [props])

  // Apply settings across all columns
  const defaultColDef = useMemo(() => ({
    filter: true,
    editable: true,
    resizable: true,
  }));
  
  // Container: Defines the grid's theme & dimensions.
  return (
    <div
      className={
        "ag-theme-quartz-dark"
      }
      style={{ width: "100%", height: "500px" }}
    >
      <AgGridReact
        rowData={rowData}
        columnDefs={colDefs}
        defaultColDef={defaultColDef}
        onSelectionChanged={props.onSelectionChanged}
        pagination={true}
        rowSelection="single"
        onCellValueChanged={(event) =>
          console.log(`New Cell Value: ${event.value}`)
        }
      />
    </div>
  );
};