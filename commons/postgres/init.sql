-- PulseFlow Database Initialization Script

-- Create extensions if needed
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pg_trgm";

-- Create a sample schema (optional, for organization)
-- CREATE SCHEMA IF NOT EXISTS pulseflow;

-- Grant privileges
GRANT ALL PRIVILEGES ON DATABASE pulseflow_db TO pulseflow_user;

-- Log initialization
DO $$
BEGIN
	RAISE NOTICE 'PulseFlow database initialized successfully';
END $$;
