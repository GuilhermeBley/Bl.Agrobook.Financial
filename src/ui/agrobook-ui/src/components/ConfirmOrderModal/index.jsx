import React, { useState } from 'react';

const OrderConfirmationModal = ({ 
  products = [], 
  show, 
  onClose, 
  onConfirm 
}) => {

  const handleConfirm = () => {
    onConfirm(orderItems);
  };

  if (!show) return <></>;

  return (
    <div className="modal fade show" style={{ display: 'block', backgroundColor: 'rgba(0,0,0,0.5)' }}>
      <div className="modal-dialog modal-dialog-centered modal-lg">
        <div className="modal-content">
          <div className="modal-header border-bottom-0">
            <h5 className="modal-title fw-bold">Confirmar Pedido</h5>
            <button 
              type="button" 
              className="btn-close" 
              onClick={onClose}
              aria-label="Close"
            ></button>
          </div>
          <div className="modal-body">
            <div className="table-responsive">
              <table className="table">
                <thead>
                  <tr>
                    <th>Produto</th>
                    <th>Quantidade</th>
                  </tr>
                </thead>
                <tbody>
                  {products.map(product => (
                    <tr key={product.code}>
                      <td>{product.name}</td>
                      <td>{product.qtt}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
          <div className="modal-footer border-top-0">
            <button
              type="button"
              className="btn btn-outline-secondary me-2"
              onClick={onClose}
            >
              Cancelar
            </button>
            <button
              type="button"
              className="btn btn-primary"
              onClick={handleConfirm}
            >
              Finalizar Pedido
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default OrderConfirmationModal;