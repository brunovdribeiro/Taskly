// src/Frontend/src/features/users/Users.tsx
import { useState } from 'react';
import { useGetUsersQuery } from "./usersApi.ts";
import { UserDto, CreateUserDto } from "../../api/generated/models.ts";
import {PageHeader} from "../../layouts/PageHeader.tsx";
import {CreateUser} from "./CreateUser.tsx";

export const ListUsers = () => {
    const [showForm, setShowForm] = useState(false);
    const { data: users, isLoading, error } = useGetUsersQuery();

    if (isLoading) return <div>Loading...</div>;
    if (error) return <div>Error loading users</div>;


    const handleSubmit = (userData: CreateUserDto) => {
        // Here you'll add the API call to create a user
        console.log('Creating user:', userData);
        setShowForm(false);
    };
    
    return (
        <div>
            <PageHeader
                title="Users"
                actionButton={{
                    label: showForm ? "View List" : "Add User",
                    onClick: () => setShowForm(!showForm)
                }}
            />
            <div className="px-4">
                {showForm ? (
                    <div className="bg-white shadow rounded-lg p-4">
                        <CreateUser
                            onSubmit={handleSubmit}
                            onCancel={() => setShowForm(false)}
                        />
                    </div>
                ) : (
                    <div className="bg-white shadow rounded-lg p-4">
                        <ul>
                            {users?.map((user: UserDto) => (
                                <li key={user.id}>{user.name}</li>
                            ))}
                        </ul>
                    </div>
                )}
            </div>
        </div>
    );
};

export default ListUsers;