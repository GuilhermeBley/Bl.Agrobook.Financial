import React, { useEffect, useState } from 'react';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { UserStorageService } from '../../services/UserStorageService'
import { getAllDeliveryDatesAsync } from './action';

const OrderConfirmationModal = ({
  products = [],
  show,
  onClose,
  onConfirm
}) => {
  const [modalInfo, setModalInfo] = useState(({
    isInitializing: true,
    availableDeliveryDates: []
  }));
  const storeService = new UserStorageService();
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
    deliveryAt: Yup.date()
      .required('Date is required')
      .test(
        'Data disponível para coleta',
        'Data não disponível para coleta, selecione outra data.',
        (value) => {
          if (!value) return true;
          const dateStr = value.toISOString().split('T')[0];
          return !modalInfo.availableDeliveryDates.includes(dateStr);
        }
      )
  });
  const populateDeliveryDates = async () => {
    try {
      var r = await getAllDeliveryDatesAsync();

      if (!Array.isArray(r.Result)) {
        return;
      }

      setModalInfo(r => ({
        ...r,
        availableDeliveryDates: [...r.Result]
      }))
    } catch (e) {
      console.error(e)
    }
  }

  useEffect(() => {

    try {
      populateDeliveryDates();
    }
    finally {
      setModalInfo(r => ({
        ...r,
        isInitializing: false
      }))
    }

  }, [])


  const getDeliveryRange = () => {
    if (!modalInfo || modalInfo.availableDeliveryDates.length === 0) {
      return {
        minDate: undefined,
        maxDate: undefined
      }
    }

    return {
      minDate: new Date(Math.min(modalInfo.availableDeliveryDates)),
      maxDate: new Date(Math.max(modalInfo.availableDeliveryDates))
    }
  }


  var userInfo = storeService.getUserInfo();
  const formik = useFormik({
    initialValues: {
      name: userInfo.name,
      email: userInfo.email,
      phone: userInfo.phone,
      orderItems: [...products]
    },
    validationSchema,
    onSubmit: (values) => {

      console.log('requesting to create order: ')
      console.log(values)

      storeService.setUserInfo(({
        email: values.email,
        name: values.name,
        phone: values.phone
      }));

      onConfirm(({
        customerPhone: values.phone,
        customerEmail: values.email,
        customerName: values.name,
        deliveryAt: values.deliveryAt,
        products: values.orderItems.map(x => ({
          code: x.code,
          name: x.name,
          qtt: x.qtt,
        })),
        obs: undefined,
      }));
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

            <div className="mt-1">
              <div className="row justify-content-center">
                <div className="card-body">
                  <form onSubmit={formik.handleSubmit}>
                    <div id='user-info' className='overflow-auto' style={({ maxHeight: "60vh" })}>

                      {modalInfo.availableDeliveryDates.length == 0
                        ? <>
                          <div className="mb-1 w-100">
                            <span className='text-danger text-center'>Não há datas de coleta do produto disponíveis, aguarde ou contate o vendedor.</span>
                          </div>
                        </>
                        : <>

                        </>}

                      <div className="mb-1">
                        <label htmlFor="name" className="form-label">Nome completo</label>
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

                      <div className="mb-1">
                        <label htmlFor="email" className="form-label">E-mail</label>
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

                      <div className='row'>
                        <div className='col-md-8'>
                          <div className="mb-1">
                            <label htmlFor="phone" className="form-label">Telefone</label>
                            <input
                              id="phone"
                              name="phone"
                              type="tel"
                              className={`form-control ${formik.touched.phone && formik.errors.phone ? 'is-invalid' : ''
                                }`}
                              onChange={formik.handleChange}
                              onBlur={formik.handleBlur}
                              value={formik.values.phone}
                              placeholder="ex. (15)9999-9999"
                            />
                            {formik.touched.phone && formik.errors.phone ? (
                              <div className="invalid-feedback">{formik.errors.phone}</div>
                            ) : null}
                          </div>
                        </div>
                        <div className='col-md-4'>
                          <div className="mb-1">
                            <label className="form-label">Data de coleta</label>
                            {modalInfo.availableDeliveryDates.length === 0
                              ? <>
                                <input
                                  id="inputDeliveryAt"
                                  name="inputDeliveryAt"
                                  type="text"
                                  disabled
                                  className={`form-control`}
                                  title='Nenhuma data disponível para coleta das mercadorias, contate o comercial.'
                                  placeholder='Nenhuma data disponível'
                                />
                              </>
                              : <>
                                <input
                                  id="inputDeliveryAt"
                                  name="inputDeliveryAt"
                                  type="date"
                                  min={getDeliveryRange().minDate?.toISOString().split('T')[0] ?? ''}
                                  max={getDeliveryRange().maxDate?.toISOString().split('T')[0] ?? ''}
                                  className={`form-control ${formik.touched.deliveryAt && formik.errors.deliveryAt ? 'is-invalid' : ''}`}
                                  onChange={formik.handleChange}
                                  onBlur={formik.handleBlur}
                                  value={formik.values.deliveryAt}
                                />
                              </>}

                            {formik.touched.phone && formik.errors.phone ? (
                              <div className="invalid-feedback">{formik.errors.phone}</div>
                            ) : null}
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
                            {formik.values.orderItems.map(product => (
                              <tr key={'orderItems-' + product.code}>
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
                        title={modalInfo.availableDeliveryDates.length == 0 ? "Nenhuma data de coleta disponível." : "Realizar pedido."}
                        disabled={(formik.isSubmitting || modalInfo.availableDeliveryDates.length == 0)}
                      >
                        {formik.isSubmitting ? 'Finalizando...' : 'Finalizar Pedido'}
                      </button>
                    </div>
                  </form>
                </div>
              </div>
            </div>


          </div>
        </div>
      </div>
    </div>
  );
};

export default OrderConfirmationModal;