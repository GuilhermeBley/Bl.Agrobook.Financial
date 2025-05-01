import { Routes, Route } from 'react-router-dom';
import Home from '../../pages/Home';
import Order from '../../pages/Order';


function Pages() {
    return (
        <Routes>
            <Route path="/" exact element={<Home />} />
            <Route path="/Home" exact element={<Home />} />
            <Route path="/Order" exact element={<Order />} />
        </Routes>
    )
}

export default Pages;