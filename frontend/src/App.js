import React from 'react';
import QuoteForm from './QuoteForm';
import PositionCheck from './PositionCheck';
import UserForm from './UserForm';

function App() {
    return (
        <div className="container mt-4">
            <h1>Itaú Investimentos</h1>
            <UserForm />
            <hr />
            <QuoteForm />
            <hr />
            <PositionCheck />
        </div>
    );
}

export default App;
