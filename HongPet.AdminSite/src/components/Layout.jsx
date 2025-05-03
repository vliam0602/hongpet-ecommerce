import { Outlet, NavLink, Navigate, useLocation } from 'react-router-dom'
import { LayoutDashboard, Package, Tag, Users, ShoppingCart, LogOut, User } from 'lucide-react'

function Layout({ user, onLogout }) {
  const location = useLocation();
  if (!user) {
    return <Navigate to="/login" />
  }

  return (
    <div className="flex h-screen">
      {/* Sidebar */}
      <div className="w-64 bg-black flex flex-col">
        <div className="p-4 border-b border-gray-800">
          <h1 className="text-primary text-xl font-bold">Hồng Pét Admin</h1>
        </div>
        <nav className="flex-1 py-4">
          <NavLink to="/" className={({ isActive }) => `sidebar-link ${isActive ? 'active' : ''}`}>
            <LayoutDashboard size={20} />
            <span>Dashboard</span>
          </NavLink>
          <NavLink to="/products" 
            className={({ isActive }) => `sidebar-link ${isActive ? 'active' : ''}`}>
            <Package size={20} />
            <span>Products</span>
          </NavLink>
          <NavLink to="/categories" 
            className={({ isActive }) => `sidebar-link ${isActive ? 'active' : ''}`}>
            <Tag size={20} />
            <span>Categories</span>
          </NavLink>
          <NavLink to="/customers" 
            className={({ isActive }) => `sidebar-link ${isActive ? 'active' : ''}`}>
            <Users size={20} />
            <span>Customers</span>
          </NavLink>
          <NavLink to="/orders" 
            className={({ isActive }) => `sidebar-link ${isActive ? 'active' : ''}`}>
            <ShoppingCart size={20} />
            <span>Orders</span>
          </NavLink>
        </nav>
        <div className="p-4 border-t border-gray-800">
          <button 
            onClick={onLogout} 
            className="flex items-center gap-2 text-white hover:text-primary w-full"
          >
            <LogOut size={20} />
            <span>Logout</span>
          </button>
        </div>
      </div>

      {/* Main Content */}
      <div className="flex-1 flex flex-col overflow-hidden">
        <header className="bg-white shadow-sm p-4 flex justify-between items-center">
          <h2 className="text-xl font-semibold">
            {location.pathname === '/' ? 'Dashboard' : 
            location.pathname.split('/')[1].charAt(0).toUpperCase() + 
            location.pathname.split('/')[1].slice(1)}
          </h2>
          <div className="flex items-center gap-2">
            <User size={20} />
            <span>{user.email}</span>
          </div>
        </header>
        <main className="flex-1 overflow-auto p-6">
          <Outlet />
        </main>
      </div>
    </div>
  )
}

export default Layout