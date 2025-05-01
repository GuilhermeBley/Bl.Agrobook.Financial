import { HashLink } from 'react-router-hash-link';

function NavigationBar() {
    return (
        <>
            <div
                id="top"
                className="relative"
            >
                <nav
                    id="navigationBar"
                    className="z-30 w-full h-16 fixed bg-gray-500"
                >

                    <div className="max-w-6xl mx-auto px-6 lg:flex flex-row justify-between items-bottom">
                        <HashLink smooth to="#top">
                            <div
                                id="mnu-home"
                                className="my-auto p-2 text-xl cursor-pointer border-b-2 border-transparent"
                            >
                                Home
                            </div>
                        </HashLink>

                        <div className="flex flex-row space-x-4">
                            <HashLink
                                id="mnu-tickets"
                                to="#tickets"
                                className="my-auto p-2 text-xl cursor-pointer border-b-2 border-transparent"
                                smooth
                            >
                                Tickets
                            </HashLink>
                        </div>

                    </div>

                </nav>
            </div>
        </>
    );
}

export default NavigationBar;