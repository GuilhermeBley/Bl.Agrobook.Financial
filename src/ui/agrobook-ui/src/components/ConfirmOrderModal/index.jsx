import React, { useEffect, useState } from 'react';
import { useFormik } from 'formik';
import * as Yup from 'yup';

const OrderConfirmationModal = ({
  products = [],
  show,
  onClose,
  onConfirm
}) => {

  const validationSchema = Yup.object().shape({
    name: Yup.string()
      .min(2, 'Nome deve conter mais de dois caracteres')
      .max(500, 'Nome muito longo')
      .required('Nome é obrigatório'),
    email: Yup.string()
      .email('E-mail inválido')
      .required('E-mail é obrigatório'),
    phone: Yup.string()
      .matches(
        /^(\+?\d{0,4})?\s?-?\s?(\(?\d{3}\)?)\s?-?\s?(\(?\d{3}\)?)\s?-?\s?(\(?\d{4}\)?)$/,
        'Número de telefone inválido'
      )
      .required('Telefone é obrigatório'),
  });

  const formik = useFormik({
    initialValues: {
      name: '',
      email: '',
      phone: '',
      orderItems: []
    },
    validationSchema,
    onSubmit: (values) => {
      // Handle form submission
      alert(JSON.stringify(values, null, 2));
      onConfirm(values.orderItems);
    },
  });

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

            <div className="mt-5">
              <div className="row justify-content-center">
                <div className="card-header bg-primary text-white">
                  <h6 className="mb-0">Meus dados</h6>
                </div>
                <div className="card-body">
                  <form onSubmit={formik.handleSubmit}>
                    <div className="mb-3">
                      <label htmlFor="name" className="form-label">Full Name</label>
                      <input
                        id="name"
                        name="name"
                        type="text"
                        className={`form-control ${formik.touched.name && formik.errors.name ? 'is-invalid' : ''
                          }`}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        value={formik.values.name}
                      />
                      {formik.touched.name && formik.errors.name ? (
                        <div className="invalid-feedback">{formik.errors.name}</div>
                      ) : null}
                    </div>

                    <div className="mb-3">
                      <label htmlFor="email" className="form-label">Email Address</label>
                      <input
                        id="email"
                        name="email"
                        type="email"
                        className={`form-control ${formik.touched.email && formik.errors.email ? 'is-invalid' : ''
                          }`}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        value={formik.values.email}
                      />
                      {formik.touched.email && formik.errors.email ? (
                        <div className="invalid-feedback">{formik.errors.email}</div>
                      ) : null}
                    </div>

                    <div className="mb-3">
                      <label htmlFor="phone" className="form-label">Phone Number</label>
                      <input
                        id="phone"
                        name="phone"
                        type="tel"
                        className={`form-control ${formik.touched.phone && formik.errors.phone ? 'is-invalid' : ''
                          }`}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        value={formik.values.phone}
                        placeholder="e.g. 123-456-7890"
                      />
                      {formik.touched.phone && formik.errors.phone ? (
                        <div className="invalid-feedback">{formik.errors.phone}</div>
                      ) : null}
                    </div>
                  </form>
                </div>
              </div>
            </div>

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
              type="submit"
              className="btn btn-primary"
              disabled={formik.isSubmitting}
            >
              {formik.isSubmitting ? 'Finalizando...' : 'Finalizar Pedido'}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default OrderConfirmationModal;