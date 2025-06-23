import React, { useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import './QuoteForm.css';

function QuoteForm() {
    const [assetId, setAssetId] = useState('');
    const [unitPrice, setUnitPrice] = useState('');
    const [response, setResponse] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();

        const payload = {
            assetId: parseInt(assetId),
            unitPrice: parseFloat(unitPrice)
        };

        try {
            const res = await axios.post('http://localhost:5000/api/Quote', payload);
            setResponse(res.data);
            setAssetId('');
            setUnitPrice('');
        } catch (err) {
            console.error("Erro ao registrar cotação:", err);
            alert('Erro ao registrar cotação. Verifique o console.');
        }
    };

    return (
        <div className="page-container">
            <Link to="/" className="back-button">⬅️ Voltar à Home</Link>

            <h2 className="page-title">Registrar Cotação</h2>
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label>ID do Ativo</label>
                    <input type="number" value={assetId} onChange={e => setAssetId(e.target.value)} required />
                </div>
                <div className="form-group">
                    <label>Preço</label>
                    <input type="number" step="0.01" value={unitPrice} onChange={e => setUnitPrice(e.target.value)} required />
                </div>
                <button className="btn-submit">Registrar</button>
            </form>

            {response && (
                <pre className="response-box">
                    {JSON.stringify(response, null, 2)}
                </pre>
            )}
        </div>
    );
}

export default QuoteForm;
