import { useEffect, useState } from "react";
import PageNavigationBar from "../../components/PageNavigationBar";
import ProductCardItem from "../../components/ProductCardItem"
import { PaginableList } from "../../utils/PaginableList"
import { getProducts, Status } from "./action";

function Home() {
    const [pageData, setPageData] = useState({
        allItems: [],
        items: new PaginableList([], 9),
        alertMessage: { success: true, message: '', timeout: undefined }
    })

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

    return (
        <>
            <PageNavigationBar />

            <div className="container">
                <div className="row">
                    <div className="col-lg-3 mb-4">
                        <div className="card category-filter">
                            <div className="card-body">
                                <h5 className="card-title">Meus pedidos</h5>

                                <h6 className="mt-4">Categories</h6>
                                <div className="list-group list-group-flush">
                                    <a href="#" className="list-group-item list-group-item-action active">All Products</a>
                                    <a href="#" className="list-group-item list-group-item-action">Electronics</a>
                                    <a href="#" className="list-group-item list-group-item-action">Clothing</a>
                                    <a href="#" className="list-group-item list-group-item-action">Home & Garden</a>
                                    <a href="#" className="list-group-item list-group-item-action">Sports</a>
                                </div>

                                <h6 className="mt-4">Price Range</h6>
                                <div className="range-slider mt-2">
                                    <input type="range" className="form-range" min="0" max="1000" step="10" id="priceRange" />
                                    <div className="d-flex justify-content-between">
                                        <span>$0</span>
                                        <span>$1000</span>
                                    </div>
                                </div>

                                <h6 className="mt-4">Brand</h6>
                                <div className="form-check">
                                    <input className="form-check-input" type="checkbox" id="brand1" />
                                    <label className="form-check-label" for="brand1">Brand A</label>
                                </div>
                                <div className="form-check">
                                    <input className="form-check-input" type="checkbox" id="brand2" />
                                    <label className="form-check-label" for="brand2">Brand B</label>
                                </div>
                                <div className="form-check">
                                    <input className="form-check-input" type="checkbox" id="brand3" />
                                    <label className="form-check-label" for="brand3">Brand C</label>
                                </div>

                                <button className="btn btn-primary w-100 mt-3">Apply Filters</button>
                                <button className="btn btn-outline-secondary w-100 mt-2">Reset</button>
                            </div>
                        </div>
                    </div>

                    <div className="col-lg-9">
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
                                return <div className="col-sm-4 mb-3" key={index}>
                                    <ProductCardItem
                                        description={i.description}
                                        quantity={undefined}
                                        title={i.name} />
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
                    </div>
                </div>
            </div>
        </>
    );
}

export default Home;