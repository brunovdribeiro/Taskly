'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { UserDto } from '../../../api/generated/models';

export default function UsersPage() {
    const [users, setUsers] = useState<UserDto[]>([]);

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const response = await fetch('/api/users');
                if (response.ok) {
                    const data = await response.json();
                    setUsers(data);
                }
            } catch (error) {
                console.error('Error fetching users:', error);
            }
        };

        fetchUsers();
    }, []);

    return (
        <div className="p-4">
            <h1 className="text-2xl font-bold mb-4">Users</h1>
            <ul>
                {users.map((user) => (
                    <li key={user.id} className="mb-2">
                        <Link
                            href={`/users/${user.id}`}
                            className="text-blue-600 hover:underline"
                        >
                            {user.name} ({user.email})
                        </Link>
                    </li>
                ))}
            </ul>
        </div>
    );
}