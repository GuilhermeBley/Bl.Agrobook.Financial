import { useEffect, useState } from "react";
import PageNavigationBar from "../../components/PageNavigationBar";
import ProductCardItem from "../../components/ProductCardItem"
import ScrollToTopButton from "../../components/ScrollToTopButton"
import ConfirmOrderModal from "../../components/ConfirmOrderModal"
import { PaginableList } from "../../utils/PaginableList"
import { getProducts, Status } from "./action";

function Home() {
    const [pageData, setPageData] = useState({
        allItems: [],
        cartItems: new Map(),
        items: new PaginableList([], 9),
        alertMessage: { success: true, message: '', timeout: undefined }
    })
    const [shouldShowModalConfirmation, setShouldShowModalConfirmation] = useState(false);

    useEffect(
        () => {

            const populateProducts = async () => {
                let prodResult = await getProducts();

                if (prodResult.Status !== Status.Ok) {
                    console.log('Failed to get products.', prodResult.Result)
                    setPageData(p => ({
                        ...p,
                        alertMessage: { message: "Falha ao coletar dados dos produtos.", success: false, timeout: undefined }
                    }))
                    return;
                }

                console.log("updating prod list");
                setPageData(p => {
                    p.items.UpdateItems(prodResult.Result)
                    return ({
                        ...p,
                        allItems: prodResult.Result
                    })
                })
            };

            populateProducts();

        }, [])

    const removeAllCartProducts = () => {

        pageData.allItems.forEach(product => {
            if (product.resetKey)
                product.resetKey += 1;
            else
                product.resetKey = 0;
        });
        pageData.cartItems.clear();
        setPageData(p => ({
            ...p,
            cartItems: pageData.cartItems,
            allItems: pageData.allItems
        }));
    }

    const removeCartProduct = (product) => {

        let cartItems = pageData.cartItems;
        cartItems.delete(product.code)
        if (product.resetKey)
            product.resetKey += 1;
        else
            product.resetKey = 0;
        setPageData(p => ({
            ...p,
            cartItems: cartItems
        }));
    }

    const addCartProduct = (product, qtt) => {

        if (qtt < 1) {
            return removeCartProduct(product);
        }

        let cartItems = pageData.cartItems;
        cartItems.set(product.code, ({
            product,
            qtt
        }))

        setPageData(p => ({
            ...p,
            cartItems: cartItems
        }));
    }

    return (
        <>
            <PageNavigationBar />

            <div className="container">
                <div className="row">
                    <div className="col-lg-4 mb-4">
                        <div className="card">
                            <div className="card-body">
                                <h5 className="card-title">Meus pedidos</h5>

                                <div style={{ maxHeight: "300px", minHeight: "100px", overflowY: "auto" }}>
                                    <ul className="list-group">

                                        {pageData.cartItems.entries().map(([key, cartItem]) => <>
                                            <li key={key} className="list-group-item d-flex justify-content-between align-items-center" title={cartItem.product.name}>
                                                <div className="bl-text-clamp-1">
                                                    <span className="badge bg-primary rounded-pill ms-2">{cartItem.qtt}</span>
                                                    {" " + cartItem.product.name}
                                                </div>
                                                <button className="btn btn-sm btn-outline-danger" onClick={() => removeCartProduct(cartItem.product)}>Remover</button>
                                            </li>
                                        </>)}
                                    </ul>
                                </div>

                                <button className="btn btn-primary w-100 mt-3" onClick={(() => setShouldShowModalConfirmation(true))}>Finalizar pedido</button>
                                <button className="btn btn-outline-secondary w-100 mt-2" onClick={removeAllCartProducts}>Limpar</button>
                            </div>
                        </div>
                    </div>

                    <div className="col-lg-8">
                        <div className="row mb-4">
                            <div className="col-md-6">
                                <div className="input-group">
                                    <input type="text" className="form-control" placeholder="Busque os produtos..." />
                                    <button className="btn btn-primary" type="button">
                                        <i className="bi bi-search"></i>
                                    </button>
                                </div>
                            </div>
                            <div className="col-md-6 text-md-end">
                                <div className="dropdown d-inline-block me-2">
                                    <button className="btn btn-outline-secondary dropdown-toggle" type="button" id="sortDropdown" data-bs-toggle="dropdown">
                                        Ordenar por: Nome
                                    </button>
                                    <ul className="dropdown-menu">
                                        <li><a className="dropdown-item" href="#">Nome</a></li>
                                    </ul>
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            {pageData.items.CurrentShowedItems.map((i, index) => {
                                return <div className="col-sm-4 mb-3" key={`product-${index}-${i.resetKey}`}>
                                    <ProductCardItem
                                        description={i.description}
                                        quantity={undefined}
                                        title={i.name}
                                        onItemChanged={(qtt) => addCartProduct(i, qtt)} />
                                </div>
                            })}
                        </div>

                        {pageData.items.TotalPageQuantity > 1
                            ? <nav aria-label="Page navigation">
                                <ul className="pagination justify-content-center">
                                    <li className="page-item disabled">
                                        <a className="page-link" href="#" tabIndex="-1">Anterior</a>
                                    </li>
                                    <li className="page-item active"><a className="page-link" href="#">1</a></li>
                                    <li className="page-item"><a className="page-link" href="#">2</a></li>
                                    <li className="page-item"><a className="page-link" href="#">3</a></li>
                                    <li className="page-item">
                                        <a className="page-link" href="#">Próxima</a>
                                    </li>
                                </ul>
                            </nav>
                            : <></>}

                        {/**TODO: add ConfirmOrderModal */}

                        <ScrollToTopButton />
                    </div>
                </div>
            </div>

            <ConfirmOrderModal 
                products={pageData.cartItems.entries().map(([key, x]) => ({
                    code: x.product.code,
                    name: x.product.name,
                    qtt: x.qtt,
                }))}
                show={shouldShowModalConfirmation}/>
        </>
    );
}

export default Home;