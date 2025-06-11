import React, { useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import './PositionCheck.css';

function PositionCheck() {
    const [userId, setUserId] = useState('');
    const [assetId, setAssetId] = useState('');
    const [response, setResponse] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const res = await axios.get(`http://localhost:5000/api/Position/${userId}/${assetId}`);
            setResponse(res.data);
        } catch (err) {
            console.error("Erro ao consultar posição:", err);
            alert('Erro ao consultar posição.');
        }
    };

    return (
        <div className="page-container">
            <Link to="/" className="back-button">⬅️ Voltar à Home</Link>

            <h2 className="page-title">Consultar Posição</h2>
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label>ID do Usuário</label>
                    <input type="number" value={userId} onChange={e => setUserId(e.target.value)} required />
                </div>
                <div className="form-group">
                    <label>ID do Ativo</label>
                    <input type="number" value={assetId} onChange={e => setAssetId(e.target.value)} required />
                </div>
                <button className="btn-submit">Consultar</button>
            </form>

            {response && (
                <pre className="response-box">
                    {JSON.stringify(response, null, 2)}
                </pre>
            )}
        </div>
    );
}

export default PositionCheck;
