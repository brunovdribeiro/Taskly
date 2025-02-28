import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import ListUsers from '../features/users/Users';
import Main from '../layouts/Main';

const router = createBrowserRouter([
    {
        element: <Main />,
        children: [
            {
                path: '/',
                element: (
                    <h1 className="text-3xl font-bold underline">
                        Hello world!
                    </h1>
                ),
            },
            {
                path: '/users',
                element: <ListUsers />,
            },
        ],
    },
]);

export const Router = () => {
    return <RouterProvider router={router} />;
};

export default Router;