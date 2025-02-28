import { Outlet, Link } from 'react-router-dom';

const Main = () => {
    return (
        <div>
            <nav className="mb-4">
                <Link to="/" className="mr-4">Home</Link>
                <Link to="/users">Users</Link>
            </nav>
            <main>
                <Outlet />
            </main>
        </div>
    );
};

export default Main;