import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Login from './components/login';
import ProtectedRoute from './components/ProtectedRoute';
import Dashboard from './components/Dashboard';

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/dashboard" element=
                {
                  <ProtectedRoute>
                    <Dashboard />
                  </ProtectedRoute>
                } />
                <Route path="/" element={<Login />} />
            </Routes>
        </Router>
    );
}

export default App;