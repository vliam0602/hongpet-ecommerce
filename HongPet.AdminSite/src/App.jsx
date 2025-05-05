import { useEffect, useState, useCallback } from 'react'
import { Routes, useLocation, useNavigate, Navigate, Route } from "react-router-dom";
import Layout from "./components/Layout";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import Products from "./pages/products/Products";
import AddProduct from "./pages/products/AddProduct";
import EditProduct from "./pages/products/EditProduct";
import ProductDetail from "./pages/products/ProductDetail";
import Categories from "./pages/categories/Categories";
import Customers from "./pages/customers/Customers";
import Orders from "./pages/orders/Orders";
import OrderDetail from "./pages/orders/OrderDetail";
import authService from "./services/authService";

function App() {
  const [user, setUser] = useState(null);
  const navigate = useNavigate();
  const location = useLocation();

    // Define handleLogin as useCallback to use in dependency array
    const handleLogin = useCallback((userData) => {
      setUser(userData);    
      navigate('/');
    }, [navigate]);

    // Authentication check when app loads
    useEffect(() => {
      if (authService.isAuthenticated()) {
        const userData = authService.getCurrentUser();
        if (userData) {
          // User is logged in with a valid token
          setUser(userData);
          // Only redirect to dashboard if on login page
          if (location.pathname === '/login') {
            navigate('/');
          }
        }
      } else if (location.pathname !== '/login') {
        // Not authenticated and not on login page, redirect to login
        navigate('/login');
      }
    }, [navigate, location.pathname]);

  const handleLogout = () => {
    setUser(null);
    authService.logout();
    navigate('/login');
  }

  // Rest of your code remains unchanged
  return (
    <Routes>
      <Route path="/login" element={<Login onLogin={handleLogin} />} />
      <Route element={<Layout user={user} onLogout={handleLogout} />}>
          {/* Route map for main sections */}
          <Route path="/" element={<Dashboard />} />

          <Route path="/products" element={<Products />} />
          <Route path="/products/add" element={<AddProduct />} />
          <Route path="/products/:id" element={<ProductDetail />} />
          <Route path="/products/edit/:id" element={<EditProduct />} />

          <Route path="/categories" element={<Categories />} />
          
          <Route path="/customers" element={<Customers />} />

          <Route path="/orders" element={<Orders />} />
          <Route path="/orders/:id" element={<OrderDetail />} />
      </Route>
      <Route path="*" element={<Navigate to="/" />} />
    </Routes>
  )
}

export default App;