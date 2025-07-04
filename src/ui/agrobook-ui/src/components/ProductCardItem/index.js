import { useState } from "react";

function ProductCardItem({ title, quantity, description, imgUrl, initialQtt = 0, onItemChanged = (qtty) => { } }) {

    const [cardInfo, setCardInfo] = useState({
        qttSelected: initialQtt
    });

    const changeSelectedItemCount = (qtt) => {
        qtt = parseInt(qtt)
        if (!qtt || qtt < 0) {
            qtt = 0;
        }

        setCardInfo(p => ({
            ...p,
            qttSelected: qtt
        }));
        onItemChanged(qtt);
    }

    return (
        <>
            <div class="card h-100">
                <img src={(imgUrl === undefined || imgUrl === '' ? "oliveira-flores512.jpg" : imgUrl)} class="card-img-top" alt="Product 1" width="450" height="250" />
                <div class="card-body">
                    <h5 class="card-title">{title}</h5>
                    <div class="rating mb-2">
                        <i class="fas fa-star-half-alt"></i>
                        <span class="text-muted ms-1">
                            {quantity > 0 || quantity <= 0
                                ? <>({quantity})</>
                                : <></>}

                        </span>
                    </div>
                    <p class="card-text">{description}</p>
                    <div class="d-flex justify-content-end align-items-end text-end">
                        <div class="input-group">
                            <input className="form-control" type="number" onChange={e => changeSelectedItemCount(e.target.value)} value={cardInfo.qttSelected} style={{ maxWidth: 4 + 'rem' }} />
                            <div class="input-group-append">
                                <button class="btn btn-success" onClick={e => changeSelectedItemCount(cardInfo.qttSelected + 1)} type="button" title="Adicionar no carrinho" alt="Adicionar no carrinho">
                                    <i class="bi bi-bag-plus"></i> Adicionar
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