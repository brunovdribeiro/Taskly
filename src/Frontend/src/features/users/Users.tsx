import {useGetUsersQuery} from "./usersApi.ts";
import {UserDto} from "../../api/generated/models.ts";

export const ListUsers = () => {
    const { data: users, isLoading, error } = useGetUsersQuery();

    if (isLoading) return <div>Loading...</div>;
    if (error) return <div>Error loading users</div>;

    return (
        <div>
            <h1>Users</h1>
            <ul>
                {users?.map((user: UserDto) => (
                    <li key={user.id}>{user.name}</li>
                ))}
            </ul>
        </div>
    );
};

export default ListUsers;