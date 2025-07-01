import React, { useMemo } from 'react';
import { useTable, useSortBy } from 'react-table';

const PreOrderTable = ({ data }) => {
  const columns = useMemo(
    () => [
      {
        Header: 'Order Code',
        accessor: 'OrderCode',
      },
      {
        Header: 'Customer',
        accessor: 'CustomerName',
      },
      {
        Header: 'Phone',
        accessor: 'CustomerPhone',
      },
      {
        Header: 'Email',
        accessor: 'CustomerEmail',
      },
      {
        Header: 'Delivery Date',
        accessor: 'DeliveryAt',
        Cell: ({ value }) => new Date(value).toLocaleDateString(),
      },
      {
        Header: 'Total Items',
        accessor: 'Product.Quantity',
        Cell: ({ value }) => value.reduce((sum, product) => sum + product.Quantity, 0),
      },
      {
        Header: 'Total Value',
        accessor: 'Product.Price',
        Cell: ({ value }) => {
          const total = value.reduce(
            (sum, product) => sum + product.Quantity * (product.Price || 0),
            0
          );
          return `$${total.toFixed(2)}`;
        },
      },
      {
        Header: 'Created At',
        accessor: 'InsertedAt',
        Cell: ({ value }) => new Date(value).toLocaleString(),
      },
      {
        Header: 'Notes',
        accessor: 'Obs',
      },
    ],
    []
  );

  const {
    getTableProps,
    getTableBodyProps,
    headerGroups,
    rows,
    prepareRow,
  } = useTable(
    {
      columns,
      data,
    },
    useSortBy
  );

  return (
    <div className="table-container">

    {Array.isArray(data) && data.length > 0 
    ? <>
    <table {...getTableProps()} className="order-table table">
        <thead>
          {headerGroups.map(headerGroup => (
            <tr {...headerGroup.getHeaderGroupProps()}>
              {headerGroup.headers.map(column => (
                <th {...column.getHeaderProps(column.getSortByToggleProps())}>
                  {column.render('Header')}
                  <span>
                    {column.isSorted
                      ? column.isSortedDesc
                        ? ' 🔽'
                        : ' 🔼'
                      : ''}
                  </span>
                </th>
              ))}
            </tr>
          ))}
        </thead>
        <tbody {...getTableBodyProps()}>
          {rows.map(row => {
            prepareRow(row);
            return (
              <tr {...row.getRowProps()}>
                {row.cells.map(cell => {
                  return (
                    <td {...cell.getCellProps()}>{cell.render('Cell')}</td>
                  );
                })}
              </tr>
            );
          })}
        </tbody>
      </table>
    </>
    : <>
    </>}
    </div>
  );
};

export default PreOrderTable;