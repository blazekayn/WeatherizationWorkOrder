import React, { useState, useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import '../print.css';

export function PrintWos() {
  const [rowData, setRowData] = useState([]);
  const [searchParams, setSearchParams] = useSearchParams();

  useEffect(() => {
    fetch(`workOrder/workOrderByDate?from=${searchParams.get("from")}&to=${searchParams.get("to")}`)
      .then((result) => result.json()) 
      .then((data) => { setRowData(data)});
  }, []);

  const formatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
  });

  const dateFormatter = (params) => {
    return new Date(params).toLocaleDateString("en-us", {
      month: '2-digit',day: '2-digit',year: 'numeric'});
  };

  return (
    (rowData?.length > 0 ? 
    <div className='print-div'>
      <table className='print-table'>
        <tr className='header'>
          <td>
            Id
          </td>
          <td>
            Consumer
          </td>
          <td>
            Prepared By
          </td>
          <td>
            Work Date
          </td>
          <td>
            Prepared Date
          </td>
          <td>
            Material Cost
          </td>
          <td>
            Labor Cost
          </td>
          <td>
            Total Cost
          </td>
        </tr>
        {rowData.map((item, index) => (
          <tr className={index%2==1 ? 'even-row' : ''}>
            <td>
              {item.id}
            </td>
            <td>
              {item.consumer}
            </td>
            <td>
              {item.preparedBy}
            </td>
            <td>
              {dateFormatter(item.workDate)}
            </td>
            <td>
              {dateFormatter(item.preparedDate)}
            </td>
            <td className='align-right'>
              {formatter.format(item.materialCost)}
            </td>
            <td className='align-right'>
              {formatter.format(item.laborCost)}
            </td>
            <td className='align-right'>
              {formatter.format(item.totalCost)}
            </td>
          </tr>
        ))}
        <tr>
          <td>
            Totals:
          </td>
          <td colSpan={6}>

          </td>
          <td className='align-right'>
            {formatter.format(rowData.reduce((n, {totalCost}) => n + totalCost, 0))}
          </td>
        </tr>
      </table>
    </div>
    : <></>)
  );
}
