import { Link } from "react-router-dom";

function PageNavigationBar() {
    return (
        <>
            <nav class="navbar navbar-expand-lg">
                <div class="container">
                    <a class="navbar-brand" href="#">Oliveira Flores</a>
                    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                        <span class="navbar-toggler-icon"></span>
                    </button>
                    <div class="collapse navbar-collapse" id="navbarNav">
                        <ul class="navbar-nav me-auto">
                            <li class="nav-item">
                                <Link to="/Home" className="nav-link">
                                    Catálogo
                                </Link>
                            </li>
                            <li class="nav-item">
                                <Link to="/Order" className="nav-link">
                                    Pedidos
                                </Link>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>
        </>
    );
}

export default PageNavigationBar;