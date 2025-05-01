import { Link } from "react-router-dom";

function PageNavigationBar() {
    return (
        <>
            <nav
                id="navigationBar"
                className="z-30 w-full h-16 fixed bg-gray-500"
            >

                <div className="max-w-6xl mx-auto px-6 lg:flex flex-row justify-between items-bottom">
                    <Link to="/Home">
                        <div
                            id="mnu-home"
                            className="my-auto p-2 text-xl cursor-pointer border-b-2 border-transparent"
                        >
                            Home
                        </div>
                    </Link>
                    <Link to="/Order">
                        <div
                            id="mnu-home"
                            className="my-auto p-2 text-xl cursor-pointer border-b-2 border-transparent"
                        >
                            Orders
                        </div>
                    </Link>

                </div>

            </nav>
        </>
    );
}

export default PageNavigationBar;