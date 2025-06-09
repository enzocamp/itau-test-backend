
-- Schema creation script (MySQL) using snake_case and English naming

CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    fee_percentage DECIMAL(5,2) NOT NULL
);

CREATE TABLE assets (
    id INT AUTO_INCREMENT PRIMARY KEY,
    code VARCHAR(10) NOT NULL UNIQUE,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE trades (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    asset_id INT NOT NULL,
    quantity INT UNSIGNED NOT NULL,
    unit_price DECIMAL(15,6) NOT NULL,
    trade_type ENUM('BUY', 'SELL') NOT NULL,
    fee DECIMAL(6,2) NOT NULL,
    executed_at DATETIME NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (asset_id) REFERENCES assets(id)
);

CREATE TABLE quotes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    asset_id INT NOT NULL,
    unit_price DECIMAL(15,6) NOT NULL,
    quoted_ad DATETIME NOT NULL,
    FOREIGN KEY (asset_id) REFERENCES assets(id)
);

CREATE TABLE positions (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    asset_id INT NOT NULL,
    quantity INT UNSIGNED NOT NULL,
    average_price DECIMAL(15,6) NOT NULL,
    pnl DECIMAL(12,6) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (asset_id) REFERENCES assets(id)
);

-- Index to optimize queries by user, asset and date (last 30 days)
CREATE INDEX idx_trades_user_asset_date
ON trades (user_id, asset_id, executed_at DESC);

SELECT *
FROM trades
WHERE user_id = @UserId
  AND asset_id = @AssetId
  AND executed_at >= NOW() - INTERVAL 30 DAY
ORDER BY executed_at DESC;
