import React from 'react';
import { Link } from 'react-router-dom';
import './Home.css';

export default function Home() {
    return (
        <div className="home-container">
            {/* Sidebar */}
            <aside className="sidebar">
                <div>
                    <h2 className="sidebar-title">Menu</h2>
                    <nav className="nav-links">
                        <Link to="/users" className="nav-link">Cadastro de Usuários</Link>
                        <Link to="/assets" className="nav-link">Cadastro de Ativos</Link>
                        <Link to="/trades" className="nav-link">Cadastro de Trades</Link>
                        <Link to="/quotes" className="nav-link">Cadastro de Cotações</Link>
                        <Link to="/position" className="nav-link">Consulta de Posição</Link>
                    </nav>
                </div>
                <footer className="sidebar-footer">&copy; 2025 Itaú Investimentos</footer>
            </aside>

            {/* Main Content */}
            <main className="main-content">
                <img src="/banner.jpg" alt="Banner Principal" className="banner" />
                <h1 className="main-title">Bem-vindo à Plataforma de Investimentos</h1>
                <p className="main-subtitle">Gerencie ativos, operações e acompanhe sua carteira em um só lugar.</p>
            </main>
        </div>
    );
}
