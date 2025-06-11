import React, { useState } from 'react';
import axios from 'axios';

function QuoteForm() {
  const [assetId, setAssetId] = useState('');
  const [unitPrice, setUnitPrice] = useState('');
  const [response, setResponse] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await axios.post('http://localhost:5000/api/quote', {
        assetId: parseInt(assetId),
        unitPrice: parseFloat(unitPrice)
      });
      setResponse(res.data);
    } catch (err) {
      alert('Erro ao registrar cotação');
    }
  };

  return (
    <div>
      <h2>Registrar Cotação</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-2">
          <label>Asset ID</label>
          <input className="form-control" value={assetId} onChange={e => setAssetId(e.target.value)} required />
        </div>
        <div className="mb-2">
          <label>Unit Price</label>
          <input className="form-control" value={unitPrice} onChange={e => setUnitPrice(e.target.value)} required />
        </div>
        <button className="btn btn-primary">Registrar</button>
      </form>
      {response && (
        <pre className="mt-3 bg-light p-2">{JSON.stringify(response, null, 2)}</pre>
      )}
    </div>
  );
}

export default QuoteForm;
