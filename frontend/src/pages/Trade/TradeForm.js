import React, { useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import './TradeForm.css';

function TradeForm() {
    const [userId, setUserId] = useState('');
    const [assetId, setAssetId] = useState('');
    const [quantity, setQuantity] = useState('');
    const [unitPrice, setUnitPrice] = useState('');
    const [fee, setFee] = useState('');
    const [tradeType, setTradeType] = useState('BUY');

    const [response, setResponse] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();

        const payload = {
            userId: parseInt(userId),
            assetId: parseInt(assetId),
            quantity: parseInt(quantity),
            unitPrice: parseFloat(unitPrice),
            fee: parseFloat(fee),
            tradeType
        };

        try {
            console.log(payload);
            const res = await axios.post('http://localhost:5000/api/Trade', payload);
            console.log(payload);
            setResponse(res.data);
            setUserId('');
            setAssetId('');
            setQuantity('');
            setUnitPrice('');
            setTradeType('BUY');
            setFee('');
        } catch (err) {
            console.error("Erro ao registrar operação:", err);
            alert('Erro ao registrar operação.');
        }
    };

    return (
        <div className="page-container">
            <Link to="/" className="back-button">⬅️ Voltar à Home</Link>

            <h2 className="page-title">Registrar Operação (Trade)</h2>
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label>ID do Usuário</label>
                    <input type="number" value={userId} onChange={e => setUserId(e.target.value)} required />
                </div>
                <div className="form-group">
                    <label>Código do Ativo</label>
                    <input value={assetId} onChange={e => setAssetId(e.target.value)} required />
                </div>
                <div className="form-group">
                    <label>Quantidade</label>
                    <input type="number" step="1" value={quantity} onChange={e => setQuantity(e.target.value)} required />
                </div>
                <div className="form-group">
                    <label>Preço Unitário</label>
                    <input type="number" step="0.01" value={unitPrice} onChange={e => setUnitPrice(e.target.value)} required />
                </div>
                <div className="form-group">
                    <label>Taxa</label>
                    <input type="number" step="0.01" value={fee} onChange={e => setFee(e.target.value)} required />
                </div>
                <div className="form-group">
                    <label>Operação</label>
                    <select value={tradeType} onChange={e => setTradeType(e.target.value)} required>
                        <option value="BUY">Compra</option>
                        <option value="SELL">Venda</option>
                    </select>
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

export default TradeForm;
