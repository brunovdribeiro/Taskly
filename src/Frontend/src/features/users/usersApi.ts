import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { UserDto } from '../../api/generated/models';

export const usersApi = createApi({
    reducerPath: 'api',
    baseQuery: fetchBaseQuery({ baseUrl: '/api' }),
    endpoints: (builder) => ({
        getUsers: builder.query<UserDto[], void>({
            query: () => 'users',
        }),
        getUserById: builder.query<UserDto, string>({
            query: (id) => `users/${id}`,
        }),
        createUser: builder.mutation<UserDto, Partial<UserDto>>({
            query: (body) => ({
                url: 'users',
                method: 'POST',
                body,
            }),
        }),
        deactivateUser: builder.mutation<UserDto, string>({
            query: (id) => ({
                url: `users/${id}/deactivate`,
                method: 'PUT',
            }),
        }),
    }),
});

export const {
    useGetUsersQuery,
    useGetUserByIdQuery,
    useCreateUserMutation,
    useDeactivateUserMutation,
} = usersApi;