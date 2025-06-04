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
    const [textProductsFilter, setTextProductsFilter] = useState('');

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

    const handleOrderConfirmation = (orders) => {
        // TODO: handle order confirmation
    }

    const handleChangeToPage = (pageNumber) => {
        pageNumber = parseInt(pageNumber)
        if (pageNumber < 1 || pageNumber > pageData.items.TotalPageQuantity) {
            console.warn(`Invalid page ${pageNumber}.`)
            return;
        }

        pageData.items.ChangePage(pageNumber);
        setPageData(p => ({
            ...p
        }))
    }

    const handleOpenModal = () => {
        if (pageData.cartItems.size == 0) {
            setPageData(p => ({
                ...p,
                alertMessage: { message: "Selecione produtos para o carrinho.", success: false, timeout: 5000 }
            }))
            return;
        }

        setShouldShowModalConfirmation(true)
    }

    const handleModalClose = () => {
        setShouldShowModalConfirmation(false)
    }

    const handleTextProdFilter = () => {
        let searchInput = textProductsFilter?.toLowerCase()?.trim();
        if (!searchInput || searchInput.length < 1) {
            pageData.items.UpdateItems(
                pageData.allItems
            )
            setPageData(p => ({
                ...p
            }))
            return;
        }
        pageData.items.UpdateItems(
            pageData.allItems.filter(x => ("" + x.name).toLowerCase().includes(searchInput))
        )
        setPageData(p => ({
            ...p
        }))
    }

    const getPageNumberList = () => {
        let currentPage = pageData.items.CurrentPage;
        let totalPages = pageData.items.TotalPageQuantity;
        let showStart = totalPages > 5 && currentPage - 2 > 1
        let showEnd = totalPages > 5 && currentPage + 3 <= totalPages
        if (totalPages > 5 && currentPage > 3)
            return [
                ({ number: currentPage - 1, view: `Anterior`, enabled: currentPage > 1 }),
                ({ number: 1, view: `...`, enabled: showStart }),/**start */
                ({ number: currentPage, view: `${currentPage}`, enabled: true }),
                ({ number: currentPage + 1, view: `${currentPage + 1}`, enabled: currentPage + 1 <= totalPages }),
                ({ number: currentPage + 2, view: `${currentPage + 2}`, enabled: currentPage + 2 <= totalPages }),
                ({ number: totalPages, view: `...`, enabled: showEnd }),/**end */
                ({ number: currentPage + 1, view: `Próxima`, enabled: currentPage + 1 <= totalPages }),
            ]
        return [
            ({ number: currentPage - 1, view: `Anterior`, enabled: currentPage > 1 }),
            ({ number: currentPage, view: `${currentPage}`, enabled: true }),
            ({ number: currentPage + 1, view: `${currentPage + 1}`, enabled: currentPage + 1 <= totalPages }),
            ({ number: currentPage + 2, view: `${currentPage + 2}`, enabled: currentPage + 2 <= totalPages }),
            ({ number: currentPage + 1, view: `Próxima`, enabled: currentPage + 1 <= totalPages }),
        ]
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

                                <button className="btn btn-primary w-100 mt-3" onClick={(() => handleOpenModal())}>Finalizar pedido</button>
                                <button className="btn btn-outline-secondary w-100 mt-2" onClick={removeAllCartProducts}>Limpar</button>
                            </div>
                        </div>
                    </div>

                    <div className="col-lg-8">
                        <div className="row mb-4">
                            <div className="col-md-6">
                                <div className="input-group">
                                    <input type="text" className="form-control" placeholder="Busque os produtos..." value={textProductsFilter} onChange={e => setTextProductsFilter(e.target.value)} />
                                    <button className="btn btn-primary" type="button" onClick={handleTextProdFilter}>
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
                                        imgUrl={i.imgUrl}
                                        onItemChanged={(qtt) => addCartProduct(i, qtt)} />
                                </div>
                            })}
                        </div>

                        {pageData.items.TotalPageQuantity > 1
                            ? <>
                                <nav aria-label="Page navigation">
                                    <ul className="pagination justify-content-center">
                                        {getPageNumberList().map((page, idx) => <>
                                            <li className={page.number == pageData.items.CurrentPage ? "page-item active" : "page-item"} key={idx}>
                                                <a className={page.enabled ? "page-link" : "page-link disabled"} href="#" tabIndex="-1" onClick={() => handleChangeToPage(page.number)}>
                                                    {page.view}
                                                </a>
                                            </li>
                                        </>)}
                                    </ul>
                                </nav>
                            </>
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
                show={shouldShowModalConfirmation}
                onClose={handleModalClose}
                onConfirm={handleOrderConfirmation} />
        </>
    );
}

export default Home;