import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Home from './pages/Home/Home';
import UserForm from './pages/User/UserForm';
import AssetForm from './pages/Asset/AssetForm';
import QuoteForm from './pages/Quote/QuoteForm';
import PositionCheck from './pages/Position/PositionCheck';
import TradeForm from './pages/Trade/TradeForm';

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/users" element={<UserForm />} />
                <Route path="/assets" element={<AssetForm />} />
                <Route path="/quotes" element={<QuoteForm />} />
                <Route path="/trades" element={<TradeForm />} />
                <Route path="/position" element={<PositionCheck />} />
            </Routes>
        </Router>
    );
}


export default App;
