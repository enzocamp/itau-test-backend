import React, { useState } from 'react';
import axios from 'axios';

function PositionCheck() {
  const [userId, setUserId] = useState('');
  const [assetId, setAssetId] = useState('');
  const [response, setResponse] = useState(null);

  const fetchPosition = async (e) => {
    e.preventDefault();
    try {
      const res = await axios.get(`http://localhost:5000/api/position/${userId}/${assetId}`);
      setResponse(res.data);
    } catch {
      alert('Erro ao obter posição');
    }
  };

  return (
    <div>
      <h2>Consultar Posição</h2>
      <form onSubmit={fetchPosition}>
        <div className="mb-2">
          <label>User ID</label>
          <input className="form-control" value={userId} onChange={e => setUserId(e.target.value)} required />
        </div>
        <div className="mb-2">
          <label>Asset ID</label>
          <input className="form-control" value={assetId} onChange={e => setAssetId(e.target.value)} required />
        </div>
        <button className="btn btn-success">Consultar</button>
      </form>
      {response && (
        <pre className="mt-3 bg-light p-2">{JSON.stringify(response, null, 2)}</pre>
      )}
    </div>
  );
}

export default PositionCheck;
