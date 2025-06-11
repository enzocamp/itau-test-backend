import React, { useState } from 'react';
import axios from 'axios';
import './AssetForm.css';
import { Link } from 'react-router-dom';

function AssetForm() {
    const [code, setCode] = useState('');
    const [name, setName] = useState('');
    const [response, setResponse] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();

        const payload = { code, name };

        try {
            const res = await axios.post('http://localhost:5000/api/Asset', payload);
            setResponse(res.data);
            setCode('');
            setName('');
        } catch (err) {
            if (err.response) {
                console.error("Erro na resposta da API:", err.response.data);
            }
            alert('Erro ao cadastrar ativo. Veja o console.');
        }
    };

    return (
        <div className="page-container">
            <Link to="/" className="back-button">⬅️ Voltar para Home</Link>
            <h2 className="page-title">Cadastrar Ativo</h2>
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label>Nome</label>
                    <input value={name} onChange={e => setName(e.target.value)} required />
                </div>
                <div className="form-group">
                    <label>Código</label>
                    <input value={code} onChange={e => setCode(e.target.value)} required />
                </div>
                <button className="btn-submit">Cadastrar</button>
            </form>
            {response && (
                <pre style={{ marginTop: '24px', background: '#f1f5f9', padding: '16px', borderRadius: '8px' }}>
                    {JSON.stringify(response, null, 2)}
                </pre>
            )}
        </div>
    );
}

export default AssetForm;
