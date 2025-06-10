// src/UserForm.js
import React, { useState } from 'react';
import axios from 'axios';

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

        console.log("Enviando payload:", payload);

        try {
            const res = await axios.post('http://localhost:5000/api/User', payload);
            console.log("Resposta recebida:", res.data);
            setResponse(res.data);
            setName('');
            setEmail('');
            setFeePercentage('');
        } catch (err) {
            console.error("Erro ao cadastrar usuário:", err);
            if (err.response) {
                console.error("Erro na resposta da API:", err.response.data);
            }
            alert('Erro ao cadastrar usuário. Veja o console.');
        }
    };


    return (
        <div>
            <h2>Cadastrar Usuário</h2>
            <form onSubmit={handleSubmit}>
                <div className="mb-2">
                    <label>Nome</label>
                    <input className="form-control" value={name} onChange={e => setName(e.target.value)} required />
                </div>
                <div className="mb-2">
                    <label>Email</label>
                    <input className="form-control" type="email" value={email} onChange={e => setEmail(e.target.value)} required />
                </div>
                <div className="mb-2">
                    <label>Fee (%)</label>
                    <input className="form-control" type="number" step="0.01" value={feePercentage} onChange={e => setFeePercentage(e.target.value)} required />
                </div>
                <button className="btn btn-primary">Cadastrar</button>
            </form>
            {response && (
                <pre className="mt-3 bg-light p-2">{JSON.stringify(response, null, 2)}</pre>
            )}
        </div>
    );
}

export default UserForm;
