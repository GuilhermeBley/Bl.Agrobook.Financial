import { useEffect } from "react";
import { Link } from "react-router-dom";

function PageNavigationBar() {

    useEffect(() => {
        // TODO: set the token from the local storage to the input
        // then add the 
    }, [])

    const handleInputChange = () => {
        // TODO: change the input
        // TODO: change axios default headers
        // TODO: change the local storage
    }

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

                        <ul class="navbar-nav">
                            <li class="nav-item">
                                <div class="dropdown">
                                    <button class="btn btn-outline-primary border-0 dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                        <i class="bi bi-person-circle"></i>
                                    </button>
                                    <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                                        <h6 class="dropdown-header mx-2">Token do usuário</h6>
                                        <input type="password" className="form-control" placeholder="Digite o token"/>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>
        </>
    );
}

export default PageNavigationBar;