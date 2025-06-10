
import React, { useState } from 'react';
import axios from 'axios';

function AssetForm() {
    const [code, setCode] = useState('');
    const [name, setName] = useState('');
    const [response, setResponse] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();

        const payload = {
            code,
            name
        };

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
        <div>
            <h2>Cadastrar Ativo</h2>
            <form onSubmit={handleSubmit}>
                <div className="mb-2">
                    <label>Nome</label>
                    <input className="form-control" value={name} onChange={e => setName(e.target.value)} required />
                </div>
                <div className="mb-2">
                    <label>Codígo</label>
                    <input className="form-control" value={code} onChange={e => setCode(e.target.value)} required />
                </div>
                <button className="btn btn-primary">Cadastrar</button>
            </form>
            {response && (
                <pre className="mt-3 bg-light p-2">{JSON.stringify(response, null, 2)}</pre>
            )}
        </div>
    );
}

export default AssetForm;
