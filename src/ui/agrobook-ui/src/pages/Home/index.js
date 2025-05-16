import { useState } from "react";
import PageNavigationBar from "../../components/PageNavigationBar";
import ProductCardItem from "../../components/ProductCardItem"

function Home() {
    const [pageData, setPageData] = useState({
        items: []
    })
    
    return (
        <>
            <PageNavigationBar />

            <div class="container">
                <div class="row">
                    <div class="col-lg-3 mb-4">
                        <div class="card category-filter">
                            <div class="card-body">
                                <h5 class="card-title">Filters</h5>

                                <h6 class="mt-4">Categories</h6>
                                <div class="list-group list-group-flush">
                                    <a href="#" class="list-group-item list-group-item-action active">All Products</a>
                                    <a href="#" class="list-group-item list-group-item-action">Electronics</a>
                                    <a href="#" class="list-group-item list-group-item-action">Clothing</a>
                                    <a href="#" class="list-group-item list-group-item-action">Home & Garden</a>
                                    <a href="#" class="list-group-item list-group-item-action">Sports</a>
                                </div>

                                <h6 class="mt-4">Price Range</h6>
                                <div class="range-slider mt-2">
                                    <input type="range" class="form-range" min="0" max="1000" step="10" id="priceRange" />
                                    <div class="d-flex justify-content-between">
                                        <span>$0</span>
                                        <span>$1000</span>
                                    </div>
                                </div>

                                <h6 class="mt-4">Brand</h6>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="brand1" />
                                    <label class="form-check-label" for="brand1">Brand A</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="brand2" />
                                    <label class="form-check-label" for="brand2">Brand B</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="brand3" />
                                    <label class="form-check-label" for="brand3">Brand C</label>
                                </div>

                                <button class="btn btn-primary w-100 mt-3">Apply Filters</button>
                                <button class="btn btn-outline-secondary w-100 mt-2">Reset</button>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-9">
                        <div class="row mb-4">
                            <div class="col-md-6">
                                <div class="input-group">
                                    <input type="text" class="form-control" placeholder="Search products..." />
                                    <button class="btn btn-primary" type="button">
                                        <i class="bi bi-search"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="col-md-6 text-md-end">
                                <div class="dropdown d-inline-block me-2">
                                    <button class="btn btn-outline-secondary dropdown-toggle" type="button" id="sortDropdown" data-bs-toggle="dropdown">
                                        Sort by: Featured
                                    </button>
                                    <ul class="dropdown-menu">
                                        <li><a class="dropdown-item" href="#">Featured</a></li>
                                        <li><a class="dropdown-item" href="#">Price: Low to High</a></li>
                                        <li><a class="dropdown-item" href="#">Price: High to Low</a></li>
                                        <li><a class="dropdown-item" href="#">Customer Rating</a></li>
                                        <li><a class="dropdown-item" href="#">Newest Arrivals</a></li>
                                    </ul>
                                </div>
                                <div class="btn-group" role="group">
                                    <button type="button" class="btn btn-outline-secondary active"><i class="fas fa-th"></i></button>
                                    <button type="button" class="btn btn-outline-secondary"><i class="fas fa-list"></i></button>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4 mb-4">
                                {pageData.items.forEach(i => {
                                    return <>
                                        <ProductCardItem 
                                            description={i.description}
                                            quantity={i.quantity}
                                            title={i.title}/>
                                    </>
                                })}
                            </div>
                        </div>

                        <nav aria-label="Page navigation">
                            <ul class="pagination justify-content-center">
                                <li class="page-item disabled">
                                    <a class="page-link" href="#" tabindex="-1">Previous</a>
                                </li>
                                <li class="page-item active"><a class="page-link" href="#">1</a></li>
                                <li class="page-item"><a class="page-link" href="#">2</a></li>
                                <li class="page-item"><a class="page-link" href="#">3</a></li>
                                <li class="page-item">
                                    <a class="page-link" href="#">Next</a>
                                </li>
                            </ul>
                        </nav>
                    </div>
                </div>
            </div>
        </>
    );
}

export default Home;