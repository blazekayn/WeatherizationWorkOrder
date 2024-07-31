import React, { Component, useState, useEffect } from 'react';
import '../print.css';

export function PrintItems() {
  const [rowData, setRowData] = useState([]);
 
  useEffect(() => {
    fetch(`inventory?showOOS=false`)
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
    <div className='print-div'>
      <table>
        <tr className='header'>
          <td>
            Description
          </td>
          <td>
            Starting Amount
          </td>
          <td>
            Current Amount
          </td>
          <td>
            Units
          </td>
          <td>
            Purchase Date
          </td>
          <td>
            Cost per Unit
          </td>
          <td>
            Remaining $ Value
          </td>
        </tr>
        {rowData.map((item, index) => (
          <tr className={index%2==1 ? 'even-row' : ''}>
            <td>
              {item.description}
            </td>
            <td className='align-right'>
              {item.startingAmount}
            </td>
            <td className='align-right'>
              {item.remaining}
            </td>
            <td>
              {item.units}
            </td>
            <td>
              {dateFormatter(item.purchaseDate)}
            </td>
            <td className='align-right'>
              {formatter.format(item.cost)}
            </td>
            <td className='align-right'>
              {formatter.format(item.cost * item.remaining)}
            </td>
          </tr>
        ))}
        <tr>
          <td>
            Totals:
          </td>
          <td colSpan={5}>

          </td>
          <td className='align-right'>
            {formatter.format(rowData.reduce((n, {cost, remaining}) => n + (cost * remaining), 0))}
          </td>
        </tr>
      </table>
    </div>
  );

}
