import PageNavigationBar from "../../components/PageNavigationBar";

function Home() {
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
                                    <input type="range" class="form-range" min="0" max="1000" step="10" id="priceRange"/>
                                        <div class="d-flex justify-content-between">
                                            <span>$0</span>
                                            <span>$1000</span>
                                        </div>
                                </div>

                                <h6 class="mt-4">Brand</h6>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="brand1"/>
                                        <label class="form-check-label" for="brand1">Brand A</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="brand2"/>
                                        <label class="form-check-label" for="brand2">Brand B</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" id="brand3"/>
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
                                    <input type="text" class="form-control" placeholder="Search products..."/>
                                        <button class="btn btn-primary" type="button">
                                            <i class="fas fa-search"></i>
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
                                <div class="card h-100">
                                    <span class="badge bg-danger badge-sale">Sale</span>
                                    <img src="https://via.placeholder.com/300x200?text=Product+1" class="card-img-top" alt="Product 1"/>
                                        <div class="card-body">
                                            <h5 class="card-title">Wireless Headphones</h5>
                                            <div class="rating mb-2">
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star-half-alt"></i>
                                                <span class="text-muted ms-1">(24)</span>
                                            </div>
                                            <p class="card-text">High-quality wireless headphones with noise cancellation.</p>
                                            <div class="d-flex justify-content-between align-items-center">
                                                <div>
                                                    <span class="price">$89.99</span>
                                                    <span class="old-price ms-2">$129.99</span>
                                                </div>
                                                <button class="btn btn-sm btn-outline-primary">
                                                    <i class="fas fa-cart-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                </div>
                            </div>

                            <div class="col-md-4 mb-4">
                                <div class="card h-100">
                                    <img src="https://via.placeholder.com/300x200?text=Product+2" class="card-img-top" alt="Product 2"/>
                                        <div class="card-body">
                                            <h5 class="card-title">Smart Watch</h5>
                                            <div class="rating mb-2">
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="far fa-star"></i>
                                                <span class="text-muted ms-1">(18)</span>
                                            </div>
                                            <p class="card-text">Fitness tracker and smartwatch with heart rate monitor.</p>
                                            <div class="d-flex justify-content-between align-items-center">
                                                <span class="price">$149.99</span>
                                                <button class="btn btn-sm btn-outline-primary">
                                                    <i class="fas fa-cart-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                </div>
                            </div>

                            <div class="col-md-4 mb-4">
                                <div class="card h-100">
                                    <span class="badge bg-success badge-sale">New</span>
                                    <img src="https://via.placeholder.com/300x200?text=Product+3" class="card-img-top" alt="Product 3"/>
                                        <div class="card-body">
                                            <h5 class="card-title">Bluetooth Speaker</h5>
                                            <div class="rating mb-2">
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <span class="text-muted ms-1">(42)</span>
                                            </div>
                                            <p class="card-text">Portable waterproof speaker with 20-hour battery life.</p>
                                            <div class="d-flex justify-content-between align-items-center">
                                                <span class="price">$59.99</span>
                                                <button class="btn btn-sm btn-outline-primary">
                                                    <i class="fas fa-cart-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                </div>
                            </div>

                            <div class="col-md-4 mb-4">
                                <div class="card h-100">
                                    <img src="https://via.placeholder.com/300x200?text=Product+4" class="card-img-top" alt="Product 4"/>
                                        <div class="card-body">
                                            <h5 class="card-title">Laptop Backpack</h5>
                                            <div class="rating mb-2">
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star-half-alt"></i>
                                                <span class="text-muted ms-1">(31)</span>
                                            </div>
                                            <p class="card-text">Durable backpack with USB charging port and anti-theft design.</p>
                                            <div class="d-flex justify-content-between align-items-center">
                                                <span class="price">$39.99</span>
                                                <button class="btn btn-sm btn-outline-primary">
                                                    <i class="fas fa-cart-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                </div>
                            </div>

                            <div class="col-md-4 mb-4">
                                <div class="card h-100">
                                    <img src="https://via.placeholder.com/300x200?text=Product+5" class="card-img-top" alt="Product 5"/>
                                        <div class="card-body">
                                            <h5 class="card-title">Wireless Charger</h5>
                                            <div class="rating mb-2">
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="far fa-star"></i>
                                                <span class="text-muted ms-1">(15)</span>
                                            </div>
                                            <p class="card-text">Fast charging pad compatible with all Qi-enabled devices.</p>
                                            <div class="d-flex justify-content-between align-items-center">
                                                <span class="price">$24.99</span>
                                                <button class="btn btn-sm btn-outline-primary">
                                                    <i class="fas fa-cart-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                </div>
                            </div>

                            <div class="col-md-4 mb-4">
                                <div class="card h-100">
                                    <span class="badge bg-danger badge-sale">-30%</span>
                                    <img src="https://via.placeholder.com/300x200?text=Product+6" class="card-img-top" alt="Product 6"/>
                                        <div class="card-body">
                                            <h5 class="card-title">4K Action Camera</h5>
                                            <div class="rating mb-2">
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star"></i>
                                                <i class="fas fa-star-half-alt"></i>
                                                <span class="text-muted ms-1">(27)</span>
                                            </div>
                                            <p class="card-text">Waterproof camera with 4K video and image stabilization.</p>
                                            <div class="d-flex justify-content-between align-items-center">
                                                <div>
                                                    <span class="price">$199.99</span>
                                                    <span class="old-price ms-2">$279.99</span>
                                                </div>
                                                <button class="btn btn-sm btn-outline-primary">
                                                    <i class="fas fa-cart-plus"></i>
                                                </button>
                                            </div>
                                        </div>
                                </div>
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