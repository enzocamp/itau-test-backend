import React from 'react';
import QuoteForm from './QuoteForm';
import PositionCheck from './PositionCheck';

function App() {
  return (
    <div className="container mt-4">
      <h1>Itaú Investimentos</h1>
      <QuoteForm />
      <hr />
      <PositionCheck />
    </div>
  );
}

export default App;
