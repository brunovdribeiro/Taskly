'use client';

import { useEffect, useState, use } from 'react';
import { UserDto } from '../../../../api/generated/models';

interface PageProps {
    id: string;
}

export default function UserDetailsPage({params}: {params: Promise<PageProps>}) {
    const [user, setUser] = useState<UserDto | null>(null);
    const [error, setError] = useState<string | null>(null);
    const { id } = use(params);


    useEffect(() => {
        const fetchUser = async () => {
            try {
                const response = await fetch(`/api/users/${id}`);
                if (!response.ok) {
                    throw new Error('Failed to fetch user');
                }
                const data = await response.json();
                setUser(data);
            } catch {
                setError('Failed to load user');
            }
        };

        fetchUser();
    });

    if (error) return <div>{error}</div>;
    if (!user) return <div>Loading...</div>;

    return (
        <div className="p-4">
            <h1 className="text-2xl font-bold mb-4">User Details</h1>
            <div className="space-y-2">
                <p><span>Email:</span> {user.email}</p>
                <p><span className="font-semibold">Status:</span> {user.isActive ? 'Active' : 'Inactive'}</p>
                <p><span className="font-semibold">Created:</span> {new Date(user.createdAt!).toLocaleDateString()}</p>
                {user.lastModified && (
                    <p><span className="font-semibold">Last Modified:</span> {new Date(user.lastModified).toLocaleDateString()}</p>
                )}
            </div>
        </div>
    );
}