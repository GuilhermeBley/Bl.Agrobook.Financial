import React, { useState } from 'react';

const OrderConfirmationModal = ({ 
  products = [], 
  show, 
  onClose, 
  onConfirm 
}) => {
  const [quantities, setQuantities] = useState(
    products.reduce((acc, product) => {
      acc[product.id] = product.qtt || 1;
      return acc;
    }, {})
  );

  const handleQuantityChange = (productId, newQuantity) => {
    setQuantities(prev => ({
      ...prev,
      [productId]: Math.max(1, parseInt(newQuantity) || 1)
    }));
  };

  const handleConfirm = () => {
    const orderItems = products.map(product => ({
      ...product,
      qtt: quantities[product.id] || 1
    }));
    onConfirm(orderItems);
  };

  const calculateTotal = () => {
    return products.reduce((sum, product) => {
      return sum + (product.price * (quantities[product.id] || 1));
    }, 0).toFixed(2);
  };

  if (!show) return null;

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
                    <th>Preço Unitário</th>
                    <th>Quantidade</th>
                    <th>Subtotal</th>
                  </tr>
                </thead>
                <tbody>
                  {products.map(product => (
                    <tr key={product.id}>
                      <td>{product.name}</td>
                      <td>R${product.price.toFixed(2)}</td>
                      <td>
                        <input
                          type="number"
                          className="form-control form-control-sm"
                          min="1"
                          value={quantities[product.id] || 1}
                          onChange={(e) => handleQuantityChange(product.id, e.target.value)}
                          style={{ width: '70px' }}
                        />
                      </td>
                      <td>
                        R${(product.price * (quantities[product.id] || 1)).toFixed(2)}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
            <div className="d-flex justify-content-end mt-3">
              <h5 className="fw-bold">
                Total: R${calculateTotal()}
              </h5>
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