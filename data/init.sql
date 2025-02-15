CREATE TABLE task_snapshots (
                                id UUID NOT NULL,
                                title VARCHAR(255) NOT NULL,
                                description TEXT,
                                status VARCHAR(50) NOT NULL,
                                priority VARCHAR(50) NOT NULL,
                                created_at TIMESTAMP NOT NULL,
                                last_modified TIMESTAMP NOT NULL,
                                assigned_to UUID,
                                version INTEGER NOT NULL,
                                PRIMARY KEY (id, version)
);

CREATE TABLE user_snapshots (
                                id UUID NOT NULL,
                                name VARCHAR(255) NOT NULL,
                                email VARCHAR(255) NOT NULL,
                                created_at TIMESTAMP NOT NULL,
                                last_modified TIMESTAMP NOT NULL,
                                version INTEGER NOT NULL,
                                PRIMARY KEY (id, version)
);

CREATE INDEX idx_task_snapshots_id ON task_snapshots(id);
CREATE INDEX idx_user_snapshots_id ON user_snapshots(id);