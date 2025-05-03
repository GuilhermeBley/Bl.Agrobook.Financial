
function ProductCardItem() {
    return (
        <>
            <div class="card h-100">
                <img src="oliveira-flores512.jpg" class="card-img-top" alt="Product 1" width="512" height="250" />
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
                    <div class="d-flex justify-content-end align-items-end text-end">
                        <div class="input-group">
                            <input className="form-control" type="number" value={1} style={{ maxWidth: 4 + 'rem' }} />
                            <div class="input-group-append">
                                <button class="btn btn-outline-primary" type="button" title="Adicionar no carrinho" alt="Adicionar no carrinho">
                                    <i class="bi bi-bag-plus"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default ProductCardItem;