import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { Package, Tag, Users, ShoppingCart, TrendingUp, DollarSign } from 'lucide-react'

// Mock data for dashboard
const mockStats = {
  totalProducts: 124,
  totalCategories: 15,
  totalCustomers: 543,
  totalOrders: 267,
  revenue: 15420,
  recentOrders: [
    { id: '1', customer: 'John Doe', date: '2025-04-30', total: 120, status: 'Completed' },
    { id: '2', customer: 'Jane Smith', date: '2025-04-29', total: 85, status: 'Processing' },
    { id: '3', customer: 'Bob Johnson', date: '2025-04-28', total: 210, status: 'Pending' },
    { id: '4', customer: 'Alice Brown', date: '2025-04-27', total: 65, status: 'Completed' },
  ],
  topProducts: [
    { id: '1', name: 'Premium Dog Food', sales: 42, revenue: 1260 },
    { id: '2', name: 'Cat Toy Set', sales: 38, revenue: 950 },
    { id: '3', name: 'Pet Carrier', sales: 27, revenue: 1350 },
    { id: '4', name: 'Bird Cage', sales: 24, revenue: 1200 },
  ]
};

function Dashboard() {
  const [stats, setStats] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Simulate API call
    setTimeout(() => {
      setStats(mockStats)
      setLoading(false)
    }, 500)
  }, []);

  if (loading) {
    return (
        <div className="flex justify-center items-center h-full">Loading dashboard data...</div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {/* Stats Cards */}
        <div className="card bg-gradient-to-br from-primary to-pink-200">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-gray-600">Total Products</p>
              <h3 className="text-2xl font-bold">{stats.totalProducts}</h3>
            </div>
            <div className="bg-white p-3 rounded-full">
              <Package className="text-accent" />
            </div>
          </div>
          <div className="mt-4">
            <Link to="/products" className="text-sm font-medium hover:underline">
              View all products
            </Link>
          </div>
        </div>

        <div className="card bg-gradient-to-br from-primary to-pink-200">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-gray-600">Total Categories</p>
              <h3 className="text-2xl font-bold">{stats.totalCategories}</h3>
            </div>
            <div className="bg-white p-3 rounded-full">
              <Tag className="text-accent" />
            </div>
          </div>
          <div className="mt-4">
            <Link to="/categories" className="text-sm font-medium hover:underline">
              View all categories
            </Link>
          </div>
        </div>

        <div className="card bg-gradient-to-br from-primary to-pink-200">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-gray-600">Total Customers</p>
              <h3 className="text-2xl font-bold">{stats.totalCustomers}</h3>
            </div>
            <div className="bg-white p-3 rounded-full">
              <Users className="text-accent" />
            </div>
          </div>
          <div className="mt-4">
            <Link to="/customers" className="text-sm font-medium hover:underline">
              View all customers
            </Link>
          </div>
        </div>

        <div className="card bg-gradient-to-br from-primary to-pink-200">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-gray-600">Total Orders</p>
              <h3 className="text-2xl font-bold">{stats.totalOrders}</h3>
            </div>
            <div className="bg-white p-3 rounded-full">
              <ShoppingCart className="text-accent" />
            </div>
          </div>
          <div className="mt-4">
            <Link to="/orders" className="text-sm font-medium hover:underline">
              View all orders
            </Link>
          </div>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Recent Orders */}
        <div className="card">
          <div className="flex justify-between items-center mb-4">
            <h3 className="text-lg font-semibold">Recent Orders</h3>
            <Link to="/orders" className="text-sm text-primary hover:underline">View All</Link>
          </div>
          <div className="table-container">
            <table className="table">
              <thead>
                <tr>
                  <th>Order ID</th>
                  <th>Customer</th>
                  <th>Date</th>
                  <th>Status</th>
                  <th>Total</th>
                </tr>
              </thead>
              <tbody>
                {stats.recentOrders.map((order) => (
                  <tr key={order.id}>
                    <td>#{order.id}</td>
                    <td>{order.customer}</td>
                    <td>{order.date}</td>
                    <td>
                      <span className={`badge ${
                        order.status === 'Completed' ? 'badge-success' : 
                        order.status === 'Processing' ? 'badge-warning' : 'badge-danger'
                      }`}>
                        {order.status}
                      </span>
                    </td>
                    <td>${order.total.toFixed(2)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>

        {/* Top Products */}
        <div className="card">
          <div className="flex justify-between items-center mb-4">
            <h3 className="text-lg font-semibold">Top Selling Products</h3>
            <Link to="/products" className="text-sm text-primary hover:underline">View All</Link>
          </div>
          <div className="table-container">
            <table className="table">
              <thead>
                <tr>
                  <th>Product</th>
                  <th>Sales</th>
                  <th>Revenue</th>
                </tr>
              </thead>
              <tbody>
                {stats.topProducts.map((product) => (
                  <tr key={product.id}>
                    <td>{product.name}</td>
                    <td>{product.sales} units</td>
                    <td>${product.revenue.toFixed(2)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>

      {/* Revenue Overview */}
      <div className="card">
        <div className="flex justify-between items-center mb-4">
          <h3 className="text-lg font-semibold">Revenue Overview</h3>
          <div className="flex items-center gap-2">
            <TrendingUp className="text-green-500" size={20} />
            <span className="text-green-500 font-medium">+12.5%</span>
          </div>
        </div>
        <div className="flex items-center justify-center">
          <div className="text-center">
            <DollarSign size={48} className="mx-auto text-primary" />
            <h2 className="text-3xl font-bold mt-2">${stats.revenue.toLocaleString()}</h2>
            <p className="text-gray-500">Total Revenue</p>
          </div>
        </div>
      </div>
    </div>
  )
}

export default Dashboard