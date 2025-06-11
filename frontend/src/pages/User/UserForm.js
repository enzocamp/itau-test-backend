import React, { useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import './UserForm.css';

function UserForm() {
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [feePercentage, setFeePercentage] = useState('');
    const [response, setResponse] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();

        const payload = {
            name,
            email,
            fee_percentage: parseFloat(feePercentage)
        };

        try {
            const res = await axios.post('http://localhost:5000/api/User', payload);
            console.log("Resposta recebida:", res.data);
            setResponse(res.data);
            setName('');
            setEmail('');
            setFeePercentage('');
        } catch (err) {
            if (err.response) {
                console.error("Erro na resposta da API:", err.response.data);
            }
            alert('Erro ao cadastrar usuário. Veja o console.');
        }
    };

    return (
        <div className="page-container">
            <Link to="/" className="back-button">⬅️ Voltar à Home</Link>

            <h2 className="page-title">Cadastrar Usuário</h2>
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label>Nome</label>
                    <input value={name} onChange={e => setName(e.target.value)} required />
                </div>
                <div className="form-group">
                    <label>Email</label>
                    <input type="email" value={email} onChange={e => setEmail(e.target.value)} required />
                </div>
                <div className="form-group">
                    <label>Fee (%)</label>
                    <input type="number" step="0.01" value={feePercentage} onChange={e => setFeePercentage(e.target.value)} required />
                </div>
                <button className="btn-submit">Cadastrar</button>
            </form>

            {response && (
                <pre className="response-box">
                    {JSON.stringify(response, null, 2)}
                </pre>
            )}
        </div>
    );
}

export default UserForm;
