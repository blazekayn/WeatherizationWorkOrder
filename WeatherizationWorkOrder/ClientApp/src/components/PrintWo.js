import React, { useState, useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import '../print.css';

export function PrintWo() {
  const [rowData, setRowData] = useState(null);
  const [totalMaterials, setTotalMaterials] = useState(0);
  const [totalLabor, setTotalLabor] = useState(0);
  const [total, setTotal] = useState(0);
  const [searchParams, setSearchParams] = useSearchParams();

  useEffect(() => {
    fetch(`workOrder/${searchParams.get("id")}`)
      .then((result) => result.json()) 
      .then((data) => { 
        if(data != null){
          setRowData(data);
          const mT = data?.materials.reduce((n, {total}) => n + total, 0);
          const lT = data?.labors.reduce((n, {total}) => n + total, 0);
          const t = mT + lT;
          setTotalMaterials(mT);
          setTotalLabor(lT);
          setTotal(t);
        }
      });
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
    (rowData ? 
    <div className='print-div'>
      <h2>Work Order - {rowData.id}</h2>
      <table className='print-table'>
        <tr>
          <td style={{width:"150px", fontWeight:"bold"}}>
            Consumer
          </td>
          <td>
            {rowData.consumer}
          </td>
        </tr>
        <tr>
          <td style={{width:"150px", fontWeight:"bold"}}>
            Work Date
          </td>
          <td>
          {dateFormatter(rowData.workDate)}
          </td>
        </tr>
        <tr>
          <td style={{width:"150px", fontWeight:"bold"}}>
          Prepared By
          </td>
          <td>
          {rowData.preparedBy}
          </td>
        </tr>
        <tr>
          <td style={{width:"150px", fontWeight:"bold"}}>
          Prepared Date
          </td>
          <td>
          {dateFormatter(rowData.preparedDate)}
          </td>
        </tr>
        <tr>
          <td style={{width:"150px", fontWeight:"bold"}}>
          Description
          </td>
          <td>
          {rowData.description}
          </td>
        </tr>
      </table>
      <br/>
      <div>
        <h3>Materials</h3>
        <table className='print-table'>
          <tr className='header'>
            <td>
              Material
            </td>
            <td style={{width:"150px"}}>
              Units
            </td>
            <td style={{width:"75px"}}>
              Cost Per
            </td>
            <td style={{width:"75px"}}>
              Amount Used
            </td>
            <td style={{width:"100px"}}>
              Total
            </td>
          </tr>
          {rowData?.materials.map((item, index) => (
            <tr className={index%2==1 ? 'even-row' : ''}>
              <td>
                {item.description}
              </td>
              <td>
                {item.units}
              </td>
              <td className='align-right'>
                {formatter.format(item.costPer)}
              </td>
              <td className='align-right'>
                {item.amountUsed.toFixed(2)}
              </td>
              <td className='align-right'>
                {formatter.format(item.total)}
              </td>
            </tr>
          ))}
          <tr className='header'>
              <td>
                Total
              </td>
              <td>
                
              </td>
              <td className='align-right'>
                
              </td>
              <td className='align-right'>
                
              </td>
              <td className='align-right'>
                {formatter.format(totalMaterials)}
              </td>
            </tr>
        </table>
      </div>
      <br/>
      <div>
        <h3>Labor</h3>
        <table className='print-table'>
          <tr className='header'>
            <td>
              Resource
            </td>
            <td style={{width:"75px"}}>
              Cost
            </td>
            <td style={{width:"75px"}}>
              Hours
            </td>
            <td style={{width:"100px"}}>
              Total
            </td>
          </tr>
          {rowData?.labors.map((item, index) => (
            <tr className={index%2==1 ? 'even-row' : ''}>
              <td>
                {item.resource}
              </td>
              <td className='align-right'>
                {formatter.format(item.cost)}
              </td>
              <td className='align-right'>
                {item.hours.toFixed(2)}
              </td>
              <td className='align-right'>
                {formatter.format(item.total)}
              </td>
            </tr>
          ))}
          <tr className='header'>
              <td>
                Total
              </td>
              <td>
                
              </td>
              <td className='align-right'>
                
              </td>
              <td className='align-right'>
                {formatter.format(totalLabor)}
              </td>
            </tr>
        </table>
      </div>
      <br/>
      <div>
        <h3>Totals</h3>
        <table className='print-table' style={{width:"300px"}}>
          <tr>
            <td>
              Materials
            </td>
            <td className='align-right'>
              {formatter.format(totalMaterials)}
            </td>
          </tr>
          <tr>
            <td>
              Labor
            </td>
            <td className='align-right'>
              {formatter.format(totalLabor)}
            </td>
          </tr>
          <tr className='header'>
            <td>
              Total
            </td>
            <td className='align-right'>
              {formatter.format(total)}
            </td>
          </tr>
        </table>
      </div>
      <br/>
      <div>
        <h3>Signatures</h3>
        <table className='print-table'>
          <tr className='header'>
            <td style={{width:"150px"}}>
              Role
            </td>
            <td>
              Signature
            </td>
          </tr>
          <tr>
            <td style={{width:"150px"}}>
              Preparer:
            </td>
            <td>

            </td>
          </tr>
          <tr>
            <td style={{width:"150px"}}>
              Reviewer:
            </td>
            <td>
              
            </td>
          </tr>
          <tr>
            <td style={{width:"150px"}}>
              Approver:
            </td>
            <td>
              
            </td>
          </tr>
        </table>
      </div>
    </div>
    : <></>)
  );
}
